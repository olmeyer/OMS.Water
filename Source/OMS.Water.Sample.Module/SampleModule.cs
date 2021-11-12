#region

using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Autofac;
using Common.Logging;
using OMS.Water.Sample;
using OMS.Water.Sample.ViewModels;
using OMS.Water.Sample.Views;
using Systecs.Framework.Resources;
using Systecs.Framework.WPF.Resources;

#endregion


namespace OMS.Water.Bootstrapper.Module
{
    public class SampleModule : Autofac.Module
    {
        private static readonly ILog s_log = LogManager.GetCurrentClassLogger();


        protected override void Load( ContainerBuilder builder )
        {
            base.Load( builder );

            builder.Register<Window>( c => new InstallerWindow() )
                    .OnActivating(
                            handler =>
                            handler.Instance.DataContext = GetLanguages( handler.Context.ResolveNamed<ILocalizer>( "Framework.Localizer" ) ) )
                    .Named<Window>( "Bootstrapper.InstallerWindow" )
                    .SingleInstance();

            builder.Register<IPackageDescriptor>(c => new PackageDescriptor("TestMsi", "Test MSI", "", "", "", ""))
                    .SingleInstance();

            builder.Register<IPackageDescriptor>(c => new PackageDescriptor("TestReboot", "Test Reboot", "", "", "", ""))
                    .SingleInstance();

            builder.Register<IPackageDescriptor>(c => new PackageDescriptor("TestDialog", "Test Dialog", "", "", "", ""))
                    .SingleInstance();

            builder.Register<IPackageDescriptor>(c => new PackageDescriptor("Netfx4Full", "Microsoft .NET Framework 4", "", "", "", ""))
                    .SingleInstance();

            //builder.Register<IPackageDescriptor>(c => new PackageDescriptor("KiBoxCockpit131", "Kistler KiBoxCockpit V1.3.1"))
            //        .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(c => new InstallationViewModelDescriptor<IHelpViewModel, HelpView>(EInstallationState.Help))
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c => new InstallationViewModelDescriptor<IInitializeViewModel, InitializeView>( EInstallationState.Initializing ) )
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IPrepareInstallViewModel, PrepareInstallView>( EInstallationState.Preparing, EInstallationMode.Install ) )
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IPrepareUninstallViewModel, PrepareUninstallView>( EInstallationState.Preparing,
                                                                                                           EInstallationMode.Uninstall ) )
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IApplyingInstallViewModel, ApplyingInstallView>( EInstallationState.Applying, EInstallationMode.Install ) )
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IApplyingUninstallViewModel, ApplyingUninstallView>( EInstallationState.Applying,
                                                                                                             EInstallationMode.Uninstall ) )
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IInstallCompletedViewModel, InstallCompletedView>( EInstallationState.Completed,
                                                                                                           EInstallationMode.Install ) )
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IUninstallCompletedViewModel, UninstallCompletedView>( EInstallationState.Completed,
                                                                                                               EInstallationMode.Uninstall ) )
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IRebootRequiredViewModel, RebootRequiredView>(EInstallationState.Restart,
                                                                                                      EInstallationMode.Install))
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IInstallCanceledViewModel, InstallCanceledView>( EInstallationState.Canceled ) )
                    .SingleInstance();

            builder.Register<IInstallationViewModelDescriptor>(
                    c =>
                    new InstallationViewModelDescriptor<IInstallErrorViewModel, InstallErrorView>( EInstallationState.Error ) )
                    .SingleInstance();


            builder.Register<IHelpViewModel>( c => new HelpViewModel() )
                    .SingleInstance();

            builder.Register<IInitializeViewModel>( c => new InitializeViewModel() )
                    .SingleInstance();

            builder.Register<IPrepareInstallViewModel>( c => new PrepareInstallViewModel() )
                    .SingleInstance();

            builder.Register<IPrepareUninstallViewModel>( c => new PrepareUninstallViewModel() )
                    .SingleInstance();

            builder.Register<IApplyingInstallViewModel>( c => new ApplyingInstallViewModel() )
                    .SingleInstance();

            builder.Register<IApplyingUninstallViewModel>( c => new ApplyingUninstallViewModel() )
                    .SingleInstance();

            builder.Register<IInstallCompletedViewModel>( c => new InstallCompletedViewModel() )
                    .SingleInstance();

            builder.Register<IUninstallCompletedViewModel>( c => new UninstallCompletedViewModel() )
                    .SingleInstance();

            builder.Register<IInstallCanceledViewModel>( c => new InstallCanceledViewModel() )
                    .SingleInstance();

            builder.Register<IInstallErrorViewModel>( c => new InstallErrorViewModel() )
                    .SingleInstance();

            builder.Register<IRebootRequiredViewModel>(c => new RebootRequiredViewModel())
                    .SingleInstance();


            // Register string resources for localization.
            s_log.Debug( m => m( "  Register ToolManager resources." ) );
            builder.Register<IResourceDescriptor<IResxResourceProvider>>(
                    c => new ResourceDescriptor<IResxResourceProvider>( "OMS.Water.Sample.Resources.StringResources", "OMS.Water.Sample.GUI" ) )
                    .InstancePerDependency();

            builder.Register<IResourceDescriptor<IResxResourceProvider>>(
                    c => new ResourceDescriptor<IResxResourceProvider>( "OMS.Water.Sample.Resources.ImageResources", "OMS.Water.Sample.GUI" ) )
                    .InstancePerDependency();
        }


        private static IEnumerable<ILanguageViewModel> GetLanguages( ILocalizer localizer )
        {
            return new List<ILanguageViewModel>
                   {
                           new LanguageViewModel( localizer, CultureInfo.GetCultureInfo( (CultureInfo.InstalledUICulture.Equals(CultureInfo.GetCultureInfo( "en-US" )))
                                                                                                 ? "en-US"
                                                                                                 : "en" ) ),
                           new LanguageViewModel( localizer, CultureInfo.GetCultureInfo( "de" ) )
                   };
        }
    }
}
