using DevExpress.ExpressApp.DC;
using Para.Modules.Win.TaskbarIntegration.Model;

namespace Para.Modules.Win.TaskbarIntegration
{
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
}