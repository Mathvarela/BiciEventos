﻿#pragma checksum "C:\Users\luisdeolpy\Documents\GitHub\BiciEventos\Teste_PAD\Edit.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "12EA708CBF46453365B89D2F7D04A920"
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
    partial class Edit : 
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
                    global::Windows.UI.Xaml.Controls.AppBarButton element1 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 15 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element1).Click += this.AppBarButton_Click;
                    #line default
                }
                break;
            case 2:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element2 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 16 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element2).Click += this.AppBarButton_Click_1;
                    #line default
                }
                break;
            case 3:
                {
                    this.sv_Menu = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 4:
                {
                    this.b_back = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 23 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.b_back).Click += this.b_back_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.b_Hamburger = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 24 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.b_Hamburger).Click += this.b_Hamburger_Click;
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
                    #line 28 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)this.lvi_myEvents).Tapped += this.lvi_myEvents_Tapped;
                    #line default
                }
                break;
            case 9:
                {
                    this.lvi_invites = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    #line 29 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)this.lvi_invites).Tapped += this.lvi_invite_Tapped;
                    #line default
                }
                break;
            case 10:
                {
                    this.lvi_Create = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    #line 30 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)this.lvi_Create).Tapped += this.lvi_Create_Tapped;
                    #line default
                }
                break;
            case 11:
                {
                    this.lvi_Logout = (global::Windows.UI.Xaml.Controls.ListViewItem)(target);
                    #line 31 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListViewItem)this.lvi_Logout).Tapped += this.lvi_Logout_Tapped;
                    #line default
                }
                break;
            case 12:
                {
                    global::Windows.UI.Xaml.Controls.StackPanel element12 = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                    #line 36 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.StackPanel)element12).Loaded += this.StackPanel_Loaded;
                    #line default
                }
                break;
            case 13:
                {
                    this.tblock_Title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 14:
                {
                    this.tb_Title = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 15:
                {
                    this.cdp_StartDate = (global::Windows.UI.Xaml.Controls.CalendarDatePicker)(target);
                }
                break;
            case 16:
                {
                    this.tp_Start_Time = (global::Windows.UI.Xaml.Controls.TimePicker)(target);
                }
                break;
            case 17:
                {
                    this.cdp_EndDate = (global::Windows.UI.Xaml.Controls.CalendarDatePicker)(target);
                }
                break;
            case 18:
                {
                    this.tp_End_Time = (global::Windows.UI.Xaml.Controls.TimePicker)(target);
                }
                break;
            case 19:
                {
                    this.tblock_Description = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 20:
                {
                    this.tb_Description = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 21:
                {
                    this.MapControl = (global::Windows.UI.Xaml.Controls.Maps.MapControl)(target);
                    #line 47 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.Maps.MapControl)this.MapControl).MapTapped += this.MapControl_MapTapped;
                    #line 47 "..\..\..\Edit.xaml"
                    ((global::Windows.UI.Xaml.Controls.Maps.MapControl)this.MapControl).Loaded += this.MapControl_Loaded;
                    #line default
                }
                break;
            case 22:
                {
                    this.tblock_latitude = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 23:
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

