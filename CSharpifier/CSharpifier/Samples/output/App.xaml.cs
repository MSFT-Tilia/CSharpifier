namespace CalculatorApp
{
    namespace ApplicationResourceKeys
    {
    }
    
    
    public class App
    {
        public virtual void OnLaunched( Windows.ApplicationModel.Activation.LaunchActivatedEventArgs args)
        {
            if( args.PrelaunchActivated) {
                m_preLaunched = true;
                
            }
            OnAppLaunch( args, args.Arguments);
            
        }
        
        
        public virtual void OnActivated( Windows.ApplicationModel.Activation.IActivatedEventArgs args)
        {
            if( args.Kind == ActivationKind.Protocol) {
                OnAppLaunch( args, nullptr);
                
            }
            
        }
        
        
        internal void RemoveWindow( WindowFrameService frameService)
        {
            if( m_mainViewId != frameService.GetViewId()) {
                HandleViewReleaseAndRemoveWindowFromMap( frameService);
                
            }
            
        }
        
        
        internal void RemoveSecondaryWindow( WindowFrameService frameService)
        {
            if( m_mainViewId != frameService.GetViewId()) {
                RemoveWindowFromMap( frameService.GetViewId());
                
            }
            
        }
        
        
        private static Windows.UI.Xaml.Controls.Frame  CreateFrame ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private static void SetMinWindowSizeAndActivate( Windows.UI.Xaml.Controls.Frame rootFrame , Windows.Foundation.Size minWindowSize)
        {
            ApplicationView appView = ApplicationView.GetForCurrentView();
            appView.SetPreferredMinSize( minWindowSize);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            
        }
        
        
        private void OnAppLaunch( Windows.ApplicationModel.Activation.IActivatedEventArgs args , Platform.String argument)
        {
            args.SplashScreen.Dismissed += ref new TypedEventHandler < SplashScreen, Object >( this, & App.DismissedEventHandler);
            var rootFrame = dynamic_cast < Frame >( Window.Current.Content);
            WeakReference weak( this);
            float minWindowWidth = static_cast < float >( static_cast < double >( this.Resources.Lookup( ApplicationResourceKeys.AppMinWindowWidth)));
            float minWindowHeight = static_cast < float >( static_cast < double >( this.Resources.Lookup( ApplicationResourceKeys.AppMinWindowHeight)));
            Size minWindowSize = SizeHelper.FromDimensions( minWindowWidth, minWindowHeight);
            ApplicationView appView = ApplicationView.GetForCurrentView();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if( ! localSettings.Values.HasKey( L"VeryFirstLaunch")) {
                localSettings.Values.Insert( ref new String( L"VeryFirstLaunch"), false);
                appView.SetPreferredMinSize( minWindowSize);
                appView.TryResizeView( minWindowSize);
                
            }
            else {
                appView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
                
            }
            if( rootFrame == nullptr) {
                if( ! Windows.Foundation.Metadata.ApiInformation.IsTypePresent( "Windows.Phone.UI.Input.HardwareButtons")) {
                    try {
                        ApplicationViewSwitcher.DisableSystemViewActivationPolicy();
                        
                    }
                    catch( Exception e) {
                        
                    }
                    
                }
                rootFrame = App.CreateFrame();
                if( ! rootFrame.Navigate( MainPage.typeid, argument)) {
                    throw std.bad_exception();
                    
                }
                SetMinWindowSizeAndActivate( rootFrame, minWindowSize);
                m_mainViewId = ApplicationView.GetForCurrentView().Id;
                AddWindowToMap( WindowFrameService.CreateNewWindowFrameService( rootFrame, false, weak));
                
            }
            else {
                if(( UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Mouse) &&( ! Windows.Foundation.Metadata.ApiInformation.IsTypePresent( "Windows.Phone.UI.Input.HardwareButtons"))) {
                    if( ! m_preLaunched) {
                        var newCoreAppView = CoreApplication.CreateNewView();
                        newCoreAppView.Dispatcher.RunAsync( CoreDispatcherPriority.Normal, ref new DispatchedHandler( [ args, argument, minWindowSize, weak ]() {
                            var that = weak.Resolve < App >();
                            if( that != nullptr) {
                                var rootFrame = App.CreateFrame();
                                SetMinWindowSizeAndActivate( rootFrame, minWindowSize);
                                if( ! rootFrame.Navigate( MainPage.typeid, argument)) {
                                    throw std.bad_exception();
                                    
                                }
                                var frameService = WindowFrameService.CreateNewWindowFrameService( rootFrame, true, weak);
                                that.AddWindowToMap( frameService);
                                var dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
                                var safeFrameServiceCreation = std.make_shared < SafeFrameWindowCreation >( frameService, that);
                                int newWindowId = ApplicationView.GetApplicationViewIdForWindow( CoreWindow.GetForCurrentThread());
                                ActivationViewSwitcher activationViewSwitcher;
                                var activateEventArgs = dynamic_cast < IViewSwitcherProvider >( args);
                                if( activateEventArgs != nullptr) {
                                    activationViewSwitcher = activateEventArgs.ViewSwitcher;
                                    
                                }
                                if( activationViewSwitcher != nullptr) {
                                    activationViewSwitcher.ShowAsStandaloneAsync( newWindowId, ViewSizePreference.Default);
                                    safeFrameServiceCreation.SetOperationSuccess( true);
                                    
                                }
                                else {
                                    var activatedEventArgs = dynamic_cast < IApplicationViewActivatedEventArgs >( args);
                                    if(( activatedEventArgs != nullptr) &&( activatedEventArgs.CurrentlyShownApplicationViewId != 0)) {
                                        create_task( ApplicationViewSwitcher.TryShowAsStandaloneAsync( frameService.GetViewId(), ViewSizePreference.Default, activatedEventArgs.CurrentlyShownApplicationViewId, ViewSizePreference.Default)).then( [ safeFrameServiceCreation ]( bool viewShown) {
                                            safeFrameServiceCreation.SetOperationSuccess( viewShown);
                                            
                                        }
                                        , task_continuation_context.use_current());
                                        
                                    }
                                    
                                }
                                
                            }
                            
                        }
                        ));
                        
                    }
                    else {
                        ActivationViewSwitcher activationViewSwitcher;
                        var activateEventArgs = dynamic_cast < IViewSwitcherProvider >( args);
                        if( activateEventArgs != nullptr) {
                            activationViewSwitcher = activateEventArgs.ViewSwitcher;
                            
                        }
                        if( activationViewSwitcher != nullptr) {
                            activationViewSwitcher.ShowAsStandaloneAsync( ApplicationView.GetApplicationViewIdForWindow( CoreWindow.GetForCurrentThread()), ViewSizePreference.Default);
                            
                        }
                        else {
                            TraceLogger.GetInstance().LogError( ViewMode.None, L"App.OnAppLaunch", L"Null_ActivationViewSwitcher");
                            
                        }
                        
                    }
                    m_preLaunched = false;
                    
                }
                else {
                    if( rootFrame.Content == nullptr) {
                        if( ! rootFrame.Navigate( MainPage.typeid, argument)) {
                            throw std.bad_exception();
                            
                        }
                        
                    }
                    if( ApplicationView.GetForCurrentView().ViewMode != ApplicationViewMode.CompactOverlay) {
                        if( ! Windows.Foundation.Metadata.ApiInformation.IsTypePresent( "Windows.Phone.UI.Input.HardwareButtons")) {
                            ActivationViewSwitcher activationViewSwitcher;
                            var activateEventArgs = dynamic_cast < IViewSwitcherProvider >( args);
                            if( activateEventArgs != nullptr) {
                                activationViewSwitcher = activateEventArgs.ViewSwitcher;
                                
                            }
                            if( activationViewSwitcher != nullptr) {
                                var viewId = safe_cast < IApplicationViewActivatedEventArgs >( args).CurrentlyShownApplicationViewId;
                                if( viewId != 0) {
                                    activationViewSwitcher.ShowAsStandaloneAsync( viewId);
                                    
                                }
                                
                            }
                            
                        }
                        Window.Current.Activate();
                        
                    }
                    
                }
                
            }
            
        }
        
        
        private void DismissedEventHandler( Windows.ApplicationModel.Activation.SplashScreen sender , Platform.Object e)
        {
            SetupJumpList();
            
        }
        
        
        private void RegisterDependencyProperties ()
        {
            NarratorNotifier.RegisterDependencyProperties();
            
        }
        
        
        private void OnSuspending( Platform.Object sender , Windows.ApplicationModel.SuspendingEventArgs args)
        {
            TraceLogger.GetInstance().LogButtonUsage();
            
        }
        
        
        
        public class SafeFrameWindowCreation
        {
            public SafeFrameWindowCreation( WindowFrameService frameService , App parent)
            {
                
            }
            
            
            public void SetOperationSuccess( bool success)
            {
                m_frameOpenedInWindow = success;
                
            }
            
            
            public ~ SafeFrameWindowCreation ()
            {
                if( ! m_frameOpenedInWindow) {
                    m_parent.RemoveWindowFromMap( m_frameService.GetViewId());
                    
                }
                
            }
            
            
            private WindowFrameService  m_frameService;
            private bool m_frameOpenedInWindow;
            private App  m_parent;
        }
        
        private Task SetupJumpList ()
        {
            try {
                var calculatorOptions = NavCategoryGroup.CreateCalculatorCategory();
                var jumpList = co_await JumpList.LoadCurrentAsync();
                jumpList.SystemGroupKind = JumpListSystemGroupKind.None;
                jumpList.Items.Clear();
                for( NavCategory option :
                calculatorOptions.Categories) {
                    if( ! option.IsEnabled) {
                        continue;
                        
                    }
                    ViewMode mode = option.Mode;
                    var item = JumpListItem.CreateWithArguments((( int) mode).ToString(), L"ms-resource:///Resources/" + NavCategory.GetNameResourceKey( mode));
                    item.Description = L"ms-resource:///Resources/" + NavCategory.GetNameResourceKey( mode);
                    item.Logo = ref new Uri( "ms-appx:///Assets/" + mode.ToString() + ".png");
                    jumpList.Items.Append( item);
                    
                }
                co_await jumpList.SaveAsync();
                
            }
            catch( ...) {
                
            }
            
        }
        
        
        private Task HandleViewReleaseAndRemoveWindowFromMap( WindowFrameService frameService)
        {
            reader_writer_lock.scoped_lock lock( m_windowsMapLock);
            var iter = m_secondaryWindows.find( viewId);
            assert( iter != m_secondaryWindows.end() && "Window does not exist in the list");
            m_secondaryWindows.erase( viewId);
            
        }
        
        
        private void AddWindowToMap( WindowFrameService frameService)
        {
            reader_writer_lock.scoped_lock lock( m_windowsMapLock);
            m_secondaryWindows [ frameService.GetViewId() ] = frameService;
            TraceLogger.GetInstance().UpdateWindowCount( m_secondaryWindows.size());
            
        }
        
        
        private WindowFrameService  GetWindowFromMap( int viewId)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void RemoveWindowFromMap( int viewId)
        {
            reader_writer_lock.scoped_lock lock( m_windowsMapLock);
            var iter = m_secondaryWindows.find( viewId);
            assert( iter != m_secondaryWindows.end() && "Window does not exist in the list");
            m_secondaryWindows.erase( viewId);
            
        }
        
        
        private int m_mainViewId;
        private bool m_preLaunched;
        private Windows.UI.Xaml.Controls.Primitives.Popup  m_aboutPopup;
    }
    
}

