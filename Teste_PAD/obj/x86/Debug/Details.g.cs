﻿#pragma checksum "C:\Users\luisdeolpy\Documents\GitHub\BiciEventos\Teste_PAD\Details.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9DAA58F9C46FA25E68933D8F5656C34D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Teste_PAD
{
    partial class Details : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.sv_Menu = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 2:
                {
                    this.b_back = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 19 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.b_back).Click += this.b_back_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.b_Hamburger = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 20 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.b_Hamburger).Click += this.b_Hamburger_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.b_Edit = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 21 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.b_Edit).Click += this.b_Edit_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.b_Delete = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.b_Delete).Click += this.b_Delete_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.tblock_Welcome = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.lvi_Main = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                }
                break;
            case 8:
                {
                    this.lvi_myEvents = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    #line 26 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)this.lvi_myEvents).Tapped += this.lvi_myEvents_Tapped;
                    #line default
                }
                break;
            case 9:
                {
                    this.lvi_invites = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    #line 27 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)this.lvi_invites).Tapped += this.lvi_invite_Tapped;
                    #line default
                }
                break;
            case 10:
                {
                    this.lvi_Create = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    #line 28 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)this.lvi_Create).Tapped += this.lvi_Create_Tapped;
                    #line default
                }
                break;
            case 11:
                {
                    this.lvi_Logout = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    #line 29 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)this.lvi_Logout).Tapped += this.lvi_Logout_Tapped;
                    #line default
                }
                break;
            case 12:
                {
                    global::Windows.UI.Xaml.Controls.StackPanel element12 = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                    #line 34 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.StackPanel)element12).Loaded += this.StackPanel_Loaded;
                    #line default
                }
                break;
            case 13:
                {
                    this.b_going = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 35 "..\..\..\Details.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.b_going).Click += this.b_going_Click;
                    #line default
                }
                break;
            case 14:
                {
                    this.tblock_Users_Participating = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 15:
                {
                    this.b_invite = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 16:
                {
                    this.tblock_Title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 17:
                {
                    this.tblock_Title_value = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 18:
                {
                    this.tblock_Start_Date = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 19:
                {
                    this.tblock_Start_Date_value = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 20:
                {
                    this.tblock_Start_Time = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 21:
                {
                    this.tblock_Start_Time_value = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 22:
                {
                    this.tblock_End_Date = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 23:
                {
                    this.tblock_End_Date_value = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 24:
                {
                    this.tblock_End_Time = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 25:
                {
                    this.tblock_End_Time_value = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 26:
                {
                    this.tblock_Description = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 27:
                {
                    this.tblock_Description_value = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 28:
                {
                    this.MapControl = (global::Windows.UI.Xaml.Controls.Maps.MapControl)(target);
                }
                break;
            case 29:
                {
                    this.tblock_latitude = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 30:
                {
                    this.tblock_longitude = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

