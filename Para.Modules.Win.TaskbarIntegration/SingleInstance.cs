using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;

namespace Para.Modules.Win.TaskbarIntegration
{
    /// <summary>
    ///     Enforces single instance for an application.
    /// </summary>
    public class SingleInstance : IDisposable
    {
        private readonly Boolean _OwnsMutex;
        private Guid _Identifier = Guid.Empty;
        private Mutex _Mutex;

        /// <summary>
        ///     Enforces single instance for an application.
        /// </summary>
        /// <param name="identifier">An identifier unique to this application.</param>
        public SingleInstance(Guid identifier)
        {
            _Identifier = identifier;
            _Mutex = new Mutex(true, identifier.ToString(), out _OwnsMutex);
        }

        /// <summary>
        ///     Indicates whether this is the first instance of this application.
        /// </summary>
        public Boolean IsFirstInstance
        {
            get { return _OwnsMutex; }
        }

        /// <summary>
        ///     Passes the given arguments to the first running instance of the application.
        /// </summary>
        /// <param name="arguments">The arguments to pass.</param>
        /// <returns>Return true if the operation succeded, false otherwise.</returns>
        private Boolean PassArgumentsToFirstInstance(String[] arguments)
        {
            if (arguments == null)
                return false;

            if (IsFirstInstance)
                throw new InvalidOperationException("This is the first instance.");

            try
            {
                using (var client = new NamedPipeClientStream(_Identifier.ToString()))
                using (var writer = new StreamWriter(client))
                {
                    client.Connect(200);

                    foreach (String argument in arguments)
                    {
                        if (!string.IsNullOrEmpty(argument))
                        {
                            writer.WriteLine(argument);
                        }
                    }

                }
                return true;
            }
            catch (TimeoutException)
            {
            } //Couldn't connect to server
            catch (IOException)
            {
            } //Pipe was broken

            return false;
        }

        /// <summary>
        ///     Listens for arguments being passed from successive instances of the applicaiton.
        /// </summary>
        public void ListenForArgumentsFromSuccessiveInstances()
        {
            if (!IsFirstInstance)
                throw new InvalidOperationException("This is not the first instance.");
            ThreadPool.QueueUserWorkItem(ListenForArguments);
        }

        /// <summary>
        ///     Listens for arguments on a named pipe.
        /// </summary>
        /// <param name="state">State object required by WaitCallback delegate.</param>
        private void ListenForArguments(Object state)
        {
            try
            {
                using (var server = new NamedPipeServerStream(_Identifier.ToString()))
                using (var reader = new StreamReader(server))
                {
                    server.WaitForConnection();

                    var arguments = new List<String>();
                    while (server.IsConnected)
                    {
                        string result = reader.ReadLine();

                        if (!string.IsNullOrEmpty(result))
                            arguments.Add(result);
                    }


                    ThreadPool.QueueUserWorkItem(CallOnArgumentsReceived, arguments.ToArray());
                }
            }
            catch (IOException)
            {
            } //Pipe was broken
            finally
            {
                ListenForArguments(null);
            }
        }

        /// <summary>
        ///     Calls the OnArgumentsReceived method casting the state Object to String[].
        /// </summary>
        /// <param name="state">The arguments to pass.</param>
        private void CallOnArgumentsReceived(Object state)
        {
            OnArgumentsReceived((String[])state);
        }

        /// <summary>
        ///     Event raised when arguments are received from successive instances.
        /// </summary>
        public event EventHandler<ArgumentsReceivedEventArgs> ArgumentsReceived;

        /// <summary>
        ///     Fires the ArgumentsReceived event.
        /// </summary>
        /// <param name="arguments">The arguments to pass with the ArgumentsReceivedEventArgs.</param>
        private void OnArgumentsReceived(String[] arguments)
        {
            if (ArgumentsReceived != null)
                ArgumentsReceived(this, new ArgumentsReceivedEventArgs { Args = arguments });
        }

        #region IDisposable

        private Boolean disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (_Mutex != null && _OwnsMutex)
                {
                    _Mutex.ReleaseMutex();
                    _Mutex = null;
                }
                disposed = true;
            }
        }

        ~SingleInstance()
        {
            Dispose(false);
        }

        #endregion

        public void PassArgumentsToFirstInstance()
        {
            PassArgumentsToFirstInstance(Arguments);
        }

        private static string[] FilterAndAppendSlash(IEnumerable<string> args)
        {
            var listOfArgs = new List<string>();
            foreach (var argument in args)
            {
                if (!string.IsNullOrEmpty(argument))
                {
                    if (Regex.IsMatch(argument, @"[\w-]{3,}://")) //Is Uri-Protocol
                        listOfArgs.Add(argument);
                    else
                    {
                        var arg = argument;
                        if (!arg.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
                            arg = "/" + arg;
                        listOfArgs.Add(arg);
                    }
                }
            }
            return listOfArgs.ToArray();
        }

        public static string[] Arguments
        {
            get
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    var args = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;

                    if (args.Length >= 1)
                        args = args.Except(new[] { ApplicationDeployment.CurrentDeployment.UpdateLocation.ToString() }).ToArray();

                    return FilterAndAppendSlash(args);
                }

                var environmentArgs = Environment.GetCommandLineArgs();

                if (environmentArgs.Length >= 2)
                {
                    return FilterAndAppendSlash(environmentArgs.Skip(1));
                }

                return new string[0];
            }
        }
    }
}