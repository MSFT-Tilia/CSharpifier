namespace CalculatorApp
{
    namespace ApplicationResourceKeys
    {
    }
    
    
    public class App
    {
        public virtual void OnLaunched ( Windows.ApplicationModel.Activation.LaunchActivatedEventArgs args )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        public virtual void OnActivated ( Windows.ApplicationModel.Activation.IActivatedEventArgs args )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        internal void RemoveWindow ( WindowFrameService frameService )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        internal void RemoveSecondaryWindow ( WindowFrameService frameService )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private static Windows.UI.Xaml.Controls.Frame  CreateFrame ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private static void SetMinWindowSizeAndActivate ( Windows.UI.Xaml.Controls.Frame rootFrame , Windows.Foundation.Size minWindowSize )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnAppLaunch ( Windows.ApplicationModel.Activation.IActivatedEventArgs args , Platform.String argument )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void DismissedEventHandler ( Windows.ApplicationModel.Activation.SplashScreen sender , Platform.Object e )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void RegisterDependencyProperties ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnSuspending ( Platform.Object sender , Windows.ApplicationModel.SuspendingEventArgs args )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        
        public class SafeFrameWindowCreation
        {
            public SafeFrameWindowCreation ( WindowFrameService frameService , App parent )
            { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
            
            public void SetOperationSuccess ( bool success )
            { m_frameOpenedInWindow = success ; }
            
            public ~ SafeFrameWindowCreation ()
            { if ( ! m_frameOpenedInWindow ) m_parent -> RemoveWindowFromMap ( m_frameService -> GetViewId ( ) ) ; }
            
            private WindowFrameService  m_frameService;
            private bool m_frameOpenedInWindow;
            private App  m_parent;
        }
        
        private Task SetupJumpList ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private Task HandleViewReleaseAndRemoveWindowFromMap ( WindowFrameService frameService )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void AddWindowToMap ( WindowFrameService frameService )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private WindowFrameService  GetWindowFromMap ( int viewId )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void RemoveWindowFromMap ( int viewId )
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private int m_mainViewId;
        private bool m_preLaunched;
        private Windows.UI.Xaml.Controls.Primitives.Popup  m_aboutPopup;
    }
    
}

