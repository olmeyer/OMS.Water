#region

using System;
using System.Windows.Input;
using OMS.Water.Bootstrapper;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework.WPF.Commands;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public class PrepareUninstallViewModel : ViewModel, IPrepareUninstallViewModel
    {
        #region IPrepareUninstallViewModel Members

        public IBootstrapperViewModel Bootstrapper { get; set; }


        public bool RunEmbedded()
        {
            DoUninstall();

            return true;
        }


        public ICommand UninstallCommand
        {
            get { return new DelegateCommand( DoUninstall ); }
        }

        #endregion


        private void DoUninstall()
        {
            Bootstrapper.Apply.Execute( ELaunchAction.Uninstall );
        }
    }
}
