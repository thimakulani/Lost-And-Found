using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lost_And_Found.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string email;
        private string password;
        public string Email 
        {
            get
            { 
                return email; 
            } 
            set 
            { 
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            } 
        }
        public string Password 
        {
            get 
            { 
                return password; 
            } 
            set 
            { 
                password = value; 
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SubmitLoginCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitLoginCommand = new Command(Login);
        }
        public async void Login()
        {
            if(Email == null)
            {
                await Application.Current.MainPage.DisplayAlert("Waring", "Email required", "OK");
                return;
            }
            if(Password == null)
            {
                await Application.Current.MainPage.DisplayAlert("Waring", "Email required", "OK");
                return;
            }
            try
            {
                var auth = CrossFirebaseAuth.Current.Instance;
                var results = await auth.SignInWithEmailAndPasswordAsync(Email, Password);
                if(results.User != null)
                {
                    Application.Current.MainPage = new AppShell();
                }
            }
            catch (FirebaseAuthException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
