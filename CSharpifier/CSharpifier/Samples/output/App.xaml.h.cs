namespace CalculatorApp
{
    namespace ApplicationResourceKeys
    {
    }
    
    
    public class App
    {
        public App() { throw new NotImplementedException(); /* CSharpifier Warning */ }
        public virtual void OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs^args)override { throw new NotImplementedException(); /* CSharpifier Warning */ }
        public virtual void OnActivated(Windows::ApplicationModel::Activation::IActivatedEventArgs^args)override { throw new NotImplementedException(); /* CSharpifier Warning */ }
        internal void RemoveWindow(_In_WindowFrameService^frameService) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        internal void RemoveSecondaryWindow(_In_WindowFrameService^frameService) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private static Windows::UI::Xaml::Controls::Frame ^CreateFrame() { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private static void SetMinWindowSizeAndActivate(Windows::UI::Xaml::Controls::Frame^rootFrame,Windows::Foundation::SizeminWindowSize) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private void OnAppLaunch(Windows::ApplicationModel::Activation::IActivatedEventArgs^args,Platform::String^argument) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private void DismissedEventHandler(Windows::ApplicationModel::Activation::SplashScreen^sender,Platform::Object^e) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private void RegisterDependencyProperties() { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private void OnSuspending(Platform::Object^sender,Windows::ApplicationModel::SuspendingEventArgs^args) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        
        public class SafeFrameWindowCreation
        {
            public SafeFrameWindowCreation(_In_WindowFrameService^frameService,App^parent) :
            m_frameService ( frameService ) ,
            m_frameOpenedInWindow ( false ) ,
            m_parent ( parent ) {
            }
            
            public void SetOperationSuccess(boolsuccess) {
            }
            
            public ~SafeFrameWindowCreation() {
                if ( ! m_frameOpenedInWindow ) {
                }
            }
            
        }
        
        private concurrency::task<void> SetupJumpList() { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private concurrency::task<void> HandleViewReleaseAndRemoveWindowFromMap(_In_WindowFrameService^frameService) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private void AddWindowToMap(_In_WindowFrameService^frameService) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private WindowFrameService ^GetWindowFromMap(intviewId) { throw new NotImplementedException(); /* CSharpifier Warning */ }
        private void RemoveWindowFromMap(intviewId) { throw new NotImplementedException(); /* CSharpifier Warning */ }
    }
    
}

