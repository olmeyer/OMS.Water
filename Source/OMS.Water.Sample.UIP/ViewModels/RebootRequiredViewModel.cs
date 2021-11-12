#region

using System.Windows.Input;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework.WPF.Commands;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public class RebootRequiredViewModel : ViewModel, IRebootRequiredViewModel
    {
        #region IInstallErrorViewModel Members

        public IBootstrapperViewModel Bootstrapper { get; set; }


        public bool RunEmbedded()
        {
            return true;
        }

        #endregion

        public ICommand Close
        {
            get { return Bootstrapper.Close; }
        }

        public ICommand Reboot
        {
            get { return Bootstrapper.Reboot; }
        }

    }
}
