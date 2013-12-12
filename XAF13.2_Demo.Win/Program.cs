using System;
using System.Configuration;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using Para.Modules.Win.TaskbarIntegration;
using Para.Modules.Win.TaskbarIntegration.Helpers;
using XAF13_2_Demo.Module.BusinessObjects;
using View = DevExpress.ExpressApp.View;

namespace XAF13_2_Demo.Win
{
    static class Program
    {
        private static WinApplication _Application;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] arguments)
        {
            const string applicationName = "XAF13.2_Demo";

            var guid = new Guid("B3F684D7-4CE8-471D-8E56-61BCC274EAA5");

            using (var instance = new SingleInstance(guid))
            {
                if (instance.IsFirstInstance)
                {
                    instance.ArgumentsReceived += TaskbarIntegrationWindowsFormsModule.InstanceOnArgumentsReceived;
                    instance.ListenForArgumentsFromSuccessiveInstances();

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    _Application = new DemoWindowsFormsApplication
                    {
                        ApplicationName = applicationName,
                        SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen()
                    };

                    TaskbarIntegrationWindowsFormsModule.TaskbarApplication = _Application;

                    if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                    {
                        _Application.ConnectionString =
                            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    }
                    else
                    {
                        InMemoryDataStoreProvider.Register();
                        _Application.ConnectionString = InMemoryDataStoreProvider.ConnectionString;
                    }

                    try
                    {
                        _Application.Setup();

                        _Application.Start();
                    }
                    catch (Exception e)
                    {
                        _Application.HandleException(e);
                    }
                }
                else
                {
                    instance.PassArgumentsToFirstInstance(arguments);
                }
            }

        }

     
    }
}
