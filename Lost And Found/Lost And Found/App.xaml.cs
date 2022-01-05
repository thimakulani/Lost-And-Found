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

            MainPage = new LoginPage();
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
