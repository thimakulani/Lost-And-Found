using Lost_And_Found.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lost_And_Found
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Plugin.FirebaseAuth
                .CrossFirebaseAuth
                .Current
                .Instance
                .AuthState += Instance_AuthState;
        }

        private void Instance_AuthState(object sender, Plugin.FirebaseAuth.AuthStateEventArgs e)
        {
            if(e.Auth.CurrentUser == null)
            {
                MainPage = new LoginPage();
            }
            else
            {
                MainPage = new AppShell();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
