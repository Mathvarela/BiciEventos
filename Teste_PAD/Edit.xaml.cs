using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Teste_PAD
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Edit : Page
    {
        Geopoint startLocation = null;
        Geopoint endLocation = null;
        public Edit()
        {
            this.InitializeComponent();
        }

        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Details));
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

        private void lvi_Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Index));
            }
        }

        private void lvi_myEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(myEvents));
            }
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            tb_Title.Text = localSettings.Values["Event_Title"].ToString();
            cdp_StartDate.Date = Convert.ToDateTime(localSettings.Values["Event_Start_Date"]);
            tp_Start_Time.Time = TimeSpan.Parse(localSettings.Values["Event_Start_Time"].ToString());
            cdp_EndDate.Date = Convert.ToDateTime(localSettings.Values["Event_End_Date"]);
            tp_End_Time.Time = TimeSpan.Parse(localSettings.Values["Event_End_Time"].ToString());
            tb_Description.Text = localSettings.Values["Event_Description"].ToString();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["sessionUser"];
            var client = new HttpClient();
            string getUri = string.Format("http://localhost:50859/api/Events/{0}",localSettings.Values["Event_id"].ToString());
            var uri = new Uri(getUri);
            var evento = new Event()
            {
                Id = Convert.ToInt32(localSettings.Values["Event_id"]),
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
            jsonSer.WriteObject(ms, evento);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            var put_response = await client.PutAsync(uri, theContent);
            var editDialog = new MessageDialog("Changes are saved!");
            await editDialog.ShowAsync();
            localSettings.Values["Event_Title"] = evento.Title;
            localSettings.Values["Event_Description"] = evento.Description;
            localSettings.Values["Event_Start_Date"] = evento.Start_Date.Date.ToString();
            localSettings.Values["Event_Start_Time"] = evento.Start_Time;
            localSettings.Values["Event_End_Date"] = evento.End_Date.ToString();
            localSettings.Values["Event_End_Time"] = evento.End_Time;
            localSettings.Values["Event_startLatitude"] = evento.start_Latitude.ToString();
            localSettings.Values["Event_startLongitude"] = evento.start_Longitude.ToString();
            localSettings.Values["Event_endLatitude"] = evento.end_Latitude.ToString();
            localSettings.Values["Event_endLongitude"] = evento.end_Longitude.ToString();
            localSettings.Values["Event_Username"] = evento.Username;
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

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            MapControl.Center =
               new Geopoint(new BasicGeoposition()
               {
                   Latitude = Convert.ToDouble(localSettings.Values["Event_Latitude"]),
                   Longitude = Convert.ToDouble(localSettings.Values["Event_Longitude"])
               });
            MapControl.LandmarksVisible = true;
            MapControl.ZoomLevel = 12;
            MapIcon Event_Map_Icon = new MapIcon();
            Event_Map_Icon.Location = MapControl.Center;
            Event_Map_Icon.ZIndex = 0;
            MapControl.MapElements.Add(Event_Map_Icon);
        }

        private void MapControl_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            var tappedGeoPosition = args.Location.Position;
            tblock_latitude.Text = tappedGeoPosition.Latitude.ToString();
            tblock_longitude.Text = tappedGeoPosition.Longitude.ToString();
        }

        private void lvi_invite_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Invites));
            }
        }
    }
}
