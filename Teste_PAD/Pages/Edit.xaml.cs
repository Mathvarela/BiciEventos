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
    public sealed partial class Edit : Page
    {
        int _first = 0;
        Geopoint _startLocation = null;
        Geopoint _endLocation = null;
        private int _eventId;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var evento = (Event)e.Parameter;
            if (evento != null)
            {
                tb_Title.Text = evento.Title;
                cdp_StartDate.Date = evento.StartDate;
                tp_Start_Time.Time = TimeSpan.Parse(evento.StartTime);
                cdp_EndDate.Date = evento.EndDate;
                tp_End_Time.Time = TimeSpan.Parse(evento.EndTime);
                tb_Description.Text = evento.Description;
                _eventId = evento.Id;
                tblock_latitude.Text = evento.StartLatitude.ToString();
                tblock_longitude.Text = evento.StartLongitude.ToString();
            }
            base.OnNavigatedTo(e);
        }

        public Edit()
        {
            this.InitializeComponent();
        }

        private async void b_back_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var eventId = Convert.ToInt32(localSettings.Values["EventId"]);
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:5000/api/Events";
            Uri uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            var evento = listEvents.SingleOrDefault(a => a.Id == eventId);
            Frame?.Navigate(typeof(Details), evento);
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
            Frame?.Navigate(typeof(MainPage));
        }

        private void lvi_Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Index));
        }

        private void lvi_myEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(MyEvents));
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["sessionUser"];
            var client = new HttpClient();
            string getUri = string.Format("http://localhost:5000/api/Events/{0}",localSettings.Values["EventId"].ToString());
            var uri = new Uri(getUri);
            var evento = new Event()
            {
                Id = Convert.ToInt32(localSettings.Values["EventId"]),
                Title = tb_Title.Text,
                Description = tb_Description.Text,
                StartDate = cdp_StartDate.Date.Value.DateTime,
                EndDate = cdp_EndDate.Date.Value.DateTime,
                StartLatitude = Convert.ToDouble(localSettings.Values["Event_startLatitude"]),
                EndLatitude = Convert.ToDouble(localSettings.Values["Event_endLatitude"]),
                StartLongitude = Convert.ToDouble(localSettings.Values["Event_endLatitude"]),
                EndLongitude = Convert.ToDouble(localSettings.Values["Event_endLongitude"]),
                StartTime = tp_Start_Time.Time.ToString(),
                EndTime = tp_End_Time.Time.ToString(),
                UserId = int.Parse(value.ToString())
                //Username = value.ToString()
            };
            var json = JsonConvert.SerializeObject(evento);
            StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            await client.PutAsync(uri, theContent);
            var editDialog = new MessageDialog("Changes are saved!");
            await editDialog.ShowAsync();
            Frame?.Navigate(typeof(Details), evento);
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Your changes won't be saved!");
            dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();
            if ((int)res.Id == 0)
            {
                Frame?.Navigate(typeof(Main));
            }
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

        private async void MapControl_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            int icons = Convert.ToInt32(localSettings.Values["icons"]);
            var tappedGeoPosition = args.Location.Position;
            tblock_latitude.Text = tappedGeoPosition.Latitude.ToString();
            tblock_longitude.Text = tappedGeoPosition.Longitude.ToString();
            if ((icons > 1) && (_first != 0))
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
                _first++;
                if (icons == 1)
                    _startLocation = icon.Location;
                else
                {
                    _endLocation = icon.Location;
                    MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(
                    _startLocation,
                    _endLocation,
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

            }
            localSettings.Values["icons"] = icons;
        }

        private void lvi_invite_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Invites));
        }
    }
}
