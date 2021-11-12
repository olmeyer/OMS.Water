#region

using System;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework.Resources;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public class ApplyingInstallViewModel : ViewModel, IApplyingInstallViewModel
    {
        #region IApplyingInstallViewModel Members

        private IBootstrapperViewModel _bootstrapper;
        public IBootstrapperViewModel Bootstrapper
        {
            get { return _bootstrapper; }
            set
            {
                _bootstrapper = value;

                _bootstrapper.PropertyChanged += OnPropertyChanged;
                _bootstrapper.Localizer.CurrentUICultureChanged += OnCurrentUICultureChanged;
            }
        }


        void OnCurrentUICultureChanged(object sender, CultureEventArgs e)
        {
            UpdateMessage();
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var bootstrapper = sender as IBootstrapperViewModel;

            if (bootstrapper == null || e.PropertyName != "CurrentPackage" )
                return;

            UpdateMessage();
        }


        private void UpdateMessage()
        {
            if (Bootstrapper.CurrentPackage != null)
            {
                var packageName = Bootstrapper.CurrentPackage.PackageName;
                Message = string.Format(Bootstrapper.Localizer.GetString("ApplyingInstallView.InstallPackage"), packageName);
            }
            else
            {
                Message = string.Empty;
            }

        }

        public bool RunEmbedded()
        {
            return true;
        }

        #endregion


        private string _message;
        public string Message
        {
            get { return _message; }
            private set
            {
                _message = value;
                NotifyPropertyChanged( "Message" );
            }
        }
    }
}
