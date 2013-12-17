using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win;

namespace Para.Modules.Win.TaskbarIntegration
{
    public class TaskbarJumpListHandleStartupItemController : WindowController
    {
        public TaskbarJumpListHandleStartupItemController()
        {
            TargetWindowType = WindowType.Main;
        }

        private void WinShowNavigationItemController_StartupWindowShown(object sender, EventArgs e)
        {
            ((WinShowViewStrategyBase)Application.ShowViewStrategy).StartupWindowShown -= WinShowNavigationItemController_StartupWindowShown;
            var controller = Window.GetController<ShowNavigationItemController>();
            ShowJumpListNavigationItem(controller);
        }

        public void ShowJumpListNavigationItem(ShowNavigationItemController controller)
        {
            var args = SingleInstance.Arguments;

            if (args.Length > 0)
            {
                var sc = TaskbarIntegrationWindowsFormsModule.CreateViewShortcutFromArguments(args[0]);

                var item = new ChoiceActionItem("CommandLineArgument", sc);
                controller.ShowNavigationItemAction.DoExecute(item);

                (Window as WinWindow).Form.BringToFront();
                (Window as WinWindow).Form.Focus();
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ((WinShowViewStrategyBase)Application.ShowViewStrategy).StartupWindowShown += WinShowNavigationItemController_StartupWindowShown;
        }

        protected override void OnDeactivated()
        {
            ((WinShowViewStrategyBase)Application.ShowViewStrategy).StartupWindowShown -= WinShowNavigationItemController_StartupWindowShown;
            base.OnDeactivated();
        }
    }
}