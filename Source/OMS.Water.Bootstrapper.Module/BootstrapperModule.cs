#region

using System.Collections.Generic;
using System.Windows;
using Autofac;
using Common.Logging;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework;
using Systecs.Framework.FrameworkApplication;
using Systecs.Framework.Resources;
using Systecs.Framework.WPF.Resources;

#endregion


namespace OMS.Water.Bootstrapper.Module
{
    public class BootstrapperModule : Autofac.Module
    {
        private static readonly ILog s_log = LogManager.GetCurrentClassLogger();


        protected override void Load( ContainerBuilder builder )
        {
            base.Load( builder );

            s_log.Debug( m => m( "Registering module OMS.Water.Bootstrapper ..." ) );


            s_log.Debug( m => m( "  Register FrameworkApplicationInfoProvider." ) );
            builder.Register<IFrameworkApplicationInfoProvider>( c => new FrameworkApplicationInfoProvider
                                                                      {
                                                                              ApplicationName = "OMS.Water",
                                                                              ApplicationDisplayName = "OMS.Water",
                                                                              ApplicationVendorName = "OMS",
                                                                              ApplicationVendorDisplayName = "OMS"
                                                                      } )
                    .Named<IFrameworkApplicationInfoProvider>( "Framework.FrameworkApplicationInfoProvider" )
                    .SingleInstance();


            s_log.Debug( m => m( "  Register FrameworkDirectoryProvider." ) );
            builder.Register<IFrameworkDirectoryProvider>( c => new FrameworkDirectoryProvider(
                                                                        c.ResolveNamed<IFrameworkApplicationInfoProvider>(
                                                                                "Framework.FrameworkApplicationInfoProvider" ) ) )
                    .Named<IFrameworkDirectoryProvider>( "Framework.FrameworkDirectoryProvider" )
                    //.OnActivated( handler => handler.Instance.Init() )
                    .SingleInstance();


            s_log.Debug( m => m( "Register EntryPointCommands" ) );
            builder.Register<IBootstrapperEntryPointCommand>( c => new BootstrapperEntryPointCommand( c.ResolveNamed<Window>( "Bootstrapper.InstallerWindow" ),
                                                                                                      c.Resolve<IBootstrapperViewModel>(), c.Resolve<ILocalizer>()) )
                    .OnActivated( handler => handler.Instance.Application = handler.Context.ResolveNamed<IFrameworkApplication>( "Framework.Program.UI" ) )
                    .Named<IBootstrapperEntryPointCommand>("EntryPointCommand.UI")
                    .SingleInstance();
            
            builder.Register<IBootstrapperEntryPointCommand>(c => new SilentBootstrapperEntryPointCommand(c.Resolve<IBootstrapperViewModel>()))
                    .OnActivated(handler => handler.Instance.Application = handler.Context.ResolveNamed<IFrameworkApplication>("Framework.Program.Embedded"))
                    .Named<IBootstrapperEntryPointCommand>("EntryPointCommand.Embedded")
                    .SingleInstance();



            s_log.Debug( m => m( "  Register shutdown handler." ) );
            builder.Register(
                    c => c.ResolveNamed<IFrameworkApplicationShutdownHandler>( "Framework.DefaultApplicationShutdownHandler" ) )
                    .Named<IFrameworkApplicationShutdownHandler>( "Framework.ApplicationShutdownHandler" )
                    .SingleInstance();


            s_log.Debug( m => m( "  Register UI application." ) );
            builder.Register<IFrameworkApplication>( c => new FrameworkApplication(
                                                                  c.ResolveNamed<IFrameworkApplicationInfoProvider>(
                                                                          "Framework.FrameworkApplicationInfoProvider" ),
                                                                  c.ResolveNamed<IFrameworkDirectoryProvider>( "Framework.FrameworkDirectoryProvider" ),
                                                                  c.ResolveNamed<IBootstrapperEntryPointCommand>("EntryPointCommand.UI")))
                    .Named<IFrameworkApplication>( "Framework.Program.UI" )
                    .SingleInstance();

            s_log.Debug(m => m("  Register embedded application."));
            builder.Register<IFrameworkApplication>(c => new FrameworkApplication(
                                                                  c.ResolveNamed<IFrameworkApplicationInfoProvider>(
                                                                          "Framework.FrameworkApplicationInfoProvider"),
                                                                  c.ResolveNamed<IFrameworkDirectoryProvider>("Framework.FrameworkDirectoryProvider"),
                                                                  c.ResolveNamed<IBootstrapperEntryPointCommand>("EntryPointCommand.Embedded")))
                    .Named<IFrameworkApplication>("Framework.Program.Embedded")
                    .SingleInstance();

            s_log.Debug(m => m("  Register bootstrapper view.model."));
            builder.Register<IBootstrapperViewModel>( c => new BootstrapperViewModel { Localizer = c.Resolve<ILocalizer>() } )
                    .OnActivating( handler => handler.Instance.RegisterPackages( handler.Context.Resolve<IEnumerable<IPackageDescriptor>>() ) )
                    .OnActivating(
                            handler =>
                            handler.Instance.RegisterInstallationViewModels( handler.Context.Resolve<IEnumerable<IInstallationViewModelDescriptor>>() ) )
                    .SingleInstance();
        }
    }
}
