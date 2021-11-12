#region

using System.Windows;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using OMS.Water.Bootstrapper.ViewModels;
using Systecs.Framework;
using Systecs.Framework.Resources;

#endregion


namespace OMS.Water.Bootstrapper
{
    public class BootstrapperEntryPointCommand : IBootstrapperEntryPointCommand
    {
        public BootstrapperEntryPointCommand( Window window, IBootstrapperViewModel bootstrapperViewModel, ILocalizer localizer )
        {
            Window = window;
            BootstrapperViewModel = bootstrapperViewModel;
            Localizer = localizer;
        }


        private Window Window { get; set; }

        public IBootstrapperViewModel BootstrapperViewModel { get; private set; }
        private ILocalizer Localizer { get; set; }

        #region IBootstrapperEntryPointCommand Members

        public IFrameworkApplication Application { get; set; }


        public void Execute( RunMode mode, ILoadProgress loadProgress )
        {
            var app = new App();
            app.InitializeComponent();

            Window.Closing += ( o, args ) => args.Cancel = !Application.CanClose();

            app.Startup += (sender, args) =>
            {
                Window.DataContext = BootstrapperViewModel;
                BootstrapperViewModel.Initialize();
            };

            app.Run(Window);
        }


        public void Execute( RunMode runMode )
        {
            Execute( runMode, null );
        }

        #endregion
    }
}
