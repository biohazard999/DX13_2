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
using Para.Modules.Win.TaskbarIntegration.Helpers;
using Para.Modules.Win.TaskbarIntegration.ResourceManagers;
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

            var manager = new RuntimeImageResourceManager();

            var imageAssembly = manager.WriteImageResouces(imageNames);
            

            TaskbarAssistant = new TaskbarAssistant();

            TaskbarAssistant.IconsAssembly = imageAssembly;

            foreach (var item in options.Jumplists.TasksCategory.OrderBy(m => m.Index))
            {
                InitJumpList(TaskbarAssistant.JumpListTasksCategory, item, manager);
            }

            foreach (var itemCategory in options.Jumplists.CustomCategories.OrderBy(m => m.Index))
            {
                var category = new JumpListCategory(itemCategory.Caption);

                foreach (var item in itemCategory)
                    InitJumpList(category.JumpItems, item, manager);

                TaskbarAssistant.JumpListCustomCategories.Add(category);
            }

            TaskbarAssistant.ParentControl = (Window as DevExpress.ExpressApp.Win.WinWindow).Form;
        }

     
        private void InitJumpList(JumpListCategoryItemCollection collection, IModelTaskbarJumplistItem item,
            RuntimeImageResourceManager manager)
        {
            if (item is IModelTaskbarJumplistJumpItemLaunch)
            {
                var launcher = item as IModelTaskbarJumplistJumpItemLaunch;

                collection.Add(new JumpListItemTask(launcher.Caption)
                {
                    Arguments = launcher.Arguments,
                    Path = launcher.PathToLaunch,
                    WorkingDirectory = launcher.WorkingDirectory,
                    IconIndex = manager.GetImageIndex(launcher.ImageName),
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
}


