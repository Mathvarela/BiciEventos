using System;
using System.Net.Http;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Teste_PAD.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Teste_PAD.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            Frame?.Navigate(typeof(MainPage));
        }

        private async void b_Register_Click(object sender, RoutedEventArgs e)
        {
            if (pb_Password.Password== pb_Password_repeat.Password)
            {
                var client = new HttpClient();
                string postUrl = "http://localhost:5000/api/Users";
                var uri = new Uri(postUrl);
                var user = new User()
                {
                    Username = tb_Username.Text,
                    Password = pb_Password.Password,
                    RegisterDate = DateTime.Now
                };
                var json = JsonConvert.SerializeObject(user);
                StringContent theContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                await client.PostAsync(postUrl, theContent);
                var registeredDialog = new MessageDialog("New user registered!");
                await registeredDialog.ShowAsync();
                Frame?.Navigate(typeof(MainPage));
            }
            else
            {
                var errorDialog = new MessageDialog("Inserted passwords aren't the same!");
                await errorDialog.ShowAsync();
            }
        }
    }
}
