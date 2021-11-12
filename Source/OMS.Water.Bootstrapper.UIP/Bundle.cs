#region

using System;

#endregion


namespace OMS.Water.Bootstrapper
{
    internal class Bundle : PropertyNotifyBase, IBundle
    {
        private bool _perMachine;
        private string _productCode;
        private string _tag;
        private Version _version;


        #region IBundle Members

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

        public string Tag
        {
            get { return _tag; }
            set
            {
                if( _tag != value )
                {
                    _tag = value;
                    OnPropertyChanged( "Tag" );
                }
            }
        }

        #endregion
    }
}
