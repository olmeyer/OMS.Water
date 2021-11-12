#region

using System.Windows.Input;
using OMS.Water.Bootstrapper.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public interface IRebootRequiredViewModel : IInstallationViewModel
    {
        ICommand Close { get; }

        ICommand Reboot { get; }
    }
}
