using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lost_And_Found.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (InputEmail.Text.Trim() != null)
            {
                CrossFirebaseAuth
                    .Current
                    .Instance
                    .SendPasswordResetEmailAsync(InputEmail.Text.Trim());
                DisplayAlert("Successful", "Please Check Your Email Inbox", "Got it");
            }
        }
    }
}