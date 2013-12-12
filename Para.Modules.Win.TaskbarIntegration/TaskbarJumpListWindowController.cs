using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Utils.OAuth;
using DevExpress.Utils.Taskbar;
using DevExpress.XtraGrid.Views.Base;
using Microsoft.CSharp;
using Romy.Core;
using Vestris.ResourceLib;

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

        private void TaskbarJumpListWindowController_Activated(object sender, EventArgs e)
        {
            if (JumplistOption == null)
                return;

            InitJumpList();
        }

        private IModelTaskbarJumplistOption JumplistOption
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

        private void Window_TemplateChanged(object sender, EventArgs e)
        {
            var options = JumplistOption;

            if (options == null)
                return;

            
            var imageNames =
                options.Jumplists.TasksCategory.OfType<IImageNameProvider>()
                    .Where(m => !String.IsNullOrEmpty(m.ImageName))
                    .Select(imageName => imageName.ImageName)
                    .ToList();

            foreach (var category in options.Jumplists.CustomCategories)
            {
                imageNames.AddRange(
                    category.OfType<IImageNameProvider>()
                        .Where(m => !String.IsNullOrEmpty(m.ImageName))
                        .Select(imageName => imageName.ImageName));
            }

            var imageAssembly = WriteImageResouces(imageNames);
            

            TaskbarAssistant = new TaskbarAssistant();
            

            TaskbarAssistant.IconsAssembly = imageAssembly;


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

            TaskbarAssistant.ParentControl = (Window as DevExpress.ExpressApp.Win.WinWindow).Form;
        }

        private readonly Dictionary<string, int> ImageIndexes = new Dictionary<string, int>();

        private string WriteImageResouces(IEnumerable<string> imageNames)
        {
            var location = Path.GetDirectoryName(typeof(TaskbarJumpListWindowController).Assembly.Location);
            var assemblyFileName = "AutoGen.dll";
            
            var assemblyPath = Path.Combine(location, assemblyFileName);
            
            var codeProvider = new CSharpCodeProvider();
            var icc = codeProvider.CreateCompiler();
            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = assemblyFileName;

            assemblyPath = Path.Combine(location, parameters.OutputAssembly);

            CompilerResults results = icc.CompileAssemblyFromDom(parameters, new CodeCompileUnit());

            if (results.Errors.HasErrors)
                return null;

            foreach (var imageName in imageNames)
            {
                var info = ImageLoader.Instance.GetImageInfo(imageName);
                var infoLarge = ImageLoader.Instance.GetImageInfo(imageName);

                if (info == null || info.Image == null || infoLarge == null || infoLarge.Image == null)
                    continue;

                var writer = new IconFileWriter();

                writer.Images.Add(info.Image);
                writer.Images.Add(infoLarge.Image);

                writer.Save(imageName + ".ico");
            }

            using (var scope = new FilesScope(imageNames.Select(m => m + ".ico")))
            {
                uint c = 100;
                uint id = 1;
                    
                foreach (var file in scope.FilesNames)
                {
                    var rc = new IconDirectoryResource();
                    
                    rc.Name = new ResourceId(c);
                    rc.Language = GetCurrentLangId();

                    var iconFile = new IconFile(file);

                    foreach (var icon in iconFile.Icons)
                    {
                        rc.Icons.Add(new IconResource(icon, new ResourceId(id), rc.Language));
                    }

                    rc.SaveTo(assemblyFileName);
                    
                    ImageIndexes[Path.GetFileNameWithoutExtension(file)] = Convert.ToInt32(id) -1;

                    c++;
                    id++;
                }
            }

            Assembly.Load(results.CompiledAssembly.FullName);

            return assemblyPath;
        }

        private ushort GetCurrentLangId()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            int pid = PRIMARYLANGID(currentCulture.LCID);
            int sid = SUBLANGID(currentCulture.LCID);
            return (ushort)MAKELANGID(pid, sid);
        }

        public static int MAKELANGID(int primary, int sub)
        {
            return (((ushort)sub) << 10) | ((ushort)primary);
        }

        public static int PRIMARYLANGID(int lcid)
        {
            return ((ushort)lcid) & 0x3ff;
        }

        public static int SUBLANGID(int lcid)
        {
            return ((ushort)lcid) >> 10;
        }

        private void InitJumpList(JumpListCategoryItemCollection collection, IModelTaskbarJumplistItem item,
            List<string> imageNames)
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

        private void TaskbarJumpListWindowController_Deactivated(object sender, EventArgs e)
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

    public class FilesScope : IDisposable
    {
        private readonly List<FileScope> _scopes = new List<FileScope>();

        public FilesScope(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                _scopes.Add(new FileScope(file));
            }
        }

        public IEnumerable<string> FilesNames
        {
            get { return _scopes.Select(m => m.FileName); }
        }

        public void Dispose()
        {
            foreach (var fileScope in _scopes)
            {
                fileScope.Dispose();
            }
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
                //if (File.Exists(FileName))
                //    File.Delete(FileName);
            }
            catch
            {

            }
        }
    }
}


