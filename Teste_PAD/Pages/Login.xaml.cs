using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Teste_PAD.Models;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Teste_PAD.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void b_Login_Click(object sender, RoutedEventArgs e)
        {
            var context = new BiciEventosDbContext();
            var listUser = new List<User>();
            try
            {
                var client = new HttpClient();
                string getUri = "http://localhost:5000/api/Users";
                var uri = new Uri(getUri);
                var response = await client.GetStringAsync(uri);
                listUser = JsonConvert.DeserializeObject<List<User>>(response);
            }
            catch (Exception err)
            {
                listUser = context.Users.ToList();
            }
            var user = listUser.FirstOrDefault(x => x.Username == tb_Username.Text);
            if (pb_Password.Password == user.Password)
            {
                var successDialog = new MessageDialog("Successfully logged in!");
                await successDialog.ShowAsync();
                context.Users.Add(user);
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["sessionUser"] = user.Id;
                Frame?.Navigate(typeof(Main));
            }
            else
            {
                var errorDialog = new MessageDialog("Error! Username and Login don't match!");
                await errorDialog.ShowAsync();
            }
        }

        private void b_Register_Click(object sender, RoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Register));
        }
    }
}
