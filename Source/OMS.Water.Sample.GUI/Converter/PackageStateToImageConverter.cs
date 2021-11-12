#region

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using OMS.Water.Bootstrapper;

#endregion


namespace OMS.Water.Sample.Converter
{
    public class PackageStateToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var state = (EPackageState)value;

            var assemblyName = Assembly.GetExecutingAssembly().FullName;
            var resourceName = state == EPackageState.DetectedPresent ? "ButtonGreen" : "ButtonRed";
            var resourceUri = new Uri( string.Format( "{0};Component/Resources/{1}.xaml", assemblyName, resourceName ), UriKind.Relative );

            var dict = Application.LoadComponent( resourceUri ) as ResourceDictionary;
            //Get the xaml as a DrawingImage out the ResourceDictionary  
            var image = dict[dict.Keys.Cast<object>().ToList()[0]] as DrawingImage;

            return image;
        }


        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
