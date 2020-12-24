namespace CalculatorApp
{
    namespace ApplicationResourceKeys
    {
    }
    
    
    public class App
    {
        public App();
        public virtual void OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs^args)override;
        public virtual void OnActivated(Windows::ApplicationModel::Activation::IActivatedEventArgs^args)override;
        internal void RemoveWindow(_In_WindowFrameService^frameService);
        internal void RemoveSecondaryWindow(_In_WindowFrameService^frameService);
        private static Windows::UI::Xaml::Controls::Frame ^CreateFrame();
        private static void SetMinWindowSizeAndActivate(Windows::UI::Xaml::Controls::Frame^rootFrame,Windows::Foundation::SizeminWindowSize);
        private void OnAppLaunch(Windows::ApplicationModel::Activation::IActivatedEventArgs^args,Platform::String^argument);
        private void DismissedEventHandler(Windows::ApplicationModel::Activation::SplashScreen^sender,Platform::Object^e);
        private void RegisterDependencyProperties();
        private void OnSuspending(Platform::Object^sender,Windows::ApplicationModel::SuspendingEventArgs^args);
        
        public class SafeFrameWindowCreation
        {
            public SafeFrameWindowCreation(_In_WindowFrameService^frameService,App^parent)
            :m_frameService(frameService),m_frameOpenedInWindow(false),m_parent(parent){}
            public void SetOperationSuccess(boolsuccess)
            {m_frameOpenedInWindow=success;}
            public ~SafeFrameWindowCreation()
            {if(!m_frameOpenedInWindow){m_parent->RemoveWindowFromMap(m_frameService->GetViewId());}}
        }
        
        private concurrency::task<void> SetupJumpList();
        private concurrency::task<void> HandleViewReleaseAndRemoveWindowFromMap(_In_WindowFrameService^frameService);
        private void AddWindowToMap(_In_WindowFrameService^frameService);
        private WindowFrameService ^GetWindowFromMap(intviewId);
        private void RemoveWindowFromMap(intviewId);
    }
    
}

