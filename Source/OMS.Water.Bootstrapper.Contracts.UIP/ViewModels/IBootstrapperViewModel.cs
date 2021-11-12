#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Systecs.Framework.Resources;
using Systecs.Framework.WPF.Resources;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Bootstrapper.ViewModels
{
    public interface IBootstrapperViewModel : IViewModel
    {
        ObservableCollection<IPackage> Packages { get; }

        EInstallationState InstallationState { get; }

        EInstallationMode InstallationMode { get; }

        IBundle RelatedBundle { get; }

        ILaunchInfo LaunchInfo { get; }

        IVariables<string> StringVariables { get; }

        IVariables<Version> VersionVariables { get; }

        IVariables<long> NumericVariables { get; }

        IErrorViewModel Error { get; }

        /// <summary>
        ///   The active view-model. This will be bound to the content of the application window.
        ///   The content template selector will then select the appropriate view. Initially, the bootstrapper assigns, depending on the lauch action,
        ///   one of the registerd launch view-models. In the further installation process, the user is responsible to set the appropriate view-models.
        /// </summary>
        IViewModel ActiveViewModel { get; set; }


        /// <summary>
        ///   Gets percentage between 0 and 100 of the execution progress for the current package.
        /// </summary>
        int ProgressPercentage { get; }

        /// <summary>
        ///   Gets percentage between 0 and 100 of the execution progress for all packages.
        /// </summary>
        int OverallPercentage { get; }


        /// <summary>
        ///   Get the package thai ius currently beeing installed.
        /// </summary>
        IPackage CurrentPackage { get; }


        /// <summary>
        ///   Gets the message from the msi package that is currently being installed.
        ///   The Message property does not contain any messages from exe packages. (Bug in Burn?)
        /// </summary>
        string MsiMessage { get; }


        /// <summary>
        ///   Gets a command to launch the installer.
        /// </summary>
        ICommand Apply { get; }


        /// <summary>
        ///   Gets a command to close the bootstrapper in UI mode.
        /// </summary>
        ICommand Close { get; }


        /// <summary>
        ///   Gets a command to cancel the installation.
        /// </summary>
        ICommand Cancel { get; }

        /// <summary>
        /// Gets a command to reboot the system.
        /// </summary>
        ICommand Reboot { get; }
        
        /// <summary>
        ///   Returns true if the installation is in the cancelation process.
        /// </summary>
        bool IsCanceling { get; }

        /// <summary>
        ///   Returns true if the installation has been canceled.
        /// </summary>
        bool IsCanceled { get; }


        /// <summary>
        /// Returns true if a reboot is required after installation.
        /// </summary>
        bool IsRebootRequired { get; }

        /// <summary>
        ///   Gets the currently executed installer action. The action name can be used as a resource key to display 
        ///   a localized message.
        /// </summary>
        string CurrentAction { get; }


        ICommand NextInstallationState { get; }


        CultureInfo CurrentUICulture { get; }


        ILocalizer Localizer { get; set; }


        ICommand SetUICulture { get; }


        /// <summary>
        ///   Initializes the view model.
        /// </summary>
        void Initialize();


        /// <summary>
        ///   Call this method to register installation packages.
        /// </summary>
        /// <param name = "packageDescriptors"></param>
        void RegisterPackages( IEnumerable<IPackageDescriptor> packageDescriptors );


        /// <summary>
        ///   Call this method to register the installation view-models.
        /// </summary>
        /// <param name = "installationViewModelDescriptors"></param>
        void RegisterInstallationViewModels( IEnumerable<IInstallationViewModelDescriptor> installationViewModelDescriptors );


        // This method is called when the bootstrapper runs in silent or embedded mode.
        bool RunEmbedded();

    }
}
