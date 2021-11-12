#region

using System;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public class InstallCompletedViewModel : ViewModel, IInstallCompletedViewModel
    {
        #region IInstallCompletedViewModel Members

        public IBootstrapperViewModel Bootstrapper { get; set; }


        public bool RunEmbedded()
        {
            return true;
        }

        #endregion
    }
}
