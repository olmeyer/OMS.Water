#region

using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Bootstrapper.ViewModels
{
    public interface IInstallationViewModel : IViewModel
    {
        IBootstrapperViewModel Bootstrapper { get; set; }


        bool RunEmbedded();
    }
}
