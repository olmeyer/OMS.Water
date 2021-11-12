using Systecs.Framework.WPF.ViewModels;
namespace OMS.Water.Bootstrapper.ViewModels
{
    public class ErrorViewModel : ViewModel, IErrorViewModel
    {
        public ErrorViewModel(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public int? ErrorCode { get; private set; }

        public string ErrorMessage { get; private set; }
    }
}