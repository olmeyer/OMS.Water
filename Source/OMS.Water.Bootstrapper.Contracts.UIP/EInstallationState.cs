namespace OMS.Water.Bootstrapper
{
    public enum EInstallationState
    {
        /// <summary>
        ///   Initial bootstrapper state.
        /// </summary>
        None,


        /// <summary>
        ///   The bootstrapper is in the "Help" state.
        ///   In UI mode a help view can be display.
        ///   In non UI mode, the bootstrapper should print help information to the console.
        ///   The bootstrapper connot be switched into another state and must be closed after the user has read the help information.
        /// </summary>
        Help,

        /// <summary>
        ///   The bootstrapper is initializing. 
        ///   In UI mode, a initial view can be displayed. The view should execute the "NextStep" command of the BootStrappper viewmodel
        ///   to set the bootstrapper into the next state.
        /// </summary>
        Initializing,

        /// <summary>
        ///   The bootstrapper is checking license.
        ///   In UI mode, a license view can be displayed. The view should execute the "NextStep" command of the BootStrappper view-model
        ///   to set the bootstrapper into the next state.
        ///   In non UI mode, this state will be skipped and the HasLicenseAccepted property of the Bootstrapper view-model will alway be set to true.
        /// </summary>
        Licensing,

        /// <summary>
        ///   The bootstrapper checks whether to uninstall, repair, or modify the already installed application.
        ///   In UI mode, a selection view should be display to allow the user to choose the desired action.
        ///   In non UI mode, this state will be skipped and the alreay installed application will be uninstalled.
        /// </summary>
        Selecting,

        /// <summary>
        /// </summary>
        Preparing,
        Applying,
        Canceled,
        Error,
        Completed,

        Exit,

        Restart
    }
}
