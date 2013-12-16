using System.Configuration;
using System.Diagnostics;
using System.Web;

namespace ParaOffice.Win.ProtocolHandlerBootStrapper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Command line parsing
            var cmdArgs = new Arguments(args);
            string path = GetAppUrlWithQueryString(GetClickOnceUrl(), cmdArgs);
            LaunchProcess(path);
        }

        /// <summary>
        ///     Launches the process.
        /// </summary>
        /// <param name="path">The path.</param>
        private static void LaunchProcess(string path)
        {
            var startInfo = new ProcessStartInfo("IExplore.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.Arguments = path;
            Process.Start(startInfo);
        }

        /// <summary>
        ///     Gets the app URL with query string.
        /// </summary>
        /// <param name="clickOnceApplicationUrl">The click once application URL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private static string GetAppUrlWithQueryString(string clickOnceApplicationUrl, Arguments args)
        {
            string formattedArgs = string.Format("{0}?", clickOnceApplicationUrl);
            foreach (string key in args.Keys)
            {
                formattedArgs += string.Format("{0}={1}&", key, args[key]);
            }

            formattedArgs = HttpUtility.UrlEncode(formattedArgs.Substring(0, formattedArgs.Length));

            //MessageBox.Show(formattedArgs);
            //-- write the new file... 
            //   important NOTE: it has to be unicode or
            //   it won't work when executed! wierd...
            return formattedArgs;
        }


        /// <summary>
        ///     Gets the click once URL.
        /// </summary>
        /// <returns></returns>
        public static string GetClickOnceUrl()
        {
            return ConfigurationSettings.AppSettings["ClickOnceApplicationUrl"];
        }

        /// <summary>
        ///     Gets the protocol tag.
        ///     Note - you may wish to use this to parse it out of the cmd args...
        /// </summary>
        /// <returns></returns>
        public static string GetProtocolTag()
        {
            return ConfigurationSettings.AppSettings["ProtocolTag"];
        }
    }
}