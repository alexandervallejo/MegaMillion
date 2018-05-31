using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MegaMillion.Service
{
    [RunInstaller(true)]
    public partial class MegaMillion : System.Configuration.Install.Installer
    {
        // Fields
        private ServiceInstaller serviceInstallerMegaMillion = new ServiceInstaller();
        private ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
        private ServiceController serviceController1 = new ServiceController();

        // Methods
        public MegaMillion()
        {
            this.processInstaller.Account = ServiceAccount.LocalSystem;
            this.serviceInstallerMegaMillion.StartType = ServiceStartMode.Automatic;
            this.serviceInstallerMegaMillion.ServiceName = "Mega Million Service Monitor";
            base.Installers.Add(this.serviceInstallerMegaMillion);
            base.Installers.Add(this.processInstaller);
        }
    }
}
