#region

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

#endregion


namespace OMS.Water.Bootstrapper
{
    internal static class EnumConverter
    {
        public static ELaunchAction Convert( LaunchAction action )
        {
            switch( action )
            {
                case LaunchAction.Help:
                    return ELaunchAction.Help;
                case LaunchAction.Install:
                    return ELaunchAction.Install;
                case LaunchAction.Layout:
                    return ELaunchAction.Layout;
                case LaunchAction.Modify:
                    return ELaunchAction.Modify;
                case LaunchAction.Repair:
                    return ELaunchAction.Repair;
                case LaunchAction.Uninstall:
                    return ELaunchAction.Uninstall;
                case LaunchAction.Unknown:
                    return ELaunchAction.Unknown;
                default:
                    return ELaunchAction.Unknown;
            }
        }


        public static LaunchAction Convert( EInstallationMode mode )
        {
            switch( mode )
            {
                case EInstallationMode.Install:
                    return LaunchAction.Install;
                case EInstallationMode.Modify:
                    return LaunchAction.Modify;
                case EInstallationMode.Repair:
                    return LaunchAction.Repair;
                case EInstallationMode.Uninstall:
                    return LaunchAction.Uninstall;
                case EInstallationMode.None:
                    return LaunchAction.Unknown;
                default:
                    return LaunchAction.Unknown;
            }
        }


        public static EDisplay Convert( Display display )
        {
            switch( display )
            {
                case Display.Embedded:
                    return EDisplay.Embedded;
                case Display.Full:
                    return EDisplay.Full;
                case Display.None:
                    return EDisplay.None;
                case Display.Passive:
                    return EDisplay.Passive;
                case Display.Unknown:
                    return EDisplay.Unknown;
                default:
                    return EDisplay.Unknown;
            }
        }


        public static ERelationType Convert( RelationType relation )
        {
            switch( relation )
            {
                case RelationType.Addon:
                    return ERelationType.Addon;
                case RelationType.Detect:
                    return ERelationType.Detect;
                case RelationType.None:
                    return ERelationType.None;
                case RelationType.Patch:
                    return ERelationType.Patch;
                case RelationType.Upgrade:
                    return ERelationType.Upgrade;
                default:
                    return ERelationType.None;
            }
        }


        public static ERestartType Convert( Restart restart )
        {
            switch( restart )
            {
                case Restart.Always:
                    return ERestartType.Always;
                case Restart.Automatic:
                    return ERestartType.Automatic;
                case Restart.Never:
                    return ERestartType.Never;
                case Restart.Prompt:
                    return ERestartType.Prompt;
                case Restart.Unknown:
                    return ERestartType.Unknown;
                default:
                    return ERestartType.Unknown;
            }
        }


        public static EResumeType Convert( ResumeType resume )
        {
            switch( resume )
            {
                case ResumeType.Arp:
                    return EResumeType.Arp;
                case ResumeType.Interrupted:
                    return EResumeType.Interrupted;
                case ResumeType.Invalid:
                    return EResumeType.Invalid;
                case ResumeType.None:
                    return EResumeType.None;
                case ResumeType.Reboot:
                    return EResumeType.Reboot;
                case ResumeType.RebootPending:
                    return EResumeType.RebootPending;
                case ResumeType.Suspend:
                    return EResumeType.Suspend;
                default:
                    return EResumeType.None;
            }
        }
    }
}
