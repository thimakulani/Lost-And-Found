using Lost_And_Found.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lost_And_Found.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
             
            await PopupNavigation.Instance.PushAsync(new ForgotPasswordPage());
        }

        private void BtnSignup_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new SignupPage());
        }
    }
}