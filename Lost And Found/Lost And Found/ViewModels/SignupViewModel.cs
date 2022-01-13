using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lost_And_Found.ViewModels
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string name;
        private string lastname;
        private string email;
        private string password;
        private string phone;
       public string Name { get { return name; } set { name = value;PropertyChanged(this, new PropertyChangedEventArgs("Name")); } }
        public string Lastname { get { return lastname; } set { lastname = value;PropertyChanged(this, new PropertyChangedEventArgs("Lastname")); } }
        public string Email { get { return email; } set { email = value;PropertyChanged(this, new PropertyChangedEventArgs("Email")); } }
        public string Password { get { return password; } set { password = value;PropertyChanged(this,new PropertyChangedEventArgs("Password")); } }    
        public string Phone { get { return phone; } set { phone = value;PropertyChanged(this, new PropertyChangedEventArgs("Phone")); } }

        public ICommand OnBtnClick { get; set; }

        public SignupViewModel()
        {
            OnBtnClick = new Command(Signup);
        }

        private void Signup(object obj)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Name", name);
            data.Add("Phone", phone);
            data.Add("Password", password);
            data.Add("Lastname", lastname);
            try
            {
                var auth = Plugin.FirebaseAuth
                    .CrossFirebaseAuth
                    .Current
                    .Instance
                    .SignInWithEmailAndPasswordAsync(Email, Password);



                CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("PEOPLE")
                    .Document(auth.Result.User.Uid)
                    .SetAsync(data);
            }
            catch (Plugin.FirebaseAuth.FirebaseAuthException ex )
            {
                App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }

        }
    }
}
