using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Teste_PAD
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Details : Page
    {
        public Details()
        {
            this.InitializeComponent();
            MapControl.Loaded += MapControl_Loaded;
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Allowed_to_Edit"].Equals(true))
            {
                b_Edit.Visibility = Windows.UI.Xaml.Visibility.Visible;
                b_Delete.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var evento = (Event)e.Parameter;
            if (evento != null)
            {
                tblock_Title_value.Text = evento.Title;
                tblock_Start_Date_value.Text = evento.Start_Date.ToString("g");
                tblock_Start_Time_value.Text = evento.Start_Time;
                tblock_End_Date_value.Text = evento.End_Date.ToString("g");
                tblock_End_Time_value.Text = evento.End_Time;
                tblock_Description_value.Text = evento.Description;
            }
            base.OnNavigatedTo(e);


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

        private void lvi_Main_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Main));
            }
        }

        private void b_Hamburger_Click(object sender, RoutedEventArgs e)
        {
            sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
        }

        private async void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string username = localSettings.Values["sessionUser"].ToString();
            int eventId = Convert.ToInt32(localSettings.Values["Event_Id"]);
            var client = new HttpClient();
            string getUri = "http://localhost:50859/api/Event_Going";
            var uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Event_Going> events = JsonConvert.DeserializeObject<List<Event_Going>>(response);
            var usersParticipations = events.FindAll(x => x.EventId.Equals(eventId));
            tblock_Users_Participating.Text = usersParticipations.Count.ToString() + " Confirmados";
        }

        private void lvi_Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Index));
            }
        }

        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Main));
            }
        }

        private async void ShowMyLocation()
        {
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            double latitude = myGeoposition.Coordinate.Point.Position.Latitude;
            double longitude = myGeoposition.Coordinate.Point.Position.Longitude;
            MapControl.Center =
                new Geopoint(new BasicGeoposition()
                {
                    Latitude = latitude,
                    Longitude = longitude
                });
            MapControl.LandmarksVisible = true;
            MapControl.ZoomLevel = 12;
        }
        private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Geopoint start = new Geopoint(new BasicGeoposition()
            {
                Latitude = Convert.ToDouble(localSettings.Values["Event_startLatitude"]),
                Longitude = Convert.ToDouble(localSettings.Values["Event_startLongitude"]),
            });
            Geopoint end = new Geopoint(new BasicGeoposition()
            {
                Latitude = Convert.ToDouble(localSettings.Values["Event_endLatitude"]),
                Longitude = Convert.ToDouble(localSettings.Values["Event_endLongitude"]),
            });
            MapControl.Center = start;
            MapControl.LandmarksVisible = true;
            MapControl.ZoomLevel = 12;
            MapIcon startIcon = new MapIcon();
            startIcon.Location = start;
            startIcon.ZIndex = 0;
            MapControl.MapElements.Add(startIcon);

            MapIcon endIcon = new MapIcon();
            endIcon.Location = end;
            endIcon.ZIndex = 0;
            MapControl.MapElements.Add(endIcon);

            MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(
                        startIcon.Location,
                        endIcon.Location,
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
        private void lvi_myEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(myEvents));
            }
        }

        private void b_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Edit));
            }
        }

        private async void b_going_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string username = localSettings.Values["sessionUser"].ToString();
            int eventId = Convert.ToInt32(localSettings.Values["Event_Id"]);
            var client = new HttpClient();
            string getUri = "http://localhost:50859/api/Event_Going";
            var uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Event_Going> events = JsonConvert.DeserializeObject <List<Event_Going>>(response);
            var regParticipations = events.FindAll(x => x.Username == username && x.EventId.Equals (eventId));
            if (regParticipations.Count == 0){
                var participation = new Event_Going()
                {
                    EventId = Convert.ToInt32(localSettings.Values["Event_Id"]),
                    Username = username
                };
                DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(Event_Going));
                MemoryStream ms = new MemoryStream();
                jsonSer.WriteObject(ms, participation);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                try
                {
                    var post_response = await client.PostAsync(getUri.ToString(), theContent);
                    var createdDialog = new MessageDialog("You're confirmated for this event!");
                    await createdDialog.ShowAsync();
                    tblock_Users_Participating.Text = (Convert.ToInt32(tblock_Users_Participating.Text.Substring(0,1))+1).ToString() + " Confirmados";
                }
                catch { }
            }        
        }

        private async void b_Delete_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:50859/api/Events";
            string getInvitesUri = "http://localhost:50859/api/Invites";
            Uri uri = new Uri(getUri);
            Uri invitesUri = new Uri(getInvitesUri);
            var response_Invites = await client.GetStringAsync(invitesUri);
            var response = await client.GetStringAsync(uri);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            List<Invite> listInvites = JsonConvert.DeserializeObject<List<Invite>>(response_Invites);
            var myInvites = listInvites.FindAll(x => x.Username == localSettings.Values["sessionUser"].ToString());
            string DeleteUri = string.Format("http://localhost:50859/api/Events/{0}", localSettings.Values["Event_id"].ToString());
            foreach (var item in myInvites)
            {
                string deleteInviteUri = string.Format("http://localhost:50859/api/Invites/{0}", item.EventId.ToString());
                var deleteInviteResponse = await client.DeleteAsync(deleteInviteUri);
            }
            var title = "Delete event";
            var content = "Are you sure that you want to delete this event?";
            var dialog = new MessageDialog(content, title);
            var yesComand = new Windows.UI.Popups.UICommand("Yes") { Id = 0 };
            var noCommand = new Windows.UI.Popups.UICommand("No") { Id = 1 };
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;
            dialog.Commands.Add(yesComand);
            dialog.Commands.Add(noCommand);
            var result = await dialog.ShowAsync();
            if (result == yesComand)
            {
                var evento = listEvents.FirstOrDefault(x => x.Id == Convert.ToInt32(localSettings.Values["Event_Id"]));
                var deleteResponse = await client.DeleteAsync(DeleteUri);
                var deletesuccessDialog = new MessageDialog(String.Format("Event {0} was successfully deleted!", localSettings.Values["Event_Title"].ToString()));
                await deletesuccessDialog.ShowAsync();
            };
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(myEvents));
            }
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
