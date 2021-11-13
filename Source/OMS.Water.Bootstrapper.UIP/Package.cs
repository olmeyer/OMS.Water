#region

using System;
//using Microsoft.Practices.ServiceLocation;
//using Systecs.Framework.Resources;

#endregion


namespace OMS.Water.Bootstrapper
{
    internal class Package : PropertyNotifyBase, IPackage
    {
        private string _packageId;
        private string _packageName;
        private bool _perMachine;
        private string _productCode;

        private IPackage _relatedPackage;

        private EPackageState _state;
        private Version _version;

        //private static ILocalizer s_localizer ;

        static Package()
        {
            //s_localizer = ServiceLocator.Current.GetInstance<ILocalizer>();
        }

        #region IPackage Members

        public string PackageId
        {
            get { return _packageId; }
            set
            {
                if( _packageId != value )
                {
                    _packageId = value;
                    OnPropertyChanged( "PackageId" );
                }
            }
        }

        public string PackageName
        {
            get { return _packageName; }
            set
            {
                if( _packageName != value )
                {
                    _packageName = value;
                    OnPropertyChanged( "PackageName" );
                }
            }
        }

        private string _installHeader;
        public string InstallHeader
        {
            get { return _installHeader; }
            set
            {
                if (_installHeader != value)
                {
                    _installHeader = value;
                    OnPropertyChanged("InstallHeader");
                }
            }
        }

        private string _uninstallHeader;
        public string UninstallHeader
        {
            get { return _uninstallHeader; }
            set
            {
                if (_uninstallHeader != value)
                {
                    _uninstallHeader = value;
                    OnPropertyChanged("UninstallHeader");
                }
            }
        }

        private string _installDescription;
        public string InstallDescription
        {
            get { return _installDescription; }
            set
            {
                if (_installDescription != value)
                {
                    _installDescription = value;
                    OnPropertyChanged("InstallDescription");
                }
            }
        }

        private string _uninstallDescription;
        public string UninstallDescription
        {
            get { return _uninstallDescription; }
            set
            {
                if (_uninstallDescription != value)
                {
                    _uninstallDescription = value;
                    OnPropertyChanged("UninstallDescription");
                }
            }
        }



        public bool PerMachine
        {
            get { return _perMachine; }
            set
            {
                if( _perMachine != value )
                {
                    _perMachine = value;
                    OnPropertyChanged( "PerMachine" );
                }
            }
        }

        public string ProductCode
        {
            get { return _productCode; }
            set
            {
                if( _productCode != value )
                {
                    _productCode = value;
                    OnPropertyChanged( "ProductCode" );
                }
            }
        }

        public Version Version
        {
            get { return _version; }
            set
            {
                if( _version != value )
                {
                    _version = value;
                    OnPropertyChanged( "Version" );
                }
            }
        }

        public EPackageState State
        {
            get { return _state; }
            set
            {
                if( _state != value )
                {
                    _state = value;
                    OnPropertyChanged( "State" );
                }
            }
        }

        public IPackage RelatedPackage
        {
            get { return _relatedPackage; }
            set
            {
                if( _relatedPackage != value )
                {
                    _relatedPackage = value;
                    OnPropertyChanged( "RelatedPackage" );
                }
            }
        }

        #endregion
    }
}
