using Lost_And_Found.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lost_And_Found.ViewModels
{
    public class LostFoundItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string description { get; set; }
        private string lastSeen { get; set; }
        private string category { get; set; }
        public string Description { get { return description; } set { description = value; PropertyChanged(this, new PropertyChangedEventArgs("Description")); } }
        public string LastSeen { get { return lastSeen; } set { lastSeen = value; PropertyChanged(this, new PropertyChangedEventArgs("LastSeen")); } }
        public string Category { get { return category; } set { category = value; PropertyChanged(this, new PropertyChangedEventArgs("Category"));} }


        public ICommand OnAddCommand;
        public LostFoundItemViewModel()
        {
            OnAddCommand = new Command(OnAddItem);
        }
        private async void OnAddItem()
        {
            if (Description != null)
            {
                return;
            }
            if(LastSeen != null)
            {
                return;
            }
            if(Category != null)
            {
                return;
            }
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Description", description);
            data.Add("LastSeen", lastSeen);
            data.Add("Category", category);
            data.Add("TimePosted", FieldValue.ServerTimestamp);

            await CrossCloudFirestore
                .Current
                .Instance
                .Collection("LOST")
                .AddAsync(data);
                
        }


    }
}
