using System;
using System.Configuration;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Xpo;
using Para.Modules.Win.TaskbarIntegration;
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

            //var s = string.Empty;

            //foreach (var i in arguments)
            //    s += "\n" + i;

            //MessageBox.Show("Arguments: " + s);

            using (var instance = new SingleInstance(guid))
            {
                if (instance.IsFirstInstance)
                {
                    instance.ArgumentsReceived += InstanceOnArgumentsReceived;
                    instance.ListenForArgumentsFromSuccessiveInstances();
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;

                    string[] args = Environment.GetCommandLineArgs();

                    _Application = new _2_DemoWindowsFormsApplication
                    {
                        ApplicationName = applicationName,
                        SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen()
                    };

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
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
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

        private static void InstanceOnArgumentsReceived(object sender, ArgumentsReceivedEventArgs argumentsReceivedEventArgs)
        {
            if (_Application == null || !(_Application.MainWindow is WinWindow))
                return;

            var window = _Application.MainWindow as WinWindow;

            var arguments = argumentsReceivedEventArgs.Args;

            window.Form.SafeInvoke(() => HandleShortCut(arguments));
        }

        private static void HandleShortCut(string[] arguments)
        {
            var os = _Application.CreateObjectSpace(typeof (XPViewCalculationProxy));

            var obj = os.FindObject<XPViewCalculationProxy>(CriteriaOperator.Parse("SumMode == ?", arguments[0]));

            if (obj != null)
            {
                var item = os.GetKeyValueAsString(obj);

                View shortCutView =
                    _Application.ProcessShortcut(new ViewShortcut(typeof (XPViewCalculationProxy), item,
                        _Application.FindDetailViewId(typeof (XPViewCalculationProxy))));

                _Application.ShowViewStrategy.ShowView(new ShowViewParameters(shortCutView), new ShowViewSource(null, null));
            }
        }
    }
}
