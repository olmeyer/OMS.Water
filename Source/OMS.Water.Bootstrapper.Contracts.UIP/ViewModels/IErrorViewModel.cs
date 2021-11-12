using Systecs.Framework.WPF.ViewModels;

namespace OMS.Water.Bootstrapper.ViewModels
{
    public interface IErrorViewModel : IViewModel
    {
        int? ErrorCode { get; }

        string ErrorMessage { get; }
    }
}