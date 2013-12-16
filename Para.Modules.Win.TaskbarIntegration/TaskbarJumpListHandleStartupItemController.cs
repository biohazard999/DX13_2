﻿using System;
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
            ShowStartupNavigationItem(controller);
        }
        protected virtual void ShowStartupNavigationItem(ShowNavigationItemController controller)
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length >= 2)
            {
                var sc = ViewShortcut.FromString(args[1]);

                var item = new ChoiceActionItem("CommandLineArgument", sc);
                controller.ShowNavigationItemAction.DoExecute(item);
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