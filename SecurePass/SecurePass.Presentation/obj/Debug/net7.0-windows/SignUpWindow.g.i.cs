﻿#pragma checksum "..\..\..\SignUpWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F5026CFDB4731454AA08E131CFFAA9AA2300F764"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SecurePass.Presentation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace SecurePass.Presentation {
    
    
    /// <summary>
    /// SignUpWindow
    /// </summary>
    public partial class SignUpWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 65 "..\..\..\SignUpWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image LangImage;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\SignUpWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EmailTextBox;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\SignUpWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label EmailErrorLabel;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\SignUpWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PasswordTextBox;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\SignUpWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label PasswordErrorLabel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SecurePass.Presentation;component/signupwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SignUpWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LangImage = ((System.Windows.Controls.Image)(target));
            
            #line 65 "..\..\..\SignUpWindow.xaml"
            this.LangImage.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Lang_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.EmailTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 71 "..\..\..\SignUpWindow.xaml"
            this.EmailTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.RemoveText);
            
            #line default
            #line hidden
            
            #line 71 "..\..\..\SignUpWindow.xaml"
            this.EmailTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.AddText);
            
            #line default
            #line hidden
            return;
            case 3:
            this.EmailErrorLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.PasswordTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 81 "..\..\..\SignUpWindow.xaml"
            this.PasswordTextBox.GotFocus += new System.Windows.RoutedEventHandler(this.RemoveText);
            
            #line default
            #line hidden
            
            #line 81 "..\..\..\SignUpWindow.xaml"
            this.PasswordTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.AddText);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PasswordErrorLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            
            #line 90 "..\..\..\SignUpWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SignUpButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 100 "..\..\..\SignUpWindow.xaml"
            ((System.Windows.Documents.Hyperlink)(target)).Click += new System.Windows.RoutedEventHandler(this.LoginLabel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

