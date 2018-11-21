using ExampleService.Installers;
using log4net;
using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ExampleService
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
            {
                // running as service
                var service = DisposableService.Create(ServerInstaller.ServiceName, CreateServer);
                ServiceBase.Run(service);
            }
            else
            {
                if (args.Length == 1)
                {
                    switch (args[0])
                    {
                        case "-install":
                            {
                                var installer = new AssemblyInstaller(Assembly.GetExecutingAssembly(), new[] { "/LogFile=Uninstall.log" }) { UseNewContext = true };
                                var state = new Hashtable();
                                installer.Install(state);
                                installer.Commit(state);
                                return;
                            }
                        case "-uninstall":
                            {
                                var installer = new AssemblyInstaller(Assembly.GetExecutingAssembly(), new[] { "/LogFile=Uninstall.log" }) { UseNewContext = true };
                                var state = new Hashtable();
                                installer.Uninstall(state);
                                return;
                            }
                        default:
                            break;
                    }
                }

                // running as console app
                var server = CreateServer(args);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);

                server.Dispose();
            }
        }

        static MyServer CreateServer(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            var server = new MyServer();

            server.Start();

            return server;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Log.Fatal($"Unhandled error received - IsTerminating={args.IsTerminating}", args.ExceptionObject as Exception);
        }
    }
}
