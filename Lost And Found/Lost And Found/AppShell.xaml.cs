using Lost_And_Found.Views;
using Xamarin.Forms;

namespace Lost_And_Found
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }

    }
}
