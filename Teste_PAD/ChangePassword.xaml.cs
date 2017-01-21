using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Teste_PAD
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangePassword : Page
    {
        public ChangePassword()
        {
            this.InitializeComponent();
        }

        private async void b_change_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["sessionUser"];
            var client = new HttpClient();
            string getUri = "http://localhost:50859/api/Users";
            var uri = new Uri(getUri);
            var response = await client.GetStringAsync(uri);
            List<User> listUser = JsonConvert.DeserializeObject<List<User>>(response);
            try
            {
                var user = listUser.FirstOrDefault(x => x.Username == value.ToString());
                if (pb_Password.Password == user.Password)
                {
                    if (pb_NewPassword.Password == pb_NewPassword_repeat.Password)
                    {
                        string getUserUri = string.Format("http://localhost:50859/api/Users/{0}", user.Id);
                        var userUri = new Uri(getUserUri);
                        var editUser = new User
                        {
                            Id = user.Id,
                            Username = user.Username,
                            Password = pb_NewPassword.Password,
                            Register_date = user.Register_date
                        };
                        DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(User));
                        MemoryStream ms = new MemoryStream();
                        jsonSer.WriteObject(ms, editUser);
                        ms.Position = 0;
                        StreamReader sr = new StreamReader(ms);
                        StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                        var put_response = await client.PutAsync(userUri, theContent);
                        var dialog = new MessageDialog(put_response.ToString());
                        await dialog.ShowAsync();
                        var successDialog = new MessageDialog("Password sucessfully changed!");
                        await successDialog.ShowAsync();
                        if (this.Frame != null)
                        {
                            this.Frame.Navigate(typeof(Main));
                        }
                    }
                }
                else
                {
                    var errorDialog = new MessageDialog("Error! Password doesn't match with your actual password!");
                    await errorDialog.ShowAsync();
                }
            }
            catch
            {
                var errorDialog = new MessageDialog("Error! Password doesn't match with your actual password!");
                await errorDialog.ShowAsync();
            }
        }

        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            Frame?.Navigate(typeof(Main));
        }
    }
}
