using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Teste_PAD.Models;
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

namespace Teste_PAD.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Details : Page
    {
        private int _eventId;

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
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var evento = (Event)e.Parameter;
            if (evento != null)
            {
                tblock_Title_value.Text = evento.Title;
                tblock_Start_Date_value.Text = evento.StartDate.ToString("g");
                tblock_Start_Time_value.Text = evento.StartTime;
                tblock_End_Date_value.Text = evento.EndDate.ToString("g");
                tblock_End_Time_value.Text = evento.EndTime;
                tblock_Description_value.Text = evento.Description;
            }
            _eventId = (int) localSettings.Values["EventId"];
            base.OnNavigatedTo(e);


        }

        private async void lvi_Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["sessionUser"] = null;
            MessageDialog logoutMessage = new MessageDialog("Logout success");
            await logoutMessage.ShowAsync();
            Frame?.Navigate(typeof(MainPage));
        }

        private void lvi_Main_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Main));
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
            string getUri = "http://localhost:5000/api/Attendances";
            var uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Attendance> events = JsonConvert.DeserializeObject<List<Attendance>>(response);
            var usersParticipations = events.FindAll(x => x.EventId.Equals(eventId));
            tblock_Users_Participating.Text = usersParticipations.Count.ToString() + " Confirmados";
        }

        private void lvi_Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Index));
        }

        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Main));
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
            MapIcon startIcon = new MapIcon
            {
                Location = start,
                ZIndex = 0
            };
            MapControl.MapElements.Add(startIcon);

            MapIcon endIcon = new MapIcon
            {
                Location = end,
                ZIndex = 0
            };
            MapControl.MapElements.Add(endIcon);

            MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(
                        startIcon.Location,
                        endIcon.Location,
                         MapRouteOptimization.Time,
                        MapRouteRestrictions.None
                        );
            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route)
                {
                    RouteColor = Colors.Yellow,
                    OutlineColor = Colors.Black
                };
                MapControl.Routes.Add(viewOfRoute);
                await MapControl.TrySetViewBoundsAsync(
                    routeResult.Route.BoundingBox,
                    null,
                    Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }
        private void lvi_myEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(MyEvents));
        }

        private async void b_Edit_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var eventId = Convert.ToInt32(localSettings.Values["EventId"]);
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:5000/api/Events";
            Uri uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            var evento = listEvents.SingleOrDefault(a => a.Id == eventId);

            Frame?.Navigate(typeof(Edit), evento);
        }

        private async void b_going_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var userId = int.Parse(localSettings.Values["sessionUser"].ToString());
            var client = new HttpClient();
            string getUri = "http://localhost:5000/api/Attendances";
            var uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Attendance> events = JsonConvert.DeserializeObject <List<Attendance>>(response);
            if (!events.Any(x => x.UserId == userId && x.EventId.Equals(_eventId)))
            {
                var participation = new Attendance()
                {
                    EventId = _eventId,
                    UserId = userId                 
                };
                var json = JsonConvert.SerializeObject(participation);
                StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                try
                {
                    await client.PostAsync(getUri, theContent);
                    var createdDialog = new MessageDialog("You're confirmated for this event!");
                    await createdDialog.ShowAsync();
                    tblock_Users_Participating.Text = (Convert.ToInt32(tblock_Users_Participating.Text.Substring(0,1))+1).ToString() + " Confirmados";
                }
                catch
                {
                    // ignored
                }
            }        
        }

        private async void b_Delete_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var client = new HttpClient();
            string getUri = "http://localhost:5000/api/Events";
            string getInvitesUri = "http://localhost:5000/api/Invites";
            var uri = new Uri(getUri);
            var invitesUri = new Uri(getInvitesUri);
            var responseInvites = await client.GetStringAsync(invitesUri);
            var response = await client.GetStringAsync(uri);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            List<Invite> listInvites = JsonConvert.DeserializeObject<List<Invite>>(responseInvites);
            var myInvites = listInvites.FindAll(x => x.InvitedId == int.Parse(localSettings.Values["sessionUser"].ToString()));
            string deleteUri = string.Format("http://localhost:5000/api/Events/{0}", localSettings.Values["Event_id"].ToString());
            foreach (var item in myInvites)
            {
                string deleteInviteUri = string.Format("http://localhost:5000/api/Invites/{0}", item.EventId.ToString());
                await client.DeleteAsync(deleteInviteUri);
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
                await client.DeleteAsync(deleteUri);
                var deletesuccessDialog = new MessageDialog(String.Format("Event {0} was successfully deleted!", localSettings.Values["Event_Title"].ToString()));
                await deletesuccessDialog.ShowAsync();
            };
            Frame?.Navigate(typeof(MyEvents));
        }

        private void lvi_invite_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Invites));
        }

        private async void B_invite_OnClick(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var sessionName = int.Parse(localSettings.Values["sessionUser"].ToString());
            var client = new HttpClient();
            var url = "http://localhost:5000/api/Invites";
            var userUrl = "http://localhost:5000/api/Users";
            var uri = new Uri(url);
            var usersUri = new Uri(userUrl);
            var txtBox = new TextBox {Width = 120};
            var ctnDialog = new ContentDialog()
            {
                Title = "Invite a friend",
                PrimaryButtonText = "Cancel",
                SecondaryButtonText = "Invite",
                Content = txtBox
            };
            var displayDialog = await ctnDialog.ShowAsync();
            switch (displayDialog)
            {
                case ContentDialogResult.Primary:
                    break;
                case ContentDialogResult.Secondary:
                    var ctnDialogContent = (TextBox) ctnDialog.Content;
                    if (ctnDialogContent.Text != null)
                    {
                        var userName = ctnDialogContent.Text;
                        var usersResponse = await client.GetStringAsync(usersUri);
                        var users = JsonConvert.DeserializeObject<List<User>>(usersResponse);
                        var invitedUser = users.SingleOrDefault(i => i.Username == userName);
                        if (invitedUser != null)
                        {
                            var invite = new Invite
                            {
                                InviterId = sessionName,
                                EventId = _eventId,
                                InvitedId = invitedUser.Id
                            };
                            var json = JsonConvert.SerializeObject(invite);
                            StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            await client.PostAsync(uri, theContent);
                            var createdDialog = new MessageDialog("Event created!");
                            await createdDialog.ShowAsync();
                        }            
                    }
                    break;

            }
                
        }

        private void Lvi_Main_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Main));
        }
    }
}
