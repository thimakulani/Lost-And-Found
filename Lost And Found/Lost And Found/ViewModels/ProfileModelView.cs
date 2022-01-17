using Lost_And_Found.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lost_And_Found.ViewModels
{
    public class ProfileModelView: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        public string FirstName { get { return firstName; } set { firstName = value; PropertyChanged(this, new PropertyChangedEventArgs("FirstName")); } }
        public string LastName { get { return lastName; } set { lastName = value; PropertyChanged(this, new PropertyChangedEventArgs("LastName")); } }
        public string Email { get { return email; } set { email = value; PropertyChanged(this,  new PropertyChangedEventArgs("Email")); } }
        public string Phone { get { return phone; } set { phone = value; PropertyChanged(this,  new PropertyChangedEventArgs("Phone")); } }
        public ICommand OnUpdateCommand { get; set; }   
        public ProfileModelView()
        {
            OnUpdateCommand = new Command(Update);
            GetData();
        }
        private void GetData()
        {
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("PEOPLE")
                .Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .AddSnapshotListener((value, error) =>
                {
                    if (value.Exists)
                    {
                        var user = value.ToObject<User>();
                        FirstName = user.Name;
                        LastName = user.Lastname;
                        Email = user.Email;
                        Phone = user.Phone;
                    }
                });
        }

        private void Update()
        {
            if(firstName == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter first name", "Ok");
                return;
            }

            if(lastName == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter last name", "Ok");
                return;
            }

            if(email == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter email ", "Ok");
                return;
            }

            if(phone == null)
            {
                App.Current.MainPage.DisplayAlert("Warning", "Enter phone", "Ok");
                return;
            }
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "Name", firstName },
                { "Phone", phone },
                { "Email", email },
                { "Lastname", lastName }
            };
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("PEOPLE")
                .Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .UpdateAsync(data);
            App.Current.MainPage.DisplayAlert("Confirm", "Your profile has been successfully updated", "Ok");
        }
    }
}
