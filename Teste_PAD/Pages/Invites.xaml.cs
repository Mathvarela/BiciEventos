using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            ListBoxItem lbs = (sender as ListBox).SelectedItem as ListBoxItem;
            var eventId = Convert.ToInt32(lbs.Tag);

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            HttpClient client = new HttpClient();
            string getUri = "http://localhost:5000/api/Events";
            string getUriAttendances = "http://localhost:5000/api/Attendances";
            Uri uri = new Uri(getUri);
            Uri uriAttendance = new Uri(getUriAttendances);
            var response = await client.GetStringAsync(uri);
            var responseEg = await client.GetStringAsync(uriAttendance);
            List<Event> listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            List<Attendance> invites = JsonConvert.DeserializeObject<List<Attendance>>(response);
            var evento = listEvents.FirstOrDefault(x => x.Id == eventId);
            var usersParticipations = invites.FindAll(x => x.EventId.Equals(eventId));
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
                foreach (Invite item in invites)
                {
                    var evento = listEvents.SingleOrDefault(x => x.Id == item.EventId);
                    ListBoxItem lb = new ListBoxItem
                    {
                        Tag = item.EventId,
                        Content = "Invitación de " + evento.User.Username + " - " + evento.Title
                    };
                    lb_Events.Items.Add(lb);
                }
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
