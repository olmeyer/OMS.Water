#region

using System;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public class InstallCanceledViewModel : ViewModel, IInstallCanceledViewModel
    {
        #region IInstallCanceledViewModel Members

        public IBootstrapperViewModel Bootstrapper { get; set; }


        public bool RunEmbedded()
        {
            return true;
        }

        #endregion
    }
}
