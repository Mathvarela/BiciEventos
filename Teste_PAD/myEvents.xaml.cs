using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class myEvents : Page
    {
        public myEvents()
        {
            this.InitializeComponent();
        }

        private async void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["sessionUser"];

            HttpClient client = new HttpClient();
            string getUri = "http://localhost:50859/api/Events";
            Uri uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            List<Event> myEvents = listEvents.FindAll(x => x.Username == value.ToString());
            foreach (Event item in myEvents)
            {
                ListBoxItem lb = new ListBoxItem();
                lb.Content = item.Title;
                lb_Events.Items.Add(lb);
            }
        }
        private void lvi_Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Index));
            }
        }
        private async void lb_Events_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:50859/api/Events";
            Uri uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var eventos = listEvents.FirstOrDefault(x => x.Title == ((ListBoxItem)lb_Events.SelectedValue).Content.ToString());
            tblock_Title.Text = eventos.Title;
            localSettings.Values["Event_id"] = eventos.Id;
            localSettings.Values["Event_Title"] = eventos.Title;
            localSettings.Values["Event_Description"] = eventos.Description;
            localSettings.Values["Event_Start_Date"] = eventos.Start_Date.Date.ToString().Substring(0, 10);
            localSettings.Values["Event_Start_Time"] = eventos.Start_Time;
            localSettings.Values["Event_End_Date"] = eventos.End_Date.ToString().Substring(0, 10);
            localSettings.Values["Event_End_Time"] = eventos.End_Time;
            localSettings.Values["Event_startLatitude"] = eventos.start_Latitude.ToString();
            localSettings.Values["Event_startLongitude"] = eventos.start_Longitude.ToString();
            localSettings.Values["Event_endLatitude"] = eventos.end_Latitude.ToString();
            localSettings.Values["Event_endLongitude"] = eventos.end_Longitude.ToString();
            localSettings.Values["Event_Username"] = eventos.Username;
            if (eventos.Username == localSettings.Values["sessionUser"].ToString())
            {
                localSettings.Values["Allowed_to_Edit"] = true;
            }
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
        private async void b_Search_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["sessionUser"];
            HttpClient client = new HttpClient();
            string query = tb_Search.Text;
            string getUri = "http://localhost:50859/api/Events";
            Uri uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            List<Event> events = listEvents.FindAll(x => x.Title.Contains(query) || x.Username == value.ToString());
            lb_Events.Items.Clear();
            foreach (Event item in events)
            {
                ListBoxItem lb = new ListBoxItem();
                lb.Content = item.Title;
                lb_Events.Items.Add(lb);
            }
        }

        private void lvi_Main_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Main));
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
