﻿#region

using System;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Sample.ViewModels
{
    public class InstallErrorViewModel : ViewModel, IInstallErrorViewModel
    {
        #region IInstallErrorViewModel Members

        public IBootstrapperViewModel Bootstrapper { get; set; }


        public bool RunEmbedded()
        {
            return true;
        }

        #endregion
    }
}
