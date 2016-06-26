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
    public sealed partial class Invites : Page
    {
        public Invites()
        {
            this.InitializeComponent();
        }


        private void lvi_myEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(myEvents));
            }
        }

        private void lvi_Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Index));
            }
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

        private async void lb_Invites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var eventId = 61;
            ListBoxItem lbs = (sender as ListBox).SelectedItem as ListBoxItem;
            eventId = Convert.ToInt32(lbs.Tag);

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:50859/api/Events";
            string getUriEG = "http://localhost:50859/api/Event_Going";
            Uri uri = new Uri(getUri);
            Uri uriEG = new Uri(getUriEG);
            var response = await client.GetStringAsync(uri);
            var responseEG = await client.GetStringAsync(uriEG);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            List<Event_Going> events = JsonConvert.DeserializeObject<List<Event_Going>>(response);
            var eventos = listEvents.FirstOrDefault(x => x.Id == eventId);
            localSettings.Values["Event_Id"] = eventos.Id;
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
            var usersParticipations = events.FindAll(x => x.EventId.Equals(eventId));
            localSettings.Values["Users_Participating"] = usersParticipations.Count.ToString();
            if (eventos.Username == localSettings.Values["sessionUser"].ToString())
            {
                localSettings.Values["Allowed_to_Edit"] = true;
            }
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Details));
            }
        }

        private async void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:50859/api/Invites";
            Uri uri = new Uri(getUri);
            string getEvento = "http://localhost:50859/api/Events";
            Uri eventUri = new Uri(getEvento);
            var evento_response = await client.GetStringAsync(eventUri);
            var response = await client.GetStringAsync(uri);
            List<Invite> listInvites = JsonConvert.DeserializeObject<List<Invite>>(response);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(evento_response);
            var invites = listInvites.FindAll(x => x.Friend_Username == localSettings.Values["sessionUser"].ToString());
            try
            {
                foreach (Invite item in invites)
                {
                    var evento = listEvents.FirstOrDefault(x => x.Id == item.EventId);
                    ListBoxItem lb = new ListBoxItem();
                    lb.Tag = item.EventId;
                    lb.Content = "Invitación de " + item.Username + " - " + evento.Title;
                    lb_Events.Items.Add(lb);
                }
            }
            catch
            {

            }
            localSettings.Values["Allowed_to_Edit"] = false;
        }

        private void b_Hamburger_Click(object sender, RoutedEventArgs e)
        {
            sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
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
