using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.UI.Popups;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Teste_PAD
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
            var client = new HttpClient();
            string getUri = "http://localhost:50859/api/Users";
            var uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<User> listUser = JsonConvert.DeserializeObject<List<User>>(response);
                var user = listUser.FirstOrDefault(x => x.Username == tb_Username.Text);
                if (pb_Password.Password == user.Password)
                {
                    var successDialog = new MessageDialog("Successfully logged in!");
                    await successDialog.ShowAsync();
                    Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    localSettings.Values["sessionUser"] = tb_Username.Text;
                    if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(Main));
                    }   
                }
                else
            {
                var errorDialog = new MessageDialog("Error! Username and Login don't match!");
                await errorDialog.ShowAsync();
            }
        }

        private void b_Register_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame!= null)
            {
                this.Frame.Navigate(typeof(Register));
            }
        }
    }
}
