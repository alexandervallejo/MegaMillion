using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MegaMillion.Service
{
    static class Program
    {
        // Fields
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string ServiceName = "MegaMillion Monitor Service";

        // Methods
        private static void Main(string[] args)
        {
            Console.WriteLine("Type -install to install and -uninstall to uninstall...");
            if (!Environment.UserInteractive)
            {
                using (Service service = new Service())
                {
                    ServiceBase.Run(service);
                }
            }
            else
            {
                Start(args);
                switch (Console.ReadLine())
                {
                    case "-install":
                        {
                            string[] installArgs = new string[] { Assembly.GetExecutingAssembly().Location };
                            ManagedInstallerClass.InstallHelper(installArgs);
                            break;
                        }
                    case "-uninstall":
                        {
                            string[] installArgs = new string[] { "/u", Assembly.GetExecutingAssembly().Location };
                            ManagedInstallerClass.InstallHelper(installArgs);
                            break;
                        }
                }
                Console.WriteLine("Press any key to stop...");
                Console.ReadLine();
                Stop();
            }
        }

        private static void Start(string[] args)
        {
            XmlConfigurator.Configure();
            log.Info($"{"MegaMillion Monitor Service"} is starting...");
            Task.Run(delegate {
                try
                {
                    new MonitorService(args).Initialize();
                }
                catch (Exception exception)
                {
                    log.Error("Start", exception);
                }
            });
        }

        private static void Stop()
        {
            try
            {
                log.Info($"{"MegaMillion Monitor Service"} is stopping...");
            }
            catch (Exception exception)
            {
                log.Error("Stop", exception);
            }
        }

        // Nested Types
        public class Service : ServiceBase
        {
            // Methods
            public Service()
            {
                base.ServiceName = base.ServiceName;
            }

            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
                base.OnStop();
            }
        }

    }
}
