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
        public string Name { get { return name; PropertyChanged(this, new PropertyChangedEventArgs("Name")); } }
        public string Lastname { get { return lastname; PropertyChanged(this, new PropertyChangedEventArgs("Lastname")); } }
        public string Email { get { return email; PropertyChanged(this, new PropertyChangedEventArgs("Email")); } }
        public string Password { get { return password; PropertyChanged(this,new PropertyChangedEventArgs("Password")); } }
        public string Phone { get { return phone; PropertyChanged(this, new PropertyChangedEventArgs("Phone")); } }
        public ICommand OnBtnClick { get; set; }

        public SignupViewModel()
        {
            OnBtnClick = new Command(Signup);
        }

        private void Signup(object obj)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["Name"] = Name;
            data["Password"] = Password;

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("PEOPLE")
                .AddAsync(data);
        }
    }
}
