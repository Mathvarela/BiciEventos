using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
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
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        public class User
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public DateTime Register_date { get; set; }
        }

        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame!= null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private async void b_Register_Click(object sender, RoutedEventArgs e)
        {
            if (pb_Password.Password== pb_Password_repeat.Password)
            {
                var client = new HttpClient();
                string getUri = "http://localhost:50859/api/Users";
                var uri = new Uri(getUri);
                var user = new User()
                {
                    Username = tb_Username.Text.ToString(),
                    Password = pb_Password.Password,
                    Register_date = DateTime.Now
                };
                DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(User));
                MemoryStream ms = new MemoryStream();
                jsonSer.WriteObject(ms, user);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var post_response = await client.PostAsync(getUri.ToString(), theContent);
                var registeredDialog = new MessageDialog("New user registered!");
                await registeredDialog.ShowAsync();
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
            }
            else
            {
                var errorDialog = new MessageDialog("Inserted passwords aren't the same!");
                await errorDialog.ShowAsync();
            }
        }
    }
}
