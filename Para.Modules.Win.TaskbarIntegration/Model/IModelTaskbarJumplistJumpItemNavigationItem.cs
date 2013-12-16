using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;

namespace Para.Modules.Win.TaskbarIntegration.Model
{
    public interface IModelTaskbarJumplistJumpItemNavigationItem : IModelTaskbarJumplistJumpItemBase
    {
        [DataSourceProperty("Application.NavigationItems.AllItems")]
        IModelNavigationItem NavigationItem { get; set; }
    }
}