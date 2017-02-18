using Newtonsoft.Json;
using System;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Teste_PAD.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Index : Page
    {
        Geopoint _startLocation = null;
        Geopoint _endLocation = null;
        int _first = 0;
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
                Frame?.Navigate(typeof(MainPage));
            }
            else
            {
                sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
            }
        }

        private async void MapControl_MapTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var icons = Convert.ToInt32(localSettings.Values["icons"]);
            var tappedGeoPosition = args.Location.Position;
            if ((icons > 1) && (_first != 0))
            {
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
            var initialPoint = new Geopoint(new BasicGeoposition()
            {
                Latitude = 40.4528057109565,
                Longitude = -3.73339807614684
            });
            var finalPoint = new Geopoint(new BasicGeoposition()
            {
                Latitude = 49.4528057109565,
                Longitude = -10.73339807614684
            });
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["sessionUser"];
            var client = new HttpClient();
            string getUri = "http://localhost:5000/api/Events";
            var uri = new Uri(getUri);
            var objEvento = new Event()
            {
                Title = tb_Title.Text,
                Description = tb_Description.Text,
                StartDate = cdp_StartDate.Date.Value.DateTime,
                EndDate = cdp_EndDate.Date.Value.DateTime,
                StartLatitude = initialPoint.Position.Latitude,
                EndLatitude = finalPoint.Position.Latitude,
                StartLongitude = initialPoint.Position.Longitude,
                EndLongitude = finalPoint.Position.Longitude,
                StartTime = tp_Start_Time.Time.ToString(),
                EndTime = tp_End_Time.Time.ToString(),
                //Username = value.ToString(),
                UserId =  int.Parse(value.ToString())
            };
            var json = JsonConvert.SerializeObject(objEvento);
            StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            await client.PostAsync(getUri.ToString(), theContent);
            var createdDialog = new MessageDialog("Event created!");
            await createdDialog.ShowAsync();
            tblock_Title.Text = objEvento.Title;
            Frame?.Navigate(typeof(Details), objEvento);
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
                Frame?.Navigate(typeof(Main));
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
                Frame?.Navigate(typeof(MyEvents));
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
                Frame?.Navigate(typeof(Invites));
            }
            else
            {
                sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
            }
        }
    }
}
