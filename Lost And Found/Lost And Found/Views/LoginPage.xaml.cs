using Lost_And_Found.ViewModels;
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

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            
        }

        private void BtnSignup_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new SignupPage());
        }
    }
}