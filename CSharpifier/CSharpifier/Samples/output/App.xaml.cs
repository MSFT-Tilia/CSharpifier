namespace CalculatorApp
{
    namespace ApplicationResourceKeys
    {
    }
    
    
    public class App
    {
        public virtual void OnLaunched ( Windows.ApplicationModel.Activation.LaunchActivatedEventArgs args )
        {
            if ( args -> PrelaunchActivated ) {
            }
            OnAppLaunch ( args ,
        }
        
        
        public virtual void OnActivated ( Windows.ApplicationModel.Activation.IActivatedEventArgs args )
        {
            if ( args -> Kind == ActivationKind :: Protocol ) {
                OnAppLaunch ( args ,
            }
        }
        
        
        internal void RemoveWindow ( WindowFrameService frameService )
        {
            if ( m_mainViewId != frameService -> GetViewId ( ) ) {
            }
        }
        
        
        internal void RemoveSecondaryWindow ( WindowFrameService frameService )
        {
            if ( m_mainViewId != frameService -> GetViewId ( ) ) {
            }
        }
        
        
        private static Windows.UI.Xaml.Controls.Frame  CreateFrame ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private static void SetMinWindowSizeAndActivate ( Windows.UI.Xaml.Controls.Frame rootFrame , Windows.Foundation.Size minWindowSize )
        {
        }
        
        
        private void OnAppLaunch ( Windows.ApplicationModel.Activation.IActivatedEventArgs args , Platform.String argument )
        {
            args -> SplashScreen -> Dismissed += ref new TypedEventHandler < SplashScreen ^ ,
            Object ^ > ( this ,
            & App :: DismissedEventHandler ) ; auto rootFrame = dynamic_cast < Frame ^ > ( Window :: Current -> Content ) ; WeakReference weak ( this ) ; float minWindowWidth = static_cast < float > ( static_cast < double > ( this -> Resources -> Lookup ( ApplicationResourceKeys :: AppMinWindowWidth ) ) ) ; float minWindowHeight = static_cast < float > ( static_cast < double > ( this -> Resources -> Lookup ( ApplicationResourceKeys :: AppMinWindowHeight ) ) ) ; Size minWindowSize = SizeHelper :: FromDimensions ( minWindowWidth ,
            minWindowHeight ) ; ApplicationView ^ appView = ApplicationView :: GetForCurrentView ( ) ; ApplicationDataContainer ^ localSettings = ApplicationData :: Current -> LocalSettings ; if ( ! localSettings -> Values -> HasKey ( L"VeryFirstLaunch" ) ) {
                localSettings -> Values -> Insert ( ref new String ( L"VeryFirstLaunch" ) ,
            }
            else {
            }
            if ( rootFrame == nullptr ) {
                if ( ! Windows :: Foundation :: Metadata :: ApiInformation :: IsTypePresent ( "Windows.Phone.UI.Input.HardwareButtons" ) ) {
                    try {
                    }
                    catch ( Exception ^ e ) {
                    }
                }
                rootFrame = App :: CreateFrame ( ) ; if ( ! rootFrame -> Navigate ( MainPage :: typeid ,
                argument ) ) {
                }
                SetMinWindowSizeAndActivate ( rootFrame ,
                minWindowSize ) ; m_mainViewId = ApplicationView :: GetForCurrentView ( ) -> Id ; AddWindowToMap ( WindowFrameService :: CreateNewWindowFrameService ( rootFrame ,
                false ,
            }
            else {
                if ( ( UIViewSettings :: GetForCurrentView ( ) -> UserInteractionMode == UserInteractionMode :: Mouse ) && ( ! Windows :: Foundation :: Metadata :: ApiInformation :: IsTypePresent ( "Windows.Phone.UI.Input.HardwareButtons" ) ) ) {
                    if ( ! m_preLaunched ) {
                        auto newCoreAppView = CoreApplication :: CreateNewView ( ) ; newCoreAppView -> Dispatcher -> RunAsync ( CoreDispatcherPriority :: Normal ,
                        ref new DispatchedHandler ( [ args ,
                        argument ,
                        minWindowSize ,
                        weak ] ( ) {
                            auto that = weak.Resolve < App > ( ) ; if ( that != nullptr ) {
                                auto rootFrame = App :: CreateFrame ( ) ; SetMinWindowSizeAndActivate ( rootFrame ,
                                minWindowSize ) ; if ( ! rootFrame -> Navigate ( MainPage :: typeid ,
                                argument ) ) {
                                }
                                auto frameService = WindowFrameService :: CreateNewWindowFrameService ( rootFrame ,
                                true ,
                                weak ) ; that -> AddWindowToMap ( frameService ) ; auto dispatcher = CoreWindow :: GetForCurrentThread ( ) -> Dispatcher ; auto safeFrameServiceCreation = std :: make_shared < SafeFrameWindowCreation > ( frameService ,
                                that ) ; int newWindowId = ApplicationView :: GetApplicationViewIdForWindow ( CoreWindow :: GetForCurrentThread ( ) ) ; ActivationViewSwitcher ^ activationViewSwitcher ; auto activateEventArgs = dynamic_cast < IViewSwitcherProvider ^ > ( args ) ; if ( activateEventArgs != nullptr ) {
                                }
                                if ( activationViewSwitcher != nullptr ) {
                                    activationViewSwitcher -> ShowAsStandaloneAsync ( newWindowId ,
                                }
                                else {
                                    auto activatedEventArgs = dynamic_cast < IApplicationViewActivatedEventArgs ^ > ( args ) ; if ( ( activatedEventArgs != nullptr ) && ( activatedEventArgs -> CurrentlyShownApplicationViewId != 0 ) ) {
                                        create_task ( ApplicationViewSwitcher :: TryShowAsStandaloneAsync ( frameService -> GetViewId ( ) ,
                                        ViewSizePreference :: Default ,
                                        activatedEventArgs -> CurrentlyShownApplicationViewId ,
                                        ViewSizePreference :: Default ) ).then ( [ safeFrameServiceCreation ] ( bool viewShown ) {
                                        }
                                        ,
                                    }
                                }
                            }
                        }
                    }
                    else {
                        ActivationViewSwitcher ^ activationViewSwitcher ; auto activateEventArgs = dynamic_cast < IViewSwitcherProvider ^ > ( args ) ; if ( activateEventArgs != nullptr ) {
                        }
                        if ( activationViewSwitcher != nullptr ) {
                            activationViewSwitcher -> ShowAsStandaloneAsync ( ApplicationView :: GetApplicationViewIdForWindow ( CoreWindow :: GetForCurrentThread ( ) ) ,
                        }
                        else {
                            TraceLogger :: GetInstance ( ) -> LogError ( ViewMode :: None ,
                            L"App::OnAppLaunch" ,
                        }
                    }
                }
                else {
                    if ( rootFrame -> Content == nullptr ) {
                        if ( ! rootFrame -> Navigate ( MainPage :: typeid ,
                        argument ) ) {
                        }
                    }
                    if ( ApplicationView :: GetForCurrentView ( ) -> ViewMode != ApplicationViewMode :: CompactOverlay ) {
                        if ( ! Windows :: Foundation :: Metadata :: ApiInformation :: IsTypePresent ( "Windows.Phone.UI.Input.HardwareButtons" ) ) {
                            ActivationViewSwitcher ^ activationViewSwitcher ; auto activateEventArgs = dynamic_cast < IViewSwitcherProvider ^ > ( args ) ; if ( activateEventArgs != nullptr ) {
                            }
                            if ( activationViewSwitcher != nullptr ) {
                                auto viewId = safe_cast < IApplicationViewActivatedEventArgs ^ > ( args ) -> CurrentlyShownApplicationViewId ; if ( viewId != 0 ) {
                                }
                            }
                        }
                    }
                }
            }
        }
        
        
        private void DismissedEventHandler ( Windows.ApplicationModel.Activation.SplashScreen sender , Platform.Object e )
        {
        }
        
        
        private void RegisterDependencyProperties ()
        {
        }
        
        
        private void OnSuspending ( Platform.Object sender , Windows.ApplicationModel.SuspendingEventArgs args )
        {
        }
        
        
        
        public class SafeFrameWindowCreation
        {
            public SafeFrameWindowCreation ( WindowFrameService frameService , App parent )
            {
            }
            
            
            public void SetOperationSuccess ( bool success )
            {
            }
            
            
            public ~ SafeFrameWindowCreation ()
            {
                if ( ! m_frameOpenedInWindow ) {
                }
            }
            
            
            private WindowFrameService  m_frameService;
            private bool m_frameOpenedInWindow;
            private App  m_parent;
        }
        
        private Task SetupJumpList ()
        {
            try {
                auto calculatorOptions = NavCategoryGroup :: CreateCalculatorCategory ( ) ; auto jumpList = co_await JumpList :: LoadCurrentAsync ( ) ; jumpList -> SystemGroupKind = JumpListSystemGroupKind :: None ; jumpList -> Items -> Clear ( ) ; for ( NavCategory ^ option :
                calculatorOptions -> Categories ) {
                    if ( ! option -> IsEnabled ) {
                    }
                    ViewMode mode = option -> Mode ; auto item = JumpListItem :: CreateWithArguments ( ( ( int ) mode ).ToString ( ) ,
                }
            }
            catch ( ... ) {
            }
        }
        
        
        private Task HandleViewReleaseAndRemoveWindowFromMap ( WindowFrameService frameService )
        {
        }
        
        
        private void AddWindowToMap ( WindowFrameService frameService )
        {
        }
        
        
        private WindowFrameService  GetWindowFromMap ( int viewId )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void RemoveWindowFromMap ( int viewId )
        {
        }
        
        
        private int m_mainViewId;
        private bool m_preLaunched;
        private Windows.UI.Xaml.Controls.Primitives.Popup  m_aboutPopup;
    }
    
}

