using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
            Frame?.Navigate(typeof(myEvents));
        }

        private void lvi_Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Index));
        }

        private async void lvi_Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["sessionUser"] = null;
            MessageDialog logoutMessage = new MessageDialog("Logout success");
            await logoutMessage.ShowAsync();
            Frame?.Navigate(typeof(MainPage));
        }

        private async void lb_Invites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbs = (sender as ListBox).SelectedItem as ListBoxItem;
            var eventId = Convert.ToInt32(lbs.Tag);

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:50859/api/Events";
            string getUriEG = "http://localhost:50859/api/Event_Going";
            Uri uri = new Uri(getUri);
            Uri uriEG = new Uri(getUriEG);
            var response = await client.GetStringAsync(uri);
            var responseEG = await client.GetStringAsync(uriEG);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            List<Event_Going> invites = JsonConvert.DeserializeObject<List<Event_Going>>(response);
            var evento = listEvents.FirstOrDefault(x => x.Id == eventId);
            var usersParticipations = invites.FindAll(x => x.EventId.Equals(eventId));
            localSettings.Values["Users_Participating"] = usersParticipations.Count.ToString();
            if (evento.Username == localSettings.Values["sessionUser"].ToString())
            {
                localSettings.Values["Allowed_to_Edit"] = true;
            }
            Frame?.Navigate(typeof(Details), evento);
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
                    ListBoxItem lb = new ListBoxItem
                    {
                        Tag = item.EventId,
                        Content = "Invitación de " + item.Username + " - " + evento.Title
                    };
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
            Frame?.Navigate(typeof(Invites));
        }

        private void Lvi_Main_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Main));
        }
    }
}
