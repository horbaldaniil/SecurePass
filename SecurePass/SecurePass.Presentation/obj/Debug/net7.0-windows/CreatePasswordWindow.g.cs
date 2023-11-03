﻿#pragma checksum "..\..\..\CreatePasswordWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3B89F2401E2D978691EBE73CBD1E55565E1648FD"
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
    /// CreatePasswordWindow
    /// </summary>
    public partial class CreatePasswordWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 155 "..\..\..\CreatePasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border GenaratePasswordBackground;
        
        #line default
        #line hidden
        
        
        #line 157 "..\..\..\CreatePasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border GenaratePassword;
        
        #line default
        #line hidden
        
        
        #line 204 "..\..\..\CreatePasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border SettingsPanel;
        
        #line default
        #line hidden
        
        
        #line 237 "..\..\..\CreatePasswordWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border UserInfoPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/SecurePass.Presentation;component/createpasswordwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CreatePasswordWindow.xaml"
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
            
            #line 8 "..\..\..\CreatePasswordWindow.xaml"
            ((SecurePass.Presentation.CreatePasswordWindow)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.GenaratePasswordBackground = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.GenaratePassword = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            
            #line 193 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SettingsPanel = ((System.Windows.Controls.Border)(target));
            return;
            case 6:
            this.UserInfoPanel = ((System.Windows.Controls.Border)(target));
            return;
            case 7:
            
            #line 244 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LogOut_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 268 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.UserImage_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 276 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Border)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.SettingsImage_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 284 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Passwords_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 285 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Folders_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 286 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Trash_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 300 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.RemoveText);
            
            #line default
            #line hidden
            
            #line 300 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).LostFocus += new System.Windows.RoutedEventHandler(this.AddText);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 309 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.RemoveText);
            
            #line default
            #line hidden
            
            #line 309 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).LostFocus += new System.Windows.RoutedEventHandler(this.AddText);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 317 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.RemoveText);
            
            #line default
            #line hidden
            
            #line 317 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).LostFocus += new System.Windows.RoutedEventHandler(this.AddText);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 324 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.GeneratePassword_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 340 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveButton_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 347 "..\..\..\CreatePasswordWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

