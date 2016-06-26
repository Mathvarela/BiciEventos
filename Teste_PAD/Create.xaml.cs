﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Data.Common;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Services.Maps;
using Windows.UI;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Teste_PAD
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Page
    {
        Geopoint startLocation = null;
        Geopoint endLocation = null;
        int first = 0;
        public Index()
        {
            this.InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["sessionUser"];
            tblock_Welcome.Text = string.Format("Welcome, {0}", value.ToString());
            MapControl.Loaded += MapControl_Loaded;
            MapControl.MapTapped += MapControl_MapTapped;
            int icons = 0;
            localSettings.Values["icons"] = icons;
        }


        private async void b_back_Click(object sender, RoutedEventArgs e)
        {
            var title = "Create Event";
            var content = "Your changes won't be saved if you access another view of the application! ";
            var dialog = new MessageDialog(content, title);
            var yesComand = new Windows.UI.Popups.UICommand("Ok") { Id = 0 };
            var noCommand = new Windows.UI.Popups.UICommand("Cancel") { Id = 1 };
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            dialog.Commands.Add(yesComand);
            dialog.Commands.Add(noCommand);
            var result = await dialog.ShowAsync();
            if (result == yesComand)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
            }
            else
            {
                sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
            }
        }

        private async void MapControl_MapTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int icons = Convert.ToInt32(localSettings.Values["icons"]);
            var tappedGeoPosition = args.Location.Position;
            tblock_latitude.Text = tappedGeoPosition.Latitude.ToString();
            tblock_longitude.Text = tappedGeoPosition.Longitude.ToString();
            if ((icons > 1) && (first != 0))
            {
                tb_Description.Text = icons.ToString();
                MapControl.MapElements.Clear();
                MapControl.Routes.Clear();
                icons = 0;
            }
            else
            {
                MapIcon icon = new MapIcon();
                icon.Location = new Geopoint(tappedGeoPosition);
                icon.ZIndex = 0;
                MapControl.MapElements.Add(icon);
                icons++;
                first++;
                if (icons == 1)
                    startLocation = icon.Location;
                else
                {
                    endLocation = icon.Location;
                        MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(
                        startLocation,
                        endLocation,
                         MapRouteOptimization.Time,
                        MapRouteRestrictions.None
                        );
                        if (routeResult.Status == MapRouteFinderStatus.Success)
                        {
                            MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                            viewOfRoute.RouteColor = Colors.Yellow;
                            viewOfRoute.OutlineColor = Colors.Black;
                            MapControl.Routes.Add(viewOfRoute);
                            await MapControl.TrySetViewBoundsAsync(
                                routeResult.Route.BoundingBox,
                                null,
                                Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
                        }    
                }

            }
            localSettings.Values["icons"] = icons;
        }

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["startLocation"] = null;
            localSettings.Values["endLocation"] = null;
            MapControl.MapServiceToken = "qXGmhIw5FsDkOFhe9Kiu~jeFOhmzd_0JJIWkmDE7ALQ~Aj9YlKn3-rwHLxT_P2jY0-TIbpvgBlxH-cPTDXus16lzezQmApbNS7L1jLgSOr9w";
            MapControl.Center =
                new Geopoint(new BasicGeoposition()
                {
                    Latitude = 40.4528057109565,
                    Longitude = -3.73339807614684
                });
            MapControl.LandmarksVisible = true;
            MapControl.ZoomLevel = 12;
        }

       

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["sessionUser"];
            var client = new HttpClient();
            string getUri = "http://localhost:50859/api/Events";
            var uri = new Uri(getUri);
            var objEvento = new Event()
            {
                Title = tb_Title.Text,
                Description = tb_Description.Text,
                Start_Date = cdp_StartDate.Date.Value.DateTime,
                End_Date = cdp_EndDate.Date.Value.DateTime,
                start_Latitude = startLocation.Position.Latitude,
                end_Latitude = endLocation.Position.Latitude,
                start_Longitude = startLocation.Position.Longitude,
                end_Longitude = endLocation.Position.Longitude,
                Start_Time = tp_Start_Time.Time.ToString(),
                End_Time = tp_End_Time.Time.ToString(),
                Username = value.ToString()
            };
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(Event));
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, objEvento);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            var post_response = await client.PostAsync(getUri.ToString(), theContent);
            var createdDialog = new MessageDialog("Event created!");
            await createdDialog.ShowAsync();
            var response = await client.GetStringAsync(uri);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            var evento = listEvents.FirstOrDefault(x => x.Title == objEvento.Title);
            tblock_Title.Text = objEvento.Title;
            localSettings.Values["Event_Id"] = evento.Id;
            localSettings.Values["Event_Title"] = objEvento.Title;
            localSettings.Values["Event_Description"] = objEvento.Description;
            localSettings.Values["Event_Start_Date"] = objEvento.Start_Date.Date.ToString().Substring(0,10);
            localSettings.Values["Event_Start_Time"] = objEvento.Start_Time;
            localSettings.Values["Event_End_Date"] = objEvento.End_Date.ToString().Substring(0, 10);
            localSettings.Values["Event_End_Time"] = objEvento.End_Time;
            localSettings.Values["Event_startLatitude"] = objEvento.start_Latitude.ToString();
            localSettings.Values["Event_startLongitude"] = objEvento.start_Longitude.ToString();
            localSettings.Values["Event_endLatitude"] = objEvento.end_Latitude.ToString();
            localSettings.Values["Event_endLongitude"] = objEvento.end_Longitude.ToString();
            localSettings.Values["Event_Username"] = objEvento.Username;
            localSettings.Values["Allowed_to_Edit"] = true;
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Details));
            }
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Your changes won't be saved!");
            dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();
            if ((int)res.Id == 0)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(Main));
                }
            }
        }

        private void b_Hamburger_Click(object sender, RoutedEventArgs e)
        {
            sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
        }

        private async void lvi_Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["sessionUser"] = null;
            MessageDialog logoutMessage = new MessageDialog("Logout success");
            await logoutMessage.ShowAsync();
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private async void lvi_Main_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var title = "Create Event";
            var content = "Your changes won't be saved if you access another view of the application! ";
            var dialog = new MessageDialog(content, title);
            var yesComand = new Windows.UI.Popups.UICommand("Ok") { Id = 0 };
            var noCommand = new Windows.UI.Popups.UICommand("Cancel") { Id = 1 };
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            dialog.Commands.Add(yesComand);
            dialog.Commands.Add(noCommand);
            var result = await dialog.ShowAsync();
            if (result == yesComand)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(Main));
                }
            }
            else {
                sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
            }
        }
        private async void lvi_myEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var title = "Create Event";
            var content = "Your changes won't be saved if you access another view of the application! ";
            var dialog = new MessageDialog(content, title);
            var yesComand = new Windows.UI.Popups.UICommand("Ok") { Id = 0 };
            var noCommand = new Windows.UI.Popups.UICommand("Cancel") { Id = 1 };
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            dialog.Commands.Add(yesComand);
            dialog.Commands.Add(noCommand);
            var result = await dialog.ShowAsync();
            if (result == yesComand)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(myEvents));
                }
            }
            else
            {
                sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
            }
        }

        private async void lvi_invite_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var title = "Create Event";
            var content = "Your changes won't be saved if you access another view of the application! ";
            var dialog = new MessageDialog(content, title);
            var yesComand = new Windows.UI.Popups.UICommand("Ok") { Id = 0 };
            var noCommand = new Windows.UI.Popups.UICommand("Cancel") { Id = 1 };
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            dialog.Commands.Add(yesComand);
            dialog.Commands.Add(noCommand);
            var result = await dialog.ShowAsync();
            if (result == yesComand)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(Invites));
                }
            }
            else
            {
                sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
            }
        }
    }
}
