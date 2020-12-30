namespace CalculatorApp
{
    
    public class FontTable
    {
        private double fullFont;
        private double fullFontMin;
        private double portraitMin;
        private double snapFont;
        private double fullNumPadFont;
        private double snapScientificNumPadFont;
        private double portraitScientificNumPadFont;
    }
    
    
    public class Calculator
    {
        public event FullscreenFlyoutClosedEventHandler  FullscreenFlyoutClosed;
        public void AnimateCalculator( bool resultAnimate)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        public void InitializeHistoryView( CalculatorApp.ViewModel.HistoryViewModel historyVM)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        public void UpdatePanelViewState ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        public void UnregisterEventHandlers ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        public void CloseHistoryFlyout ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        public void CloseMemoryFlyout ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        public void SetDefaultFocus ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        public static Windows.UI.Xaml.Visibility ShouldDisplayHistoryButton( bool isAlwaysOnTop , bool isProgrammer , Windows.UI.Xaml.Visibility dockPanelVisibility)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnLoaded( Platform.Object sender , Windows.UI.Xaml.RoutedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void LoadResourceStrings ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void UpdateViewState ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void UpdateMemoryState ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void UpdateHistoryState ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnContextRequested( Windows.UI.Xaml.UIElement sender , Windows.UI.Xaml.Input.ContextRequestedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnContextCanceled( Windows.UI.Xaml.UIElement sender , Windows.UI.Xaml.RoutedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnIsScientificPropertyChanged( bool oldValue , bool newValue)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnIsProgrammerPropertyChanged( bool oldValue , bool newValue)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnIsStandardPropertyChanged( bool oldValue , bool newValue)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnIsAlwaysOnTopPropertyChanged( bool oldValue , bool newValue)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnIsInErrorPropertyChanged ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnCalcPropertyChanged( Platform.Object sender , Windows.UI.Xaml.Data.PropertyChangedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnLayoutVisualStateCompleted( Platform.Object sender , Platform.Object e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnModeVisualStateCompleted( Platform.Object sender , Platform.Object e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnErrorVisualStateCompleted( Platform.Object sender , Platform.Object e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnDisplayVisualStateCompleted( Platform.Object sender , Platform.Object e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void EnsureScientific ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void EnsureProgrammer ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void SetFontSizeResources ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private Platform.String  GetCurrentLayoutState ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void Calculator_SizeChanged( Object sender , Windows.UI.Xaml.SizeChangedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private Windows.UI.Xaml.Controls.ListView  m_tokenList;
        private Windows.UI.Xaml.Controls.MenuFlyout  m_displayFlyout;
        private bool m_doAnimate;
        private bool m_resultAnimate;
        private bool m_isLastAnimatedInScientific;
        private bool m_isLastAnimatedInProgrammer;
        private bool m_IsLastFlyoutMemory;
        private bool m_IsLastFlyoutHistory;
        private Platform.String  m_openMemoryFlyoutAutomationName;
        private Platform.String  m_closeMemoryFlyoutAutomationName;
        private Platform.String  m_openHistoryFlyoutAutomationName;
        private Platform.String  m_closeHistoryFlyoutAutomationName;
        private Platform.String  m_dockPanelHistoryMemoryLists;
        private Platform.String  m_dockPanelMemoryList;
        private Windows.UI.Xaml.Controls.PivotItem  m_pivotItem;
        private bool m_IsDigit;
        private Memory  m_memory;
        private void HistoryFlyout_Opened( Platform.Object sender , Platform.Object args)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void HistoryFlyout_Closing( Windows.UI.Xaml.Controls.Primitives.FlyoutBase sender , Windows.UI.Xaml.Controls.Primitives.FlyoutBaseClosingEventArgs args)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void HistoryFlyout_Closed( Platform.Object sender , Platform.Object args)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnHideHistoryClicked ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnHideMemoryClicked ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnHistoryItemClicked( CalculatorApp.ViewModel.HistoryItemViewModel e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void ToggleHistoryFlyout( Platform.Object parameter)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void ToggleMemoryFlyout ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private CalculatorApp.HistoryList  m_historyList;
        private bool m_fIsHistoryFlyoutOpen;
        private bool m_fIsMemoryFlyoutOpen;
        private void OnMemoryFlyoutOpened( Platform.Object sender , Platform.Object args)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnMemoryFlyoutClosing( Windows.UI.Xaml.Controls.Primitives.FlyoutBase sender , Windows.UI.Xaml.Controls.Primitives.FlyoutBaseClosingEventArgs args)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnMemoryFlyoutClosed( Platform.Object sender , Platform.Object args)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void SetChildAsMemory ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void SetChildAsHistory ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private Memory  GetMemory ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void EnableControls( bool enable)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void EnableMemoryControls( bool enable)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnMemoryFlyOutTapped( Platform.Object sender , Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnHistoryFlyOutTapped( Platform.Object sender , Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void DockPanelTapped( Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnHistoryAccessKeyInvoked( Windows.UI.Xaml.UIElement sender , Windows.UI.Xaml.Input.AccessKeyInvokedEventArgs args)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnMemoryAccessKeyInvoked( Windows.UI.Xaml.UIElement sender , Windows.UI.Xaml.Input.AccessKeyInvokedEventArgs args)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void OnVisualStateChanged( Platform.Object sender , Windows.UI.Xaml.VisualStateChangedEventArgs e)
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
    }
    
}

