#region

using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Systecs.Framework;
using Systecs.Framework.AssemblyResolver;
using Systecs.Framework.Autofac;
using Systecs.Framework.Autofac.Bootstrapper;
using Systecs.Framework.FrameworkApplication;

#endregion


namespace OMS.Water.Bootstrapper
{
    public class Bootstrapper : BootstrapperApplication
    {
        private static Bootstrapper s_bootStrapperInstance;


        public Bootstrapper()
        {
            Debugger.Launch();
            if ( s_bootStrapperInstance != null )
                throw new Exception( "There must be only one instance of class Bootstrapper." );
            s_bootStrapperInstance = this;
        }


        public static Bootstrapper Instance()
        {
            return s_bootStrapperInstance;
        }


        protected override void Run()
        {
            AssemblyResolver.Init();

            IFrameworkApplication application;
            if(Command.Display == Display.Embedded || Command.Display == Display.None)
                application = FrameworkApplicationBootstrapper.Init("Framework.Program.Embedded", new ConfigurationSearcherStrategy());
            else
                application = FrameworkApplicationBootstrapper.Init( "Framework.Program.UI", new ConfigurationSearcherStrategy() );


            //Engine.CloseSplashScreen();

            if( application != null )
            {
                application.AddExceptionHandler( ExceptionHandler )
                        .Run();
            }

            Engine.Quit( 0 );
        }


        private static void ExceptionHandler( object sender, ExceptionEventArgs args )
        {
            MessageBox.Show( args.Exception.Message, "OMS.Water.Bootstrapper", MessageBoxButton.OK,
                             MessageBoxImage.Error );
        }
    }
}
