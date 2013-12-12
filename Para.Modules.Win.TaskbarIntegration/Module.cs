using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win.SystemModule;

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
            return new [] {typeof(TaskbarJumpListWindowController)};
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
    }

    public interface IModelTaskbarOptions : IModelNode
    {
       IModelTaskbarJumplistOption TaskbarJumplistOptions { get;  }
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

        [Editor("DevExpress.ExpressApp.Win.Core.ModelEditor.ImageGalleryModelEditorControl, DevExpress.ExpressApp.Win" + XafAssemblyInfo.VersionSuffix + XafAssemblyInfo.AssemblyNamePostfix, typeof(System.Drawing.Design.UITypeEditor))]
        string ImageName { get; set; }
    }

    public interface IModelTaskbarJumplistJumpItemLaunch : IModelTaskbarJumplistJumpItemBase
    {
        string PathToLaunch { get; set; }
        string Arguments { get; set; }

        string WorkingDirectory { get; set; }
    }

    public interface IModelTaskbarJumplistSeperatorItem : IModelTaskbarJumplistItem
    {

    }
}
