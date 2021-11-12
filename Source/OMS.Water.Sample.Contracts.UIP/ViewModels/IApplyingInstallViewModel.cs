#region

using OMS.Water.Bootstrapper.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public interface IApplyingInstallViewModel : IInstallationViewModel
    {
        string Message { get; }
    }
}
