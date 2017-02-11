using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Teste_PAD.Models;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Teste_PAD.Pages
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
            Frame?.Navigate(typeof(MyEvents));
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
            Invite lbs = lb_Events.SelectedItem as Invite;
            var eventId = lbs.EventId;
            var inviterId = lbs.InviterId;
            var invitedId = lbs.InvitedId;
            var invite = new Invite
            {
                EventId = eventId,
                InvitedId = invitedId,
                InviterId = inviterId
            };
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:5000/api/Events";
            string getUriAttendances = "http://localhost:5000/api/Attendances";
            string deleteInvite = "http://localhost:5000/api/Invites";
            Uri uri = new Uri(getUri);
            Uri uriAttendance = new Uri(getUriAttendances);
            Uri uriDeleteInvite = new Uri(deleteInvite);
            var response = await client.GetStringAsync(uri);
            var responseEg = await client.GetStringAsync(uriAttendance);
            var json = JsonConvert.SerializeObject(invite);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Content = content, 
                Method = HttpMethod.Delete,
                RequestUri = uriDeleteInvite
            };
            await client.SendAsync(request);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            List<Attendance> attendances = JsonConvert.DeserializeObject<List<Attendance>>(responseEg);
            
            var evento = listEvents.FirstOrDefault(x => x.Id == eventId);
            var usersParticipations = attendances.FindAll(x => x.EventId.Equals(eventId));
            localSettings.Values["Users_Participating"] = usersParticipations.Count.ToString();
            if (evento.UserId == int.Parse(localSettings.Values["sessionUser"].ToString()))
            {
                localSettings.Values["Allowed_to_Edit"] = true;
            }

            Frame?.Navigate(typeof(Details), evento);
        }

        private async void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:5000/api/Invites";
            Uri uri = new Uri(getUri);
            string getEvento = "http://localhost:5000/api/Events";
            Uri eventUri = new Uri(getEvento);
            var eventoResponse = await client.GetStringAsync(eventUri);
            var response = await client.GetStringAsync(uri);
            List<Invite> listInvites = JsonConvert.DeserializeObject<List<Invite>>(response);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(eventoResponse);
            var invites = listInvites.FindAll(x => x.InvitedId == int.Parse(localSettings.Values["sessionUser"].ToString()));
            try
            {
                lb_Events.ItemsSource = invites;
            }
            catch
            {
                // ignored
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
