﻿#pragma checksum "..\..\..\Pages\OfficeManagementPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "53DF69AFBFE211E4F45A4055B85CBC27A9B89200339D6E57808B1D8A5BA72179"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using Manager.Pages;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using SharpVectors.Converters;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Manager.Pages {
    
    
    /// <summary>
    /// OfficeManagementPage
    /// </summary>
    public partial class OfficeManagementPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid officeInfoPanel;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox officeInfoForNumber;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox officeInfoForOfficeName;
        
        #line default
        #line hidden
        
        
        #line 181 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox officeInfoForOfficeFounder;
        
        #line default
        #line hidden
        
        
        #line 196 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox officeInfoForType;
        
        #line default
        #line hidden
        
        
        #line 214 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid warningPanel;
        
        #line default
        #line hidden
        
        
        #line 242 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock userInfoForErrorLog;
        
        #line default
        #line hidden
        
        
        #line 409 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox searchOffice;
        
        #line default
        #line hidden
        
        
        #line 425 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox searchTypeComboBox;
        
        #line default
        #line hidden
        
        
        #line 447 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView officeListView;
        
        #line default
        #line hidden
        
        
        #line 470 "..\..\..\Pages\OfficeManagementPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar searchTaskProgressbae;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Manager;component/pages/officemanagementpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\OfficeManagementPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.officeInfoPanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            
            #line 58 "..\..\..\Pages\OfficeManagementPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OfficeInfoPanelClose_OnClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.officeInfoForNumber = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.officeInfoForOfficeName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.officeInfoForOfficeFounder = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.officeInfoForType = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.warningPanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.userInfoForErrorLog = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            
            #line 266 "..\..\..\Pages\OfficeManagementPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OfficeInfoSave_ButtonClick);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 320 "..\..\..\Pages\OfficeManagementPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OfficeInfoDelete_ButtonClick);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 376 "..\..\..\Pages\OfficeManagementPage.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Grid_Loaded);
            
            #line default
            #line hidden
            return;
            case 12:
            this.searchOffice = ((System.Windows.Controls.TextBox)(target));
            
            #line 422 "..\..\..\Pages\OfficeManagementPage.xaml"
            this.searchOffice.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.searchOffice_TextChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            this.searchTypeComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 437 "..\..\..\Pages\OfficeManagementPage.xaml"
            this.searchTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.searchTypeComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 14:
            this.officeListView = ((System.Windows.Controls.ListView)(target));
            
            #line 451 "..\..\..\Pages\OfficeManagementPage.xaml"
            this.officeListView.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.OfficeListView_PreviewMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 464 "..\..\..\Pages\OfficeManagementPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OfficeCreateMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.searchTaskProgressbae = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

