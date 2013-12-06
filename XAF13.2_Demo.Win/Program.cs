using System;
using System.Configuration;
using System.Windows.Forms;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;

namespace XAF13_2_Demo.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            _2_DemoWindowsFormsApplication winApplication = new _2_DemoWindowsFormsApplication();
            // Refer to the http://documentation.devexpress.com/#Xaf/CustomDocument2680 help article for more details on how to provide a custom splash form.
            winApplication.SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen();
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                winApplication.ConnectionString =
                    ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            else
            {
                InMemoryDataStoreProvider.Register();
                winApplication.ConnectionString = InMemoryDataStoreProvider.ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            try
            {
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }
}
