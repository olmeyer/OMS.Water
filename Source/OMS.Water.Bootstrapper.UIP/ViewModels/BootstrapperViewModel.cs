#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Systecs.Framework.Resources;
using Systecs.Framework.WPF.Commands;
using Systecs.Framework.WPF.ViewModels;

#endregion


namespace OMS.Water.Bootstrapper.ViewModels
{
    public class BootstrapperViewModel : ViewModel, IBootstrapperViewModel
    {
        private readonly List<IPackageDescriptor> _packageDescriptors = new List<IPackageDescriptor>();
        private readonly ObservableCollection<IPackage> _packages = new ObservableCollection<IPackage>();
        private IViewModel _activeViewModel;
        private string _currentAction;
        private IPackage _currentPackage;
        private EInstallationMode _installationMode = EInstallationMode.None;
        private EInstallationState _installationState = EInstallationState.None;
        private bool _isCanceling;
        private bool _isCanceled;
        private IEnumerable<IInstallationViewModelDescriptor> _viewModelDescriptors;
        private string _msiMessage;
        private int _progressPercentage;
        private int _overallPercentage;
        private int _executeProgressPercentage;
        private int _executeOverallPercentage;
        private IBundle _relatedBundle;
        private IntPtr _windowHandle;
        private bool _rebootAfterShutdown;


        public BootstrapperViewModel()
        {
            Bootstrapper = Bootstrapper.Instance();

            StringVariables = new Variables<string>( Bootstrapper.Engine.StringVariables );
            VersionVariables = new Variables<Version>( Bootstrapper.Engine.VersionVariables );
            NumericVariables = new Variables<long>( Bootstrapper.Engine.NumericVariables );

            Bootstrapper.DetectPackageComplete += OnDetectPackageComplete;
            Bootstrapper.DetectRelatedBundle += OnDetectRelatedBundle;
            Bootstrapper.DetectRelatedMsiPackage += OnDetectRelatedMsiPackage;
            Bootstrapper.ExecuteProgress += OnExecuteProgress;
            Bootstrapper.ExecutePackageBegin += OnExecutePackageBegin;
            Bootstrapper.ExecutePackageComplete += OnExecutePackageComplete;
            Bootstrapper.ExecuteBegin += OnExecuteBegin;
            Bootstrapper.ExecuteComplete += OnExecuteComplete;
            Bootstrapper.ExecuteMsiMessage += OnExecuteMsiMessage;
            Bootstrapper.PlanComplete += OnPlanComplete;
            Bootstrapper.Error += OnError;
            Bootstrapper.Progress += OnProgress;
            Bootstrapper.RestartRequired += OnRestartRequired;
            Bootstrapper.SystemShutdown += OnSystemShutdown;
            Bootstrapper.Shutdown += OnShutdown;


            Bootstrapper.DetectCompatiblePackage += (sender, args) => { };
            Bootstrapper.DetectForwardCompatibleBundle += (sender, args) => { };
            Bootstrapper.DetectMsiFeature += (sender, args) => { };
            Bootstrapper.DetectTargetMsiPackage += (sender, args) => { };
            Bootstrapper.DetectBegin += (sender, args) => { };
            Bootstrapper.DetectComplete += (sender, args) => { };
            Bootstrapper.DetectPackageBegin += (sender, args) => { };
            Bootstrapper.DetectPriorBundle += (sender, args) => { };
         }

        private void OnShutdown(object sender, ShutdownEventArgs shutdownEventArgs)
        {
            if(_rebootAfterShutdown)
                shutdownEventArgs.Result = Result.Restart;
        }

        private void OnSystemShutdown(object sender, SystemShutdownEventArgs systemShutdownEventArgs)
        {
        }

        private void OnRestartRequired(object sender, RestartRequiredEventArgs restartRequiredEventArgs)
        {
        }


        private Bootstrapper Bootstrapper { get; set; }


        #region IBootstrapperViewModel Members

        public void Initialize()
        {
            ReadLaunchInfo();

            Bootstrapper.Engine.CloseSplashScreen();
            Bootstrapper.Engine.Detect();

//            Debugger.Launch();
            InstallationState = GetNextState();

            if(InstallationState == EInstallationState.Preparing)
                DoApply();
        }


        public void RegisterPackages( IEnumerable<IPackageDescriptor> packageDescriptors )
        {
            _packageDescriptors.AddRange( packageDescriptors );
        }


        public void RegisterInstallationViewModels( IEnumerable<IInstallationViewModelDescriptor> installationViewModelDescriptors )
        {
            _viewModelDescriptors = installationViewModelDescriptors.ToList();

            RegisterDataTemplates(_viewModelDescriptors.Cast<IViewModelDescriptor>());
        }


        public bool RunEmbedded()
        {
            while(InstallationState != EInstallationState.Exit)
            {
                if(ActiveViewModel != null)
                {
                    lock( this )
                    {
                        if( !((IInstallationViewModel)ActiveViewModel).RunEmbedded() )
                        {
                            InstallationState = EInstallationState.Exit;
                            return false;
                        }
                        if (InstallationState == EInstallationState.Applying)
                        {
                            if(!((IInstallationViewModel)ActiveViewModel).RunEmbedded())
                                DoCancel();
                            break;
                        }
                    }
                    InstallationState = GetNextState();
                }
            }
            return true;
        }

        public ObservableCollection<IPackage> Packages
        {
            get { return _packages; }
        }

        public EInstallationState InstallationState
        {
            get { return _installationState; }
            private set
            {
                if( _installationState != value )
                {
                    _installationState = value;

                    //if(IsEmbedded)
                    //{
                    //    SetActiveViewModel();
                    //}
                    //else
                    {
                        if(InstallationState == EInstallationState.Exit)
                        {
                            ActiveViewModel = null;
                        }
                        // Try to set the appropriate view-model for the current state. If no view-model
                        // can be set, goto the next state.
                        else if ( !SetActiveViewModel() )
                        {
                            if(_installationState == EInstallationState.Preparing)
                                DoApply();
                            else
                                InstallationState = GetNextState();
                        }
                        else
                        {
                            NotifyPropertyChanged( "InstallationState" );
                        }
                    }
                }
            }
        }

        public EInstallationMode InstallationMode
        {
            get { return _installationMode; }
            private set
            {
                if( _installationMode != value )
                {
                    _installationMode = value;
                    NotifyPropertyChanged( "InstallationState" );
                }
            }
        }

        public IBundle RelatedBundle
        {
            get { return _relatedBundle; }
            set
            {
                if( _relatedBundle != value )
                {
                    _relatedBundle = value;
                    NotifyPropertyChanged( "RelatedBundle" );
                }
            }
        }

        public ILaunchInfo LaunchInfo { get; private set; }

        public IVariables<string> StringVariables { get; private set; }

        public IVariables<Version> VersionVariables { get; private set; }

        public IVariables<long> NumericVariables { get; private set; }

        public IErrorViewModel Error { get; private set; }

        public IViewModel ActiveViewModel
        {
            get { return _activeViewModel; }
            set
            {
                if( _activeViewModel != value )
                {
                    _activeViewModel = value;
                    NotifyPropertyChanged( "ActiveViewModel" );
                }
            }
        }

        public int ProgressPercentage
        {
            get { return _progressPercentage; }
            private set
            {
                if( _progressPercentage != value )
                {
                    _progressPercentage = value;
                    NotifyPropertyChanged("ProgressPercentage");
                }
            }
        }

        public int OverallPercentage
        {
            get { return _overallPercentage; }
            private set
            {
                if (_overallPercentage != value)
                {
                    _overallPercentage = value;
                    NotifyPropertyChanged("OverallPercentage");
                }
            }
        }

        public int ExecuteProgressPercentage
        {
            get { return _executeProgressPercentage; }
            private set
            {
                if (_executeProgressPercentage != value)
                {
                    _executeProgressPercentage = value;
                    NotifyPropertyChanged("ExecuteProgressPercentage");
                }
            }
        }

        public int ExecuteOverallPercentage
        {
            get { return _executeOverallPercentage; }
            private set
            {
                if (_executeOverallPercentage != value)
                {
                    _executeOverallPercentage = value;
                    NotifyPropertyChanged("ExecuteOverallPercentage");
                }
            }
        }

        public IPackage CurrentPackage
        {
            get { return _currentPackage; }
            private set
            {
                if( _currentPackage != value )
                {
                    _currentPackage = value;
                    NotifyPropertyChanged( "CurrentPackage" );
                }
            }
        }

        public string MsiMessage
        {
            get { return _msiMessage; }
            private set
            {
                _msiMessage = value;
                NotifyPropertyChanged( "MsiMessage" );
            }
        }

        public ICommand Apply
        {
            get { return new DelegateCommand( DoApply, CanApply ); }
        }


        public ICommand Close
        {
            get { return new DelegateCommand( DoClose ); }
        }

        public ICommand Reboot
        {
            get { return new DelegateCommand(DoReboot); }
        }



        public ICommand Cancel
        {
            get { return new DelegateCommand( DoCancel, CanCancel ); }
        }

        public bool IsCanceling
        {
            get { return _isCanceling; }
            private set
            {
                if (_isCanceling != value)
                {
                    _isCanceling = value;
                    NotifyPropertyChanged("IsCanceling");
                }
            }
        }

        public bool IsCanceled
        {
            get { return _isCanceled; }
            private set
            {
                if (_isCanceled != value)
                {
                    _isCanceled = value;
                    if (_isCanceled)
                    {
                        IsExecuting = false;
                        IsCanceling = false;
                    }
                    NotifyPropertyChanged("IsCanceled");
                }
            }
        }



        private bool _isRebootRequired;
        public bool IsRebootRequired
        {
            get { return _isRebootRequired; }
            set
            {
                if (_isRebootRequired != value)
                {
                    _isRebootRequired = value;
                    NotifyPropertyChanged("IsRebootRequired");
                }
            }
        }


        public string CurrentAction
        {
            get { return _currentAction; }
            private set
            {
                if( _currentAction != value )
                {
                    _currentAction = value;
                    NotifyPropertyChanged( "CurrentAction" );
                }
            }
        }


        public ICommand NextInstallationState
        {
            get { return new DelegateCommand<EInstallationState?>( DoSetNextState, CanSetNextState ); }
        }


        public CultureInfo CurrentUICulture
        {
            get { return Localizer.CurrentUICulture; }
        }

        public ILocalizer Localizer { get; set; }

        public ICommand SetUICulture
        {
            get { return new DelegateCommand<string>(p => DoSetUICulture( CultureInfo.GetCultureInfo( p ) ), p => true ); }
        }

        #endregion


        private void OnExecutePackageComplete( object sender, ExecutePackageCompleteEventArgs e )
        {
            lock(this)
            {
                if (IsCanceling)
                {
                    e.Result = Result.Cancel;
                    IsCanceled = true;
                }
                
                if (e.Restart != ApplyRestart.None)
                    IsRebootRequired = true;
            }
        }


        private void DoSetUICulture( CultureInfo culture )
        {
            Localizer.CurrentUICulture = culture;
        }


        private void DoClose()
        {
            Application.Current.MainWindow.Close();
        }

        private void DoReboot()
        {
            _rebootAfterShutdown = true;
            
            DoClose();
        }


        private void DoSetNextState( EInstallationState? newState )
        {
            InstallationState = newState.HasValue ? newState.Value : GetNextState();
        }


        private bool CanSetNextState( EInstallationState? newState )
        {
            var nextState = GetNextState();

            // TODO: More tests.
            return InstallationState != nextState;
        }


        private void OnError( object sender, ErrorEventArgs e )
        {
            lock( this )
            {
                CurrentPackage = null;
                Error = new ErrorViewModel(e.ErrorCode, e.ErrorMessage);

                if (IsCanceling)
                    IsCanceled = true;
                else    
                    InstallationState = EInstallationState.Error;

                e.Result = IsCanceled ? Result.Cancel : Result.Ok;
            }
        }


        private void OnPlanComplete( object sender, PlanCompleteEventArgs e )
        {
            lock (this)
            {
                if( e.Status >= 0 )
                    Bootstrapper.Engine.Apply( _windowHandle );
            }
        }


        private void DoApply()
        {
            if(!IsEmbedded)
                _windowHandle = new WindowInteropHelper( Application.Current.MainWindow ).Handle;

            InstallationState = GetNextState();

            Bootstrapper.Engine.Plan( EnumConverter.Convert( InstallationMode ) );
        }


        private bool CanApply()
        {
            return !IsCanceling && !IsCanceled && (InstallationState == EInstallationState.Preparing || InstallationState == EInstallationState.Applying);
        }


        private void DoCancel()
        {
            // If we are not installing yet, set the bootstrapper into the "Canceled" state. 
            IsCanceling = true;

            if( InstallationState != EInstallationState.Applying )
                InstallationState = EInstallationState.Canceled;
        }


        private bool CanCancel()
        {
            return !IsCanceling && !IsCanceled;
        }


        private bool IsExecuting { get; set; }

        private void OnExecuteMsiMessage( object sender, ExecuteMsiMessageEventArgs e )
        {
            lock( this )
            {
                ParseMessage( e );
            }
        }

        private void OnExecuteBegin( object sender, ExecuteBeginEventArgs e )
        {
            lock(this)
            {
                IsExecuting = true;

                e.Result = IsCanceling ? Result.Cancel : Result.Ok;
            }
        }

        private void OnExecuteComplete( object sender, ExecuteCompleteEventArgs e )
        {
            lock (this)
            {
                if (IsCanceling)
                {
                    IsCanceled = true;
                }

                if( !IsCanceled && e.Status < 0 )
                    InstallationState = EInstallationState.Error;

                // Do not change state on error or if installation has been cancelled.)
                if( InstallationState == EInstallationState.Error )
                    return;

                // Installation has finished. Set the current package to null.
                //CurrentPackage = null;

                InstallationState = IsCanceled ? EInstallationState.Canceled : IsRebootRequired ? EInstallationState.Restart : EInstallationState.Completed;
                if( IsEmbedded && Application.Current != null )
                {
                    Application.Current.Dispatcher.BeginInvoke( new Action( Application.Current.Shutdown) );
                }
            }
        }


        private void OnExecutePackageBegin( object sender, ExecutePackageBeginEventArgs e )
        {
            lock (this)
            {
                // Set the package that is currently being installed.
                CurrentPackage = (from p in Packages where p.PackageId == e.PackageId select p).FirstOrDefault();

                e.Result = IsCanceled ? Result.Cancel : Result.Ok;
            }
        }


        private void OnProgress( object sender, ProgressEventArgs e )
        {
            lock( this )
            {
                if (OverallPercentage == e.OverallPercentage && ProgressPercentage == e.ProgressPercentage)
                    return;

                OverallPercentage = e.OverallPercentage;
                ProgressPercentage = e.ProgressPercentage;

                //if(ProgressPercentage != 100 && IsCanceling && IsExecuting)
                //    e.Result = Result.Cancel;
                //e.Result = IsCanceled && IsExecuting ? Result.Cancel : Result.Ok;
            }
        }


        private void OnExecuteProgress( object sender, ExecuteProgressEventArgs e )
        {
            lock( this )
            {
                if (OverallPercentage == e.OverallPercentage && ProgressPercentage == e.ProgressPercentage)
                    return;
                
                OverallPercentage = e.OverallPercentage;
                ProgressPercentage = e.ProgressPercentage;

                if (ProgressPercentage != 100 && IsCanceling && IsExecuting)
                    e.Result = Result.Cancel;

                //e.Result = IsCanceled && IsExecuting ? Result.Cancel : Result.Ok;
            }
        }


        private void OnDetectPackageComplete( object sender, DetectPackageCompleteEventArgs e )
        {
            lock (this)
            {
                var package = GetPackageById( e.PackageId );

                if( package == null )
                    throw new Exception( string.Format( "Package '{0}' has not been registered to the bootstrapper view-model.", e.PackageId ) );

                switch( e.State )
                {
                    case PackageState.Absent:
                        package.State = EPackageState.DetectedAbsent;
                        break;
                    case PackageState.Present:
                        package.State = EPackageState.DetectedPresent;
                        break;
                    default:
                        package.State = EPackageState.DetectedAbsent;
                        break;
                }

                Packages.Add( package );
            }
        }


        private void OnDetectRelatedBundle( object sender, DetectRelatedBundleEventArgs e )
        {
            lock (this)
            {
                var relatedBundle = new Bundle
                                    {
                                            PerMachine = e.PerMachine,
                                            ProductCode = e.ProductCode,
                                            Tag = e.BundleTag,
                                            Version = e.Version
                                    };

                RelatedBundle = relatedBundle;
            }
        }


        private void OnDetectRelatedMsiPackage( object sender, DetectRelatedMsiPackageEventArgs e )
        {
            lock (this)
            {
                var package = GetPackageById( e.PackageId );

                if( package == null )
                    throw new Exception( string.Format( "Package '{0}' has not been registered to the bootstrapper view-model.", e.PackageId ) );

                var relatedPackage = new Package
                                     {
                                             PackageId = e.PackageId,
                                             PerMachine = e.PerMachine,
                                             ProductCode = e.ProductCode,
                                             Version = e.Version
                                     };

                package.RelatedPackage = relatedPackage;
            }
        }


        private IPackage GetPackageById( string packageId )
        {
            var package = (from p in Packages
                           where p.PackageId == packageId
                           select p).FirstOrDefault();

            if( package == null )
            {
                var descriptor = (from p in _packageDescriptors
                                  where p.PackageId == packageId
                                  select p).First();

                package = new Package
                          {
                                  PackageId = packageId,
                                  PackageName = descriptor.PackageName,
                                  InstallHeader = descriptor.InstallHeader,
                                  InstallDescription = descriptor.InstallDescription,
                                  UninstallHeader = descriptor.UninstallHeader,
                                  UninstallDescription = descriptor.UninstallDescription
                          };
            }

            return package;
        }


        private void ReadLaunchInfo()
        {
            var command = Bootstrapper.Command;

            var launchInfo = new LaunchInfo
                             {
                                     Action = EnumConverter.Convert( command.Action ),
                                     CommandLineArgs = command.GetCommandLineArgs(),
                                     Display = EnumConverter.Convert( command.Display ),
                                     Relation = EnumConverter.Convert( command.Relation ),
                                     Restart = EnumConverter.Convert( command.Restart ),
                                     Resume = EnumConverter.Convert( command.Resume ),
                             };

            LaunchInfo = launchInfo;
        }


        private void ParseMessage( ExecuteMsiMessageEventArgs e )
        {
            // Just for testing
            MsiMessage = e.Message;
            return;

            // http://msdn.microsoft.com/en-us/library/windows/desktop/aa370573(v=vs.85).aspx
            switch( e.MessageType )
            {
                case InstallMessage.ActionStart:
                    MsiMessage = ParseActionStartMessage( e );
                    break;
            }
        }


        private string ParseActionStartMessage( ExecuteMsiMessageEventArgs e )
        {
            var parts = e.Message.Split( new[] { " " }, StringSplitOptions.RemoveEmptyEntries );

            if( parts.Length > 2 )
                CurrentAction = parts[2].Trim( '.' );

            return CurrentAction;
        }


        private bool SetActiveViewModel()
        {
            // Find the appropriate launch view-model type.
            var viewModelType = (from l in _viewModelDescriptors
                                 where
                                         l.InstallationState == InstallationState
                                         && l.InstallationMode == InstallationMode
                                 select l.ViewModelType).FirstOrDefault()
                                ??
                                // Try to get a mode independent view-model.
                                (from l in _viewModelDescriptors
                                 where
                                         l.InstallationState == InstallationState
                                         && l.InstallationMode == EInstallationMode.None
                                 select l.ViewModelType).FirstOrDefault();


            if( viewModelType == null )
                return false;

            // Create an instance of the view-model and set it as the active view-model.
            var installationViewModel = ViewModelFactory.CreateViewModel<IInstallationViewModel>( viewModelType );
            installationViewModel.Bootstrapper = this;
            ActiveViewModel = installationViewModel;

            return true;
        }


        private EInstallationState GetNextState()
        {
            var newState = EInstallationState.None;

            switch( InstallationState )
            {
                case EInstallationState.None:
                    switch (LaunchInfo.Resume)
                    {
                        case EResumeType.Reboot:
                            newState = EInstallationState.Preparing;
                            break;
                        default:
                            newState = EInstallationState.Initializing;
                            break;
                    }
                    
                    switch( LaunchInfo.Action )
                    {
                        case ELaunchAction.Install:
                            // TODO: For development we set mode to uninstall if ARP is set. This must be removed.
                            // If lunch action is Install and resume is set to ARP, the user must choose what to do (install, repair, modify).
                            InstallationMode = (LaunchInfo.Resume != EResumeType.Arp) ? EInstallationMode.Install : EInstallationMode.Uninstall;
                            break;
                        case ELaunchAction.Uninstall:
                            InstallationMode = EInstallationMode.Uninstall;
                            break;
                        case ELaunchAction.Modify:
                            InstallationMode = EInstallationMode.Modify;
                            break;
                        case ELaunchAction.Repair:
                            InstallationMode = EInstallationMode.Repair;
                            break;
                    }
                    break;
                case EInstallationState.Help:
                    newState = EInstallationState.Help; // No next state.
                    break;
                case EInstallationState.Initializing:
                    newState = EInstallationState.Licensing;
                    break;
                case EInstallationState.Licensing:
                {
                    //switch( LaunchInfo.Action )
                    //{
                    //    case ELaunchAction.Install:
                    //        // TODO: For development we set mode to uninstall if ARP is set. This must be removed.
                    //        // If lunch action is Install and resume is set to ARP, the user must choose what to do (install, repair, modify).
                    //        InstallationMode = (LaunchInfo.Resume != EResumeType.Arp) ? EInstallationMode.Install : EInstallationMode.Uninstall;
                    //        break;
                    //    case ELaunchAction.Uninstall:
                    //        InstallationMode = EInstallationMode.Uninstall;
                    //        break;
                    //    case ELaunchAction.Modify:
                    //        InstallationMode = EInstallationMode.Modify;
                    //        break;
                    //    case ELaunchAction.Repair:
                    //        InstallationMode = EInstallationMode.Repair;
                    //        break;
                    //}
                    newState = (InstallationMode == EInstallationMode.None) ? EInstallationState.Selecting : EInstallationState.Preparing;
                    break;
                }
                case EInstallationState.Selecting:
                    newState = EInstallationState.Preparing;
                    break;
                case EInstallationState.Preparing:
                    newState = EInstallationState.Applying;
                    break;
                case EInstallationState.Applying:
                    newState = EInstallationState.Completed;
                    break;
                case EInstallationState.Completed:
                    newState = EInstallationState.Exit; // No next state.
                    break;
                case EInstallationState.Canceled:
                    newState = EInstallationState.Exit; // No next state.
                    break;
                case EInstallationState.Error:
                    newState = EInstallationState.Exit; // No next state.
                    break;
                case EInstallationState.Restart:
                    newState = EInstallationState.Exit; // No next state.
                    break;
            }

            return newState;
        }

        private bool IsEmbedded
        {
            get { return LaunchInfo.Display == EDisplay.None || LaunchInfo.Display == EDisplay.Embedded; }
        }
    }
}
