#region

using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Systecs.Framework.Resources;
using Systecs.Framework.WPF.Resources;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public class LanguageViewModel : ViewModel, ILanguageViewModel
    {
        public LanguageViewModel( ILocalizer localizer, CultureInfo culture )
        {
            Culture = culture;

            var icon = (Icon)localizer.GetObject( "Image.Flag", Culture, true );

            var ms = new MemoryStream();
            icon.Save( ms );
            ms.Position = 0;
            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.StreamSource = ms;
            imageSource.EndInit();

            ImageSource = imageSource;
        }


        #region ILanguageViewModel Members

        public CultureInfo Culture { get; set; }

        public ImageSource ImageSource { get; set; }

        #endregion
    }
}
