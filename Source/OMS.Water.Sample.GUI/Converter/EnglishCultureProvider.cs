#region

using System.Diagnostics;
using System.Globalization;
using System.Windows;

#endregion


namespace OMS.Water.Sample.Converter
{
    public static class EnglishCultureProvider
    {
        public static CultureInfo Culture
        {
            get
            {
                return (CultureInfo.InstalledUICulture.Equals( CultureInfo.GetCultureInfo( "en-US" )))
                               ? CultureInfo.GetCultureInfo("en-US")
                               : CultureInfo.GetCultureInfo( "en" );
            }
        }
    }
}
