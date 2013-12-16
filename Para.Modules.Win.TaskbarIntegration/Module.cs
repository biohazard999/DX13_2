using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.Persistent.Base;
using Para.Modules.Win.TaskbarIntegration.Helpers;
using View = DevExpress.ExpressApp.View;

namespace Para.Modules.Win.TaskbarIntegration
{
    public sealed class TaskbarIntegrationWindowsFormsModule : ModuleBase
    {
        protected override ModuleTypeList GetRequiredModuleTypesCore()
        {
            return new ModuleTypeList(
                typeof(SystemModule),
                typeof(SystemWindowsFormsModule)
                );
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            return ModuleUpdater.EmptyModuleUpdaters;
        }

        protected override IEnumerable<Type> GetDeclaredControllerTypes()
        {
            return new[]
            {
                typeof(TaskbarJumpListWindowController),
                typeof(TaskbarJumpListHandleStartupItemController),
            };
        }

        protected override IEnumerable<Type> GetDeclaredExportedTypes()
        {
            return Type.EmptyTypes;
        }

        protected override void RegisterEditorDescriptors(List<EditorDescriptor> editorDescriptors)
        {

        }

        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelOptions, IModelTaskbarOptions>();
        }

        public static void InstanceOnArgumentsReceived(object sender, ArgumentsReceivedEventArgs argumentsReceivedEventArgs)
        {
            if (TaskbarApplication == null || !(TaskbarApplication.MainWindow is WinWindow))
                return;

            var window = TaskbarApplication.MainWindow as WinWindow;

            var arguments = argumentsReceivedEventArgs.Args;

            if (arguments.Length > 0)
                window.Form.SafeInvoke(() => HandleShortCut(arguments[0]));
        }

        public static void HandleShortCut(string argument)
        {
            var sc = ViewShortcut.FromString(argument);

            View shortCutView =
                TaskbarApplication.ProcessShortcut(sc);

            TaskbarApplication.ShowViewStrategy.ShowView(new ShowViewParameters(shortCutView), new ShowViewSource(null, null));

        }

        public static WinApplication TaskbarApplication { get; set; }
    }

    public interface IModelTaskbarOptions : IModelNode
    {
        IModelTaskbarJumplistOption TaskbarJumplistOptions { get; }
    }

    public interface IModelTaskbarJumplistOption : IModelNode
    {
        bool EnableJumpList { get; set; }

        IModelTaskbarJumplists Jumplists { get; }

        [DefaultValue("JumplistImages.dll")]
        string AutomaticImageAssemblyName { get; set; }
    }

    public interface IModelTaskbarJumplists : IModelNode
    {
        IModelTaskbarJumplistTaskCategory TasksCategory { get; }

        IModelTaskbarJumplistCustomCategories CustomCategories { get; }
    }

    public interface IModelTaskbarJumplistTaskCategory : IModelNode, IModelList<IModelTaskbarJumplistItem>
    {

    }

    public interface IModelTaskbarJumplistCustomCategories : IModelNode, IModelList<IModelTaskbarJumplistCustomCategory>
    {

    }

    public interface IModelTaskbarJumplistCustomCategory : IModelNode, IModelList<IModelTaskbarJumplistItem>
    {
        string Caption { get; set; }
    }

    [ModelAbstractClass]
    public interface IModelTaskbarJumplistItem : IModelNode
    {

    }

    [ModelAbstractClass]
    public interface IModelTaskbarJumplistJumpItemBase : IModelTaskbarJumplistItem
    {
        string Caption { get; set; }

        [Editor("DevExpress.ExpressApp.Win.Core.ModelEditor.ImageGalleryModelEditorControl, DevExpress.ExpressApp.Win" + XafAssemblyInfo.VersionSuffix + XafAssemblyInfo.AssemblyNamePostfix, typeof(UITypeEditor))]
        string ImageName { get; set; }
    }

    public interface IModelTaskbarJumplistJumpItemLaunch : IModelTaskbarJumplistJumpItemBase
    {
        string PathToLaunch { get; set; }
        string Arguments { get; set; }

        string WorkingDirectory { get; set; }
    }

    public interface IModelTaskbarJumplistJumpItemNavigationItem : IModelTaskbarJumplistJumpItemBase
    {
        [DataSourceProperty("Application.NavigationItems.AllItems")]
        IModelNavigationItem NavigationItem { get; set; }
    }

    [DomainLogic(typeof(IModelTaskbarJumplistJumpItemNavigationItem))]
    public static class ModelTaskbarJumplistJumpItemNavigationItem_Logic
    {
        public static string Get_Caption(IModelTaskbarJumplistJumpItemNavigationItem item)
        {
            if (item == null)
                return null;
            if (item.NavigationItem != null)
                return item.NavigationItem.Caption;
            return null;
        }

        public static string Get_ImageName(IModelTaskbarJumplistJumpItemNavigationItem item)
        {
            if (item == null)
                return null;
            if (item.NavigationItem != null)
                return item.NavigationItem.ImageName;
            return null;
        }
    }

    public interface IModelTaskbarJumplistSeperatorItem : IModelTaskbarJumplistItem
    {

    }
}
