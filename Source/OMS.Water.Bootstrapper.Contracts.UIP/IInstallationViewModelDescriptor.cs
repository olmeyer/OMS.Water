#region

using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Bootstrapper
{
    public interface IInstallationViewModelDescriptor : IViewModelDescriptor
    {
        EInstallationState InstallationState { get; }

        EInstallationMode InstallationMode { get; }
    }
}
