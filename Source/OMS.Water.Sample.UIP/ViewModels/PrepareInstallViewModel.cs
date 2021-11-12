#region

using System;
using System.Threading;
using System.Windows.Input;
using OMS.Water.Bootstrapper;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework.WPF.Commands;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public class PrepareInstallViewModel : ViewModel, IPrepareInstallViewModel
    {
        #region IPrepareInstallViewModel Members

        public IBootstrapperViewModel Bootstrapper { get; set; }


        public bool RunEmbedded()
        {
            DoInstall();
            return true;
        }


        public ICommand InstallCommand
        {
            get { return new DelegateCommand( DoInstall ); }
        }

        #endregion


        private void DoInstall()
        {
            Bootstrapper.Apply.Execute( ELaunchAction.Install );
        }
    }
}
