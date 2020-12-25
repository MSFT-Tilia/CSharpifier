namespace CalculatorApp
{
    namespace ApplicationResourceKeys
    {
    }
    
    
    public class App
    {
        public App() { throw new NotImplementedException(); }
        public virtual void OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs^args)override { throw new NotImplementedException(); }
        public virtual void OnActivated(Windows::ApplicationModel::Activation::IActivatedEventArgs^args)override { throw new NotImplementedException(); }
        internal void RemoveWindow(_In_WindowFrameService^frameService) { throw new NotImplementedException(); }
        internal void RemoveSecondaryWindow(_In_WindowFrameService^frameService) { throw new NotImplementedException(); }
        private static Windows::UI::Xaml::Controls::Frame ^CreateFrame() { throw new NotImplementedException(); }
        private static void SetMinWindowSizeAndActivate(Windows::UI::Xaml::Controls::Frame^rootFrame,Windows::Foundation::SizeminWindowSize) { throw new NotImplementedException(); }
        private void OnAppLaunch(Windows::ApplicationModel::Activation::IActivatedEventArgs^args,Platform::String^argument) { throw new NotImplementedException(); }
        private void DismissedEventHandler(Windows::ApplicationModel::Activation::SplashScreen^sender,Platform::Object^e) { throw new NotImplementedException(); }
        private void RegisterDependencyProperties() { throw new NotImplementedException(); }
        private void OnSuspending(Platform::Object^sender,Windows::ApplicationModel::SuspendingEventArgs^args) { throw new NotImplementedException(); }
        
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
        
        private concurrency::task<void> SetupJumpList() { throw new NotImplementedException(); }
        private concurrency::task<void> HandleViewReleaseAndRemoveWindowFromMap(_In_WindowFrameService^frameService) { throw new NotImplementedException(); }
        private void AddWindowToMap(_In_WindowFrameService^frameService) { throw new NotImplementedException(); }
        private WindowFrameService ^GetWindowFromMap(intviewId) { throw new NotImplementedException(); }
        private void RemoveWindowFromMap(intviewId) { throw new NotImplementedException(); }
    }
    
}

