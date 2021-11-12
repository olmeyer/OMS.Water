#region

using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Bootstrapper
{
    public class InstallationViewModelDescriptor<TViewModel, TView> : ViewModelDescriptor<TViewModel, TView>, IInstallationViewModelDescriptor
            where TViewModel : IInstallationViewModel
    {
        public InstallationViewModelDescriptor( EInstallationState installationState )
                : this( installationState, EInstallationMode.None )
        {
        }


        public InstallationViewModelDescriptor( EInstallationState installationState, EInstallationMode installationMode )
        {
            InstallationState = installationState;
            InstallationMode = installationMode;
        }


        #region IInstallationViewModelDescriptor Members

        public EInstallationState InstallationState { get; private set; }

        public EInstallationMode InstallationMode { get; private set; }

        #endregion
    }
}
