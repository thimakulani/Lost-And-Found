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
            //this.BindingContext = new LoginViewModel();
        }
    }
}