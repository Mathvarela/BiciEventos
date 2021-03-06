﻿using Microsoft.EntityFrameworkCore;
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
    public sealed partial class Main : Page
    {
        public Main()
        {
            this.InitializeComponent();
        }

        private void lb_Icons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void b_Hamburger_Click(object sender, RoutedEventArgs e)
        {
            sv_Menu.IsPaneOpen = !sv_Menu.IsPaneOpen;
        }

        private async void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var userId = int.Parse(localSettings.Values["sessionUser"].ToString());
            HttpClient client = new HttpClient();
            var listEvents = new List<Event>();
            var listInvites = new List<Invite>();
            var listAttendances = new List<Attendance>();
            var context = new BiciEventosDbContext();
            try
            {
                var getUri = "http://localhost:5000/api/Events";
                Uri uri = new Uri(getUri);
                var getInvitesUri = "http://localhost:5000/api/Invites";
                var getAttendances = "http://localhost:5000/api/Attendances";
                Uri uriAttendance = new Uri(getAttendances);
                var inviteResponse = await client.GetStringAsync(getInvitesUri);
                var response = await client.GetStringAsync(uri);
                var attendanceResponse = await client.GetStringAsync(uriAttendance);
                listAttendances = JsonConvert.DeserializeObject<List<Attendance>>(attendanceResponse);
                listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
                listInvites = JsonConvert.DeserializeObject<List<Invite>>(inviteResponse);
                context.Events.RemoveRange(context.Events);
                context.SaveChanges();
                context.Users.RemoveRange(context.Users);
                context.SaveChanges();
                context.Attendances.RemoveRange(context.Attendances);
                context.SaveChanges();
                //context.Invites.RemoveRange(context.Invites);
                //context.SaveChanges();
                lb_Events.Items.Clear();
                //var users = context.Users.ToList();
                //var events = context.Events.ToList();
                //var invites = context.Invites.ToList();
                foreach (Event item in listEvents)
                {
                    ListBoxItem lb = new ListBoxItem { Content = item.Title };
                    lb_Events.Items.Add(lb);
                    context.Events.Add(item);
                }
                context.SaveChanges();
                foreach (var atd in listAttendances)
                {
                    context.Attendances.Add(atd);
                }
                context.SaveChanges();
                //foreach (var invite in listInvites)
                //{
                //    context.Invites.Add(invite);
                //}
                //context.SaveChanges();
            }

            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                listAttendances = context.Attendances.ToList();
                listEvents = context.Events.ToList();
                //listInvites = context.Invites.ToList();
                foreach (Event item in listEvents)
                {
                    ListBoxItem lb = new ListBoxItem { Content = item.Title };
                    lb_Events.Items.Add(lb);
                }
            }

            localSettings.Values["Allowed_to_Edit"] = false;
            if (listInvites.Any(i => i.InvitedId == userId && i.IsRead == false))
            {
                var dialog = new MessageDialog("You have unchecked invitations!") {Title = "Invitations"};
                await dialog.ShowAsync();
            }
        }

        private async void lvi_Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["sessionUser"] = null;
            MessageDialog logoutMessage = new MessageDialog("Logout success");
            await logoutMessage.ShowAsync();
            Frame?.Navigate(typeof(MainPage));
        }

        private async void lb_Events_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            List<Event> listEvents;
            List<Attendance> listAttendances = new List<Attendance>();
            var context = new BiciEventosDbContext();
            //try
            //{
            //    var client = new HttpClient();
            //    string getUri = "http://localhost:5000/api/Events";
            //    string getUriEG = "http://localhost:5000/api/Attendances";
            //    var uri = new Uri(getUri);
            //    var uriEg = new Uri(getUriEG);
            //    var response = await client.GetStringAsync(uri);
            //    var atdResponse = await client.GetStringAsync(uriEg);
            //    listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            //    listAttendances = JsonConvert.DeserializeObject<List<Attendance>>(atdResponse);
            //    context.Events.RemoveRange(context.Events);
            //    context.SaveChanges();
            //    context.Attendances.RemoveRange(context.Attendances);
            //    context.SaveChanges();
            //    foreach (var evt in listEvents)
            //    {
            //        context.Events.Add(evt);
            //    }
            //    context.SaveChanges();
            //    foreach (var atd in listAttendances)
            //    {
            //        context.Attendances.Add(atd);
            //    }
            //    context.SaveChanges();
            //}
            //catch (Exception ex)
            //{
                listEvents = context.Events.Include(ev=> ev.User).ToList();
                listAttendances = context.Attendances.ToList();
            //}
            var evento = listEvents.FirstOrDefault(x => x.Title == ((ListBoxItem)lb_Events.SelectedValue).Content.ToString());
            tblock_Title.Text = evento.Title;
            var usersParticipations = listAttendances.FindAll(x => x.EventId.Equals(evento.Id));
            localSettings.Values["Users_Participating"] = usersParticipations.Count.ToString();
            localSettings.Values["start_latitude"] = evento.StartLatitude;
            localSettings.Values["start_longitude"] = evento.StartLongitude;
            localSettings.Values["end_latitude"] = evento.EndLatitude;
            localSettings.Values["end_longitude"] = evento.EndLongitude;
            localSettings.Values["EventId"] = evento.Id;
            var sessionId = int.Parse(localSettings.Values["sessionUser"].ToString());
            if (evento.User.Id == sessionId)
            {
                localSettings.Values["Allowed_to_Edit"] = true;
            }
            Frame?.Navigate(typeof(Details), evento);
        }

        private void lvi_Create_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Index));
        }

        private async void b_Search_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            var query = tb_Search.Text;
            var getUri = "http://localhost:5000/api/Events";
            var uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            var listEvents = JsonConvert.DeserializeObject<List<Event>>(response);
            var events = listEvents.FindAll(x => x.Title.Contains(query));
            lb_Events.Items.Clear();
            foreach (Event item in events)
            {
                var lb = new ListBoxItem {Content = item.Title};
                lb_Events.Items.Add(lb);
            }
        }
        private void lvi_myEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(MyEvents));
        }

        private void lvi_invite_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Invites));
        }

        private void lvi_Change_Password_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame?.Navigate(typeof(ChangePassword));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Index));
        }
    }
}
