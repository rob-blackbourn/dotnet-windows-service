using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ExampleService.Installers
{
    [RunInstaller(true)]
    public class ServerInstaller : Installer
    {
        public const string ServiceName = "ExampleServer";
        public const string DisplayName = "JetBlack Example Service";
        public const string Description = "JetBlack Example Service";

        public ServerInstaller()
        {
            // Service Account Information
            Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.NetworkService
            });

            // Service Information
            Installers.Add(new ServiceInstaller
            {
                DisplayName = DisplayName,
                StartType = ServiceStartMode.Manual,
                ServiceName = ServiceName,
                Description = Description
            });
        }
    }
}
