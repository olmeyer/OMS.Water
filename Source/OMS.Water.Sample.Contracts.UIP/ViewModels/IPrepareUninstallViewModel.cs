#region

using System.Windows.Input;
using OMS.Water.Bootstrapper.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public interface IPrepareUninstallViewModel : IInstallationViewModel
    {
        ICommand UninstallCommand { get; }
    }
}
