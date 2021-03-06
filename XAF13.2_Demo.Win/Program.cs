using System;
using System.Configuration;
using System.Deployment.Application;
using System.Windows.Forms;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using Para.Modules.Win.TaskbarIntegration;

namespace XAF13_2_Demo.Win
{
    static class Program
    {
        private static WinApplication _Application;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MessageBox.Show("Wait for debugger");

            const string applicationName = "XAF13.2_Demo";

            var assemblyName = typeof(Program).Assembly.GetName();
            var mutexName = assemblyName.Name + "_" + assemblyName.Version.ToString(3);

#if DEBUG
            mutexName += "_Debug";
#endif
            using (var instance = new SingleInstance(mutexName))
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
                    instance.PassArgumentsToFirstInstance();
                }
            }

        }


    }
}
