using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SecureStorageDemo
{
    /// <summary>
    /// Description: This is a code demo for Xamarin Essential: Secure Storage
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public string token;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SubmitBtn_Clicked(object sender, EventArgs e)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(UsernameEntry.Text) || UsernameEntry.Text.Length <= 7)
            {
                await DisplayAlert("Error", "INVALID username input/too short!", "Ok");
                isValid = false;
            }
            else if(string.IsNullOrEmpty(PasswordEntry.Text) || PasswordEntry.Text.Length < 5)
            {
                await DisplayAlert("Error", "INVALID password input/too short.", "Ok");
                isValid = false;
            }
            if(isValid)
            {
                try
                {
                    await SecureStorage.SetAsync(token, PasswordEntry.Text);
                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                    await DisplayAlert("ERROR!!!", $"Error message(s): {ex.Message}", "Ok");
                }
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            token = await DisplayActionSheet("Select your token:", "Cancel", null, "loginToken", "secondToken");
            if (token == null)
                token = "loginToken";
            try
            {
                var password = await SecureStorage.GetAsync(token);
                PasswordEntry.Text = password;
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR!!!", $"Error message(s): {ex.Message}", "Ok");
            }
        }

        private async void DeleteBtn_Clicked(object sender, EventArgs e)
        {
            var respond = await DisplayActionSheet("Are you sure?", "No", "Yes");
            
            if (respond == "Yes")
                SecureStorage.Remove(token);
        }

        private async void DeleteAllBtn_Clicked(object sender, EventArgs e)
        {
            var respond = await DisplayActionSheet("Are you sure?", "No", "Yes");
            if (respond == "Yes")
                SecureStorage.RemoveAll();
        }
    }
}
