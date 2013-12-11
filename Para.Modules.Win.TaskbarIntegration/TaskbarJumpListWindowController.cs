using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Utils.Taskbar;
using DevExpress.XtraGrid.Views.Base;
using Microsoft.CSharp;

namespace Para.Modules.Win.TaskbarIntegration
{
    public class TaskbarJumpListWindowController : WindowController
    {
        public TaskbarJumpListWindowController()
        {
            TargetWindowType = WindowType.Main;
            Activated += TaskbarJumpListWindowController_Activated;
            Deactivated += TaskbarJumpListWindowController_Deactivated;
        }

        void TaskbarJumpListWindowController_Activated(object sender, EventArgs e)
        {
            if (JumplistOption == null)
                return;

            InitJumpList();
        }

        IModelTaskbarJumplistOption JumplistOption
        {
            get
            {
                if (Application == null)
                    return null;
                if (Application.Model == null || Application.Model.Options == null)
                    return null;

                var optionsTaskbar = Application.Model.Options as IModelTaskbarOptions;
                if (optionsTaskbar == null || optionsTaskbar.TaskbarJumplistOptions == null)
                    return null;

                if (!optionsTaskbar.TaskbarJumplistOptions.EnableJumpList)
                    return null;

                return (Application.Model.Options as IModelTaskbarOptions).TaskbarJumplistOptions;
            }
        }


        private void InitJumpList()
        {
            Window.TemplateChanged -= Window_TemplateChanged;
            Window.TemplateChanged += Window_TemplateChanged;
        }

        void Window_TemplateChanged(object sender, EventArgs e)
        {
            var options = JumplistOption;

            if (options == null)
                return;

            TaskbarAssistant = new TaskbarAssistant();
            TaskbarAssistant.ParentControl = (Window as DevExpress.ExpressApp.Win.WinWindow).Form;

            var imageNames = options.Jumplists.TasksCategory.OfType<IImageNameProvider>().Where(m => !String.IsNullOrEmpty(m.ImageName)).Select(imageName => imageName.ImageName).ToList();

            foreach (var category in options.Jumplists.CustomCategories)
            {
                imageNames.AddRange(category.OfType<IImageNameProvider>().Where(m => !String.IsNullOrEmpty(m.ImageName)).Select(imageName => imageName.ImageName));
            }

            TaskbarAssistant.IconsAssembly = WriteImageResouces(imageNames);


            foreach (var item in options.Jumplists.TasksCategory.OrderBy(m => m.Index))
            {
                InitJumpList(TaskbarAssistant.JumpListTasksCategory, item, imageNames);
            }

            foreach (var itemCategory in options.Jumplists.CustomCategories.OrderBy(m => m.Index))
            {
                var category = new JumpListCategory(itemCategory.Caption);

                foreach (var item in itemCategory)
                    InitJumpList(category.JumpItems, item, imageNames);

                TaskbarAssistant.JumpListCustomCategories.Add(category);
            }
        }

        private string WriteImageResouces(IEnumerable<string> imageNames)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = "AutoGen.dll";
            CompilerResults results = icc.CompileAssemblyFromDom(parameters, new CodeCompileUnit());

            if (results.Errors.HasErrors)
                return null;

            foreach (var imageName in imageNames)
            {
                var info = ImageLoader.Instance.GetImageInfo(imageName);
                if (info == null || info.Image == null)
                    continue;

                using (var iconStream = File.Create(imageName + ".ico"))
                {
                    using (var bitmap = new Bitmap(info.Image))
                    {
                        Icon.FromHandle(bitmap.GetHicon()).Save(iconStream);
                    }
                }

                using (var scope = new FileScope(imageName + ".ico"))
                {
                    using (var p = new Process())
                    {
                        p.StartInfo = new ProcessStartInfo("AddResource.exe", "AutoGen.dll " + scope.FileName)
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false,
                        };
                        p.Start();
                        p.WaitForExit();
                    }
                }
            }

            var location = Path.GetDirectoryName(typeof (TaskbarJumpListWindowController).Assembly.Location);
            var assemblyPath = Path.Combine(location, parameters.OutputAssembly);
            var assembly = Assembly.LoadFile(assemblyPath);

            return assemblyPath;
        }

        private void InitJumpList(JumpListCategoryItemCollection collection, IModelTaskbarJumplistItem item, List<string> imageNames)
        {
            if (item is IModelTaskbarJumplistJumpItemLaunch)
            {
                var launcher = item as IModelTaskbarJumplistJumpItemLaunch;

                collection.Add(new JumpListItemTask(launcher.Caption)
                {
                    Arguments = launcher.Arguments,
                    Path = launcher.PathToLaunch,
                    WorkingDirectory = launcher.WorkingDirectory,
                    IconIndex = imageNames.IndexOf(launcher.ImageName),
                });
            }

            if (item is IModelTaskbarJumplistSeperatorItem)
            {
                collection.Add(new JumpListItemSeparator());
            }
        }

        public TaskbarAssistant TaskbarAssistant { get; set; }

        void TaskbarJumpListWindowController_Deactivated(object sender, EventArgs e)
        {
            if (TaskbarAssistant != null)
            {
                TaskbarAssistant.Dispose();
                TaskbarAssistant = null;
            }
            if (Window != null)
                Window.TemplateChanged -= Window_TemplateChanged;
        }
    }

    public class FileScope : IDisposable
    {
        private readonly string _FileName;

        public FileScope(string fileName)
        {
            _FileName = fileName;
        }

        public string FileName
        {
            get { return _FileName; }
        }

        public void Dispose()
        {
            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
            }
            catch
            {

            }
        }
    }
}
