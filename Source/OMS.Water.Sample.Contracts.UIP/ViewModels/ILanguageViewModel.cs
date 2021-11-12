#region

using System.Globalization;
using System.Windows.Media;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public interface ILanguageViewModel : IViewModel
    {
        CultureInfo Culture { get; }

        ImageSource ImageSource { get; }
    }
}
