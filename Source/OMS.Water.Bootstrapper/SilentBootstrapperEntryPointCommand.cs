#region

using System.Diagnostics;
using System.Windows;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework;

#endregion


namespace OMS.Water.Bootstrapper
{
    public class SilentBootstrapperEntryPointCommand : IBootstrapperEntryPointCommand
    {
        public SilentBootstrapperEntryPointCommand(IBootstrapperViewModel bootstrapperViewModel)
        {
            BootstrapperViewModel = bootstrapperViewModel;
        }


        public IBootstrapperViewModel BootstrapperViewModel { get; private set; }


        #region IBootstrapperEntryPointCommand Members

        public IFrameworkApplication Application { get; set; }


        public void Execute( RunMode mode, ILoadProgress loadProgress )
        {
            //Debugger.Launch();
            //BootstrapperViewModel.Initialize();

            //BootstrapperViewModel.RunEmbedded();

            var app = new App { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            app.InitializeComponent();


            BootstrapperViewModel.Initialize();

            BootstrapperViewModel.RunEmbedded();
            app.Run(null);
            BootstrapperViewModel.RunEmbedded();

        }


        public void Execute( RunMode runMode )
        {
            Execute( runMode, null );
        }

        #endregion
    }
}
