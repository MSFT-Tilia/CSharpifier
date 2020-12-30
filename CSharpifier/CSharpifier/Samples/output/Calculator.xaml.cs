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
        {
            static var uiSettings = ref new UISettings();
            if( uiSettings.AnimationsEnabled) {
                m_doAnimate = true;
                m_resultAnimate = resultAnimate;
                if((( m_isLastAnimatedInScientific && IsScientific) ||( ! m_isLastAnimatedInScientific && ! IsScientific)) &&(( m_isLastAnimatedInProgrammer && IsProgrammer) ||( ! m_isLastAnimatedInProgrammer && ! IsProgrammer))) {
                    this.OnModeVisualStateCompleted( nullptr, nullptr);
                    
                }
                
            }
            
        }
        
        
        public void InitializeHistoryView( CalculatorApp.ViewModel.HistoryViewModel historyVM)
        {
            if( m_historyList == nullptr) {
                m_historyList = ref new HistoryList();
                m_historyList.DataContext = historyVM;
                historyVM.HideHistoryClicked += ref new ViewModel.HideHistoryClickedHandler( this, & Calculator.OnHideHistoryClicked);
                historyVM.HistoryItemClicked += ref new ViewModel.HistoryItemClickedHandler( this, & Calculator.OnHistoryItemClicked);
                
            }
            
        }
        
        
        public void UpdatePanelViewState ()
        {
            UpdateHistoryState();
            UpdateMemoryState();
            
        }
        
        
        public void UnregisterEventHandlers ()
        {
            ExpressionText.UnregisterEventHandlers();
            AlwaysOnTopResults.UnregisterEventHandlers();
            
        }
        
        
        public void CloseHistoryFlyout ()
        {
            if( m_fIsHistoryFlyoutOpen) {
                HistoryFlyout.Hide();
                
            }
            
        }
        
        
        public void CloseMemoryFlyout ()
        {
            if( m_fIsMemoryFlyoutOpen) {
                MemoryFlyout.Hide();
                
            }
            
        }
        
        
        public void SetDefaultFocus ()
        {
            if( ! IsAlwaysOnTop) {
                Results.Focus(.FocusState.Programmatic);
                
            }
            else {
                AlwaysOnTopResults.Focus(.FocusState.Programmatic);
                
            }
            
        }
        
        
        public static Windows.UI.Xaml.Visibility ShouldDisplayHistoryButton( bool isAlwaysOnTop , bool isProgrammer , Windows.UI.Xaml.Visibility dockPanelVisibility)
        {
            return ! isAlwaysOnTop && ! isProgrammer && dockPanelVisibility ==.Visibility.Collapsed ?.Visibility.Visible :
            . Visibility.Collapsed;
            
        }
        
        
        private void OnLoaded( Platform.Object sender , Windows.UI.Xaml.RoutedEventArgs e)
        {
            Model.PropertyChanged += ref new PropertyChangedEventHandler( this, & Calculator.OnCalcPropertyChanged);
            Model.HideMemoryClicked += ref new HideMemoryClickedHandler( this, & Calculator.OnHideMemoryClicked);
            InitializeHistoryView( Model.HistoryVM);
            String historyPaneName = AppResourceProvider.GetInstance().GetResourceString( L"HistoryPane");
            HistoryFlyout.FlyoutPresenterStyle.Setters.Append( ref new Setter( AutomationProperties.NameProperty, historyPaneName));
            String memoryPaneName = AppResourceProvider.GetInstance().GetResourceString( L"MemoryPane");
            MemoryFlyout.FlyoutPresenterStyle.Setters.Append( ref new Setter( AutomationProperties.NameProperty, memoryPaneName));
            if( Windows.Foundation.Metadata.ApiInformation.IsEventPresent( L"Windows.UI.Xaml.Controls.Primitives.FlyoutBase", L"Closing")) {
                HistoryFlyout.Closing += ref new TypedEventHandler < FlyoutBase, FlyoutBaseClosingEventArgs >( this, & Calculator.HistoryFlyout_Closing);
                MemoryFlyout.Closing += ref new TypedEventHandler < FlyoutBase, FlyoutBaseClosingEventArgs >( this, & Calculator.OnMemoryFlyoutClosing);
                
            }
            WeakReference weakThis( this);
            this.Dispatcher.RunAsync( CoreDispatcherPriority.Normal, ref new DispatchedHandler( [ weakThis ]() {
                if( TraceLogger.GetInstance().IsWindowIdInLog( ApplicationView.GetApplicationViewIdForWindow( CoreWindow.GetForCurrentThread()))) {
                    var refThis = weakThis.Resolve < Calculator >();
                    if( refThis != nullptr) {
                        refThis.GetMemory();
                        
                    }
                    
                }
                
            }
            ));
            
        }
        
        
        private void LoadResourceStrings ()
        {
            var resProvider = AppResourceProvider.GetInstance();
            m_openMemoryFlyoutAutomationName = resProvider.GetResourceString( L"MemoryButton_Open");
            m_closeMemoryFlyoutAutomationName = resProvider.GetResourceString( L"MemoryButton_Close");
            m_openHistoryFlyoutAutomationName = resProvider.GetResourceString( L"HistoryButton_Open");
            m_closeHistoryFlyoutAutomationName = resProvider.GetResourceString( L"HistoryButton_Close");
            m_dockPanelHistoryMemoryLists = resProvider.GetResourceString( L"DockPanel_HistoryMemoryLists");
            m_dockPanelMemoryList = resProvider.GetResourceString( L"DockPanel_MemoryList");
            AutomationProperties.SetName( MemoryButton, m_openMemoryFlyoutAutomationName);
            AutomationProperties.SetName( HistoryButton, m_openHistoryFlyoutAutomationName);
            AutomationProperties.SetName( DockPanel, m_dockPanelHistoryMemoryLists);
            
        }
        
        
        private void UpdateViewState ()
        {
            std.wstring state;
            if( IsProgrammer) {
                state = L"Programmer";
                Model.IsDecimalEnabled = false;
                ResultsMVisualStateTrigger.MinWindowHeight = 640;
                
            }
            else if( IsScientific) {
                state = L"Scientific";
                Model.IsDecimalEnabled = true;
                ResultsMVisualStateTrigger.MinWindowHeight = 544;
                
            }
            else {
                state = L"Standard";
                Model.IsDecimalEnabled = true;
                ResultsMVisualStateTrigger.MinWindowHeight = 1;
                
            }
            CloseHistoryFlyout();
            CloseMemoryFlyout();
            VisualStateManager.GoToState( this, ref new String( state.c_str()), true);
            
        }
        
        
        private void UpdateMemoryState ()
        {
            if( ! IsAlwaysOnTop) {
                if( ! Model.IsMemoryEmpty) {
                    MemRecall.IsEnabled = true;
                    ClearMemoryButton.IsEnabled = true;
                    
                }
                else {
                    MemRecall.IsEnabled = false;
                    ClearMemoryButton.IsEnabled = false;
                    
                }
                if( DockPanel.Visibility ==.Visibility.Visible) {
                    CloseMemoryFlyout();
                    SetChildAsMemory();
                    MemoryButton.Visibility =.Visibility.Collapsed;
                    if( m_IsLastFlyoutMemory && ! IsProgrammer) {
                        DockPivot.SelectedIndex = 1;
                        
                    }
                    
                }
                else {
                    MemoryButton.Visibility =.Visibility.Visible;
                    DockMemoryHolder.Child = nullptr;
                    
                }
                
            }
            
        }
        
        
        private void UpdateHistoryState ()
        {
            if( DockPanel.Visibility ==.Visibility.Visible) {
                CloseHistoryFlyout();
                SetChildAsHistory();
                if( ! IsProgrammer && m_IsLastFlyoutHistory) {
                    DockPivot.SelectedIndex = 0;
                    
                }
                
            }
            else {
                DockHistoryHolder.Child = nullptr;
                
            }
            
        }
        
        
        private void OnContextRequested( Windows.UI.Xaml.UIElement sender , Windows.UI.Xaml.Input.ContextRequestedEventArgs e)
        {
            var requestedElement = safe_cast < FrameworkElement >( e.OriginalSource);
            PasteMenuItem.IsEnabled = CopyPasteManager.HasStringToPaste();
            Point point;
            if( e.TryGetPosition( requestedElement, & point)) {
                m_displayFlyout.ShowAt( requestedElement, point);
                
            }
            else {
                m_displayFlyout.ShowAt( requestedElement);
                
            }
            e.Handled = true;
            
        }
        
        
        private void OnContextCanceled( Windows.UI.Xaml.UIElement sender , Windows.UI.Xaml.RoutedEventArgs e)
        {
            m_displayFlyout.Hide();
            
        }
        
        
        private void OnIsScientificPropertyChanged( bool oldValue , bool newValue)
        {
            if( newValue) {
                EnsureScientific();
                
            }
            UpdateViewState();
            UpdatePanelViewState();
            
        }
        
        
        private void OnIsProgrammerPropertyChanged( bool oldValue , bool newValue)
        {
            if( newValue) {
                EnsureProgrammer();
                m_pivotItem = static_cast < Windows.UI.Xaml.Controls.PivotItem >( DockPivot.Items.GetAt( 0));
                DockPivot.Items.RemoveAt( 0);
                
            }
            else {
                if( m_pivotItem != nullptr && DockPivot.Items.Size == 1) {
                    DockPivot.Items.InsertAt( 0, m_pivotItem);
                    
                }
                
            }
            DockPivot.SelectedIndex = 0;
            UpdateViewState();
            UpdatePanelViewState();
            
        }
        
        
        private void OnIsStandardPropertyChanged( bool oldValue , bool newValue)
        {
            UpdateViewState();
            UpdatePanelViewState();
            
        }
        
        
        private void OnIsAlwaysOnTopPropertyChanged( bool oldValue , bool newValue)
        {
            if( newValue) {
                VisualStateManager.GoToState( this, L"DisplayModeAlwaysOnTop", false);
                AlwaysOnTopResults.UpdateScrollButtons();
                
            }
            else {
                VisualStateManager.GoToState( this, L"DisplayModeNormal", false);
                if( ! Model.IsInError) {
                    EnableMemoryControls( true);
                    
                }
                Results.UpdateTextState();
                
            }
            Model.IsMemoryEmpty =( Model.MemorizedNumbers.Size == 0) || IsAlwaysOnTop;
            UpdateViewState();
            UpdatePanelViewState();
            
        }
        
        
        private void OnIsInErrorPropertyChanged ()
        {
            bool isError = Model.IsInError;
            String newState = isError ? L"ErrorLayout" :
            L"NoErrorLayout";
            VisualStateManager.GoToState( this, newState, false);
            if( m_memory != nullptr) {
                m_memory.IsErrorVisualState = isError;
                
            }
            OpsPanel.IsErrorVisualState = isError;
            if( IsScientific && ScientificAngleButtons) {
                ScientificAngleButtons.IsErrorVisualState = isError;
                
            }
            else if( IsProgrammer && ProgrammerDisplayPanel) {
                ProgrammerDisplayPanel.IsErrorVisualState = isError;
                
            }
            
        }
        
        
        private void OnCalcPropertyChanged( Platform.Object sender , Windows.UI.Xaml.Data.PropertyChangedEventArgs e)
        {
            String prop = e.PropertyName;
            if( prop == StandardCalculatorViewModel.IsMemoryEmptyPropertyName) {
                UpdateMemoryState();
                
            }
            else if( prop == StandardCalculatorViewModel.IsInErrorPropertyName) {
                OnIsInErrorPropertyChanged();
                
            }
            
        }
        
        
        private void OnLayoutVisualStateCompleted( Platform.Object sender , Platform.Object e)
        {
            UpdatePanelViewState();
            
        }
        
        
        private void OnModeVisualStateCompleted( Platform.Object sender , Platform.Object e)
        {
            m_isLastAnimatedInScientific = IsScientific;
            m_isLastAnimatedInProgrammer = IsProgrammer;
            if( m_doAnimate) {
                m_doAnimate = false;
                if( m_resultAnimate) {
                    m_resultAnimate = false;
                    Animate.Begin();
                    
                }
                else {
                    AnimateWithoutResult.Begin();
                    
                }
                
            }
            if( IsProgrammer) {
                AutomationProperties.SetName( DockPanel, m_dockPanelMemoryList);
                
            }
            else {
                AutomationProperties.SetName( DockPanel, m_dockPanelHistoryMemoryLists);
                
            }
            
        }
        
        
        private void OnErrorVisualStateCompleted( Platform.Object sender , Platform.Object e)
        {
            SetDefaultFocus();
            
        }
        
        
        private void OnDisplayVisualStateCompleted( Platform.Object sender , Platform.Object e)
        {
            SetDefaultFocus();
            
        }
        
        
        private void EnsureScientific ()
        {
            OpsPanel.EnsureScientificOps();
            if( ! ScientificAngleButtons) {
                this.FindName( L"ScientificAngleButtons");
                
            }
            
        }
        
        
        private void EnsureProgrammer ()
        {
            if( ! ProgrammerOperators) {
                this.FindName( L"ProgrammerOperators");
                
            }
            if( ! ProgrammerDisplayPanel) {
                this.FindName( L"ProgrammerDisplayPanel");
                
            }
            OpsPanel.EnsureProgrammerRadixOps();
            ProgrammerOperators.SetRadixButton( Model.CurrentRadixType);
            
        }
        
        
        private void SetFontSizeResources ()
        {
            static const FontTable fontTables [ ] = {
                {
                    L"Arab", 104, 29.333, 23, 40, 56, 40, 56
                }
                , {
                    L"ArabExt", 104, 29.333, 23, 40, 56, 40, 56
                }
                , {
                    L"Beng", 104, 26, 17, 40, 56, 40, 56
                }
                , {
                    L"Deva", 104, 29.333, 20.5, 40, 56, 40, 56
                }
                , {
                    L"Gujr", 104, 29.333, 18.5, 40, 56, 40, 56
                }
                , {
                    L"Khmr", 104, 29.333, 19.5, 40, 56, 40, 56
                }
                , {
                    L"Knda", 104, 25, 17, 40, 56, 40, 56
                }
                , {
                    L"Laoo", 104, 28, 18, 40, 56, 40, 56
                }
                , {
                    L"Latn", 104, 29.333, 23, 40, 56, 40, 56
                }
                , {
                    L"Mlym", 80, 22, 15.5, 30, 56, 35, 48
                }
                , {
                    L"Mymr", 104, 29.333, 20, 35, 48, 36, 48
                }
                , {
                    L"Orya", 88, 26, 20, 40, 56, 40, 56
                }
                , {
                    L"TamlDec", 77, 25, 16, 28, 48, 34, 48
                }
                , {
                    L"Telu", 104, 25, 16.5, 40, 56, 40, 56
                }
                , {
                    L"Thai", 104, 28, 18, 40, 56, 40, 56
                }
                , {
                    L"Tibt", 104, 29.333, 20, 40, 56, 40, 56
                }
                , {
                    L"Default", 104, 29.333, 23, 40, 56, 40, 56
                }
                
            }
            ;
            DecimalFormatter formatter = LocalizationService.GetInstance().GetRegionalSettingsAwareDecimalFormatter();
            const FontTable * currentItem = fontTables;
            while( currentItem.numericSystem.compare( std.wstring( L"Default")) != 0 && currentItem.numericSystem.compare( formatter.NumeralSystem.Data()) != 0) {
                currentItem ++;
                
            }
            this.Resources.Insert( StringReference( L"ResultFullFontSize"), currentItem.fullFont);
            this.Resources.Insert( StringReference( L"ResultFullMinFontSize"), currentItem.fullFontMin);
            this.Resources.Insert( StringReference( L"ResultPortraitMinFontSize"), currentItem.portraitMin);
            this.Resources.Insert( StringReference( L"ResultSnapFontSize"), currentItem.snapFont);
            this.Resources.Insert( StringReference( L"CalcButtonCaptionSizeOverride"), currentItem.fullNumPadFont);
            this.Resources.Insert( StringReference( L"CalcButtonScientificSnapCaptionSizeOverride"), currentItem.snapScientificNumPadFont);
            this.Resources.Insert( StringReference( L"CalcButtonScientificPortraitCaptionSizeOverride"), currentItem.portraitScientificNumPadFont);
            
        }
        
        
        private Platform.String  GetCurrentLayoutState ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void Calculator_SizeChanged( Object sender , Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if( Model.IsAlwaysOnTop) {
                AlwaysOnTopResults.UpdateScrollButtons();
                
            }
            
        }
        
        
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
        {
            m_fIsHistoryFlyoutOpen = true;
            m_IsLastFlyoutMemory = false;
            m_IsLastFlyoutHistory = true;
            EnableControls( false);
            AutomationProperties.SetName( HistoryButton, m_closeHistoryFlyoutAutomationName);
            
        }
        
        
        private void HistoryFlyout_Closing( Windows.UI.Xaml.Controls.Primitives.FlyoutBase sender , Windows.UI.Xaml.Controls.Primitives.FlyoutBaseClosingEventArgs args)
        {
            AutomationProperties.SetName( HistoryButton, m_openHistoryFlyoutAutomationName);
            
        }
        
        
        private void HistoryFlyout_Closed( Platform.Object sender , Platform.Object args)
        {
            AutomationProperties.SetName( HistoryButton, m_openHistoryFlyoutAutomationName);
            m_fIsHistoryFlyoutOpen = false;
            EnableControls( true);
            if( HistoryButton.IsEnabled && HistoryButton.Visibility ==.Visibility.Visible) {
                HistoryButton.Focus(.FocusState.Programmatic);
                
            }
            FullscreenFlyoutClosed();
            
        }
        
        
        private void OnHideHistoryClicked ()
        {
            ToggleHistoryFlyout( nullptr);
            
        }
        
        
        private void OnHideMemoryClicked ()
        {
            if( ! m_fIsMemoryFlyoutOpen) {
                this.Focus(.FocusState.Programmatic);
                
            }
            MemoryFlyout.Hide();
            
        }
        
        
        private void OnHistoryItemClicked( CalculatorApp.ViewModel.HistoryItemViewModel e)
        {
            Model.SelectHistoryItem( e);
            CloseHistoryFlyout();
            this.Focus(.FocusState.Programmatic);
            
        }
        
        
        private void ToggleHistoryFlyout( Platform.Object parameter)
        {
            if( Model.IsProgrammer || DockPanel.Visibility ==.Visibility.Visible) {
                return;
                
            }
            if( m_fIsHistoryFlyoutOpen) {
                HistoryFlyout.Hide();
                
            }
            else {
                HistoryFlyout.Content = m_historyList;
                m_historyList.RowHeight = NumpadPanel.ActualHeight;
                FlyoutBase.ShowAttachedFlyout( HistoryButton);
                
            }
            
        }
        
        
        private void ToggleMemoryFlyout ()
        {
            if( DockPanel.Visibility ==.Visibility.Visible) {
                return;
                
            }
            if( m_fIsMemoryFlyoutOpen) {
                MemoryFlyout.Hide();
                
            }
            else {
                MemoryFlyout.Content = GetMemory();
                m_memory.RowHeight = NumpadPanel.ActualHeight;
                FlyoutBase.ShowAttachedFlyout( MemoryButton);
                
            }
            
        }
        
        
        private CalculatorApp.HistoryList  m_historyList;
        private bool m_fIsHistoryFlyoutOpen;
        private bool m_fIsMemoryFlyoutOpen;
        private void OnMemoryFlyoutOpened( Platform.Object sender , Platform.Object args)
        {
            m_IsLastFlyoutMemory = true;
            m_IsLastFlyoutHistory = false;
            m_fIsMemoryFlyoutOpen = true;
            AutomationProperties.SetName( MemoryButton, m_closeMemoryFlyoutAutomationName);
            EnableControls( false);
            
        }
        
        
        private void OnMemoryFlyoutClosing( Windows.UI.Xaml.Controls.Primitives.FlyoutBase sender , Windows.UI.Xaml.Controls.Primitives.FlyoutBaseClosingEventArgs args)
        {
            AutomationProperties.SetName( MemoryButton, m_openMemoryFlyoutAutomationName);
            
        }
        
        
        private void OnMemoryFlyoutClosed( Platform.Object sender , Platform.Object args)
        {
            AutomationProperties.SetName( MemoryButton, m_openMemoryFlyoutAutomationName);
            m_fIsMemoryFlyoutOpen = false;
            EnableControls( true);
            if( MemoryButton.IsEnabled) {
                MemoryButton.Focus(.FocusState.Programmatic);
                
            }
            FullscreenFlyoutClosed();
            
        }
        
        
        private void SetChildAsMemory ()
        {
            DockMemoryHolder.Child = GetMemory();
            
        }
        
        
        private void SetChildAsHistory ()
        {
            if( m_historyList == nullptr) {
                InitializeHistoryView( Model.HistoryVM);
                
            }
            DockHistoryHolder.Child = m_historyList;
            
        }
        
        
        private Memory  GetMemory ()
        { throw new System.NotImplementedException(); /* CSharpifier Warning */ }
        
        private void EnableControls( bool enable)
        {
            OpsPanel.IsEnabled = enable;
            EnableMemoryControls( enable);
            
        }
        
        
        private void EnableMemoryControls( bool enable)
        {
            MemButton.IsEnabled = enable;
            MemMinus.IsEnabled = enable;
            MemPlus.IsEnabled = enable;
            if( ! Model.IsMemoryEmpty) {
                MemRecall.IsEnabled = enable;
                ClearMemoryButton.IsEnabled = enable;
                
            }
            
        }
        
        
        private void OnMemoryFlyOutTapped( Platform.Object sender , Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Grid grid = safe_cast < Grid >( sender);
            Point point = e.GetPosition( nullptr);
            if( point.Y <( grid.ActualHeight - NumpadPanel.ActualHeight)) {
                MemoryFlyout.Hide();
                
            }
            
        }
        
        
        private void OnHistoryFlyOutTapped( Platform.Object sender , Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Grid grid = safe_cast < Grid >( sender);
            Point point = e.GetPosition( nullptr);
            if( point.Y <( grid.ActualHeight - NumpadPanel.ActualHeight)) {
                HistoryFlyout.Hide();
                
            }
            
        }
        
        
        private void DockPanelTapped( Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            int index = DockPivot.SelectedIndex;
            if( index == 1 && ! IsProgrammer) {
                SetChildAsMemory();
                
            }
            m_IsLastFlyoutMemory = false;
            m_IsLastFlyoutHistory = false;
            
        }
        
        
        private void OnHistoryAccessKeyInvoked( Windows.UI.Xaml.UIElement sender , Windows.UI.Xaml.Input.AccessKeyInvokedEventArgs args)
        {
            DockPivot.SelectedItem = HistoryPivotItem;
            
        }
        
        
        private void OnMemoryAccessKeyInvoked( Windows.UI.Xaml.UIElement sender , Windows.UI.Xaml.Input.AccessKeyInvokedEventArgs args)
        {
            DockPivot.SelectedItem = MemoryPivotItem;
            
        }
        
        
        private void OnVisualStateChanged( Platform.Object sender , Windows.UI.Xaml.VisualStateChangedEventArgs e)
        {
            if( ! IsStandard && ! IsScientific && ! IsProgrammer) {
                return;
                
            }
            var mode = IsStandard ? ViewMode.Standard :
            IsScientific ? ViewMode.Scientific :
            ViewMode.Programmer;
            TraceLogger.GetInstance().LogVisualStateChanged( mode, e.NewState.Name, IsAlwaysOnTop);
            
        }
        
        
    }
    
}

