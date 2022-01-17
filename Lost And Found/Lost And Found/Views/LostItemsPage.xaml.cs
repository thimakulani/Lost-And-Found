using Lost_And_Found.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lost_And_Found.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LostItemsPage : ContentPage
    {
        private readonly ObservableCollection<LostItem> _lostItems = new ObservableCollection<LostItem>();
        public ObservableCollection<LostItem> LostItems { get { return _lostItems; } }
        public LostItemsPage()
        {
            InitializeComponent();
            PostedItems.BindingContext = this;
            PostedItems.ItemsSource = LostItems;

            Load();
        }
        private void Load()
        {
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("LOST")
                .WhereEqualsTo("Uid", Plugin.FirebaseAuth.CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .AddSnapshotListener((values, error) =>
                {
                    if (!values.IsEmpty)
                    {
                        foreach (var item in values.DocumentChanges)
                        {
                            var data = new LostItem();
                            switch (item.Type)
                            {
                                case DocumentChangeType.Added:
                                    data = item.Document.ToObject<LostItem>();
                                    if(data.Status != "C")
                                    {
                                        data.IsCurrentUser = true;
                                    }
                                    _lostItems.Add(data);
                                    break;
                                case DocumentChangeType.Modified:
                                    _lostItems[item.OldIndex] = item.Document.ToObject<LostItem>();
                                    break;
                                case DocumentChangeType.Removed:
                                    _lostItems.Remove(item.Document.ToObject<LostItem>());
                                    break;
                            }
                        }
                    }
                });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var item = (Button)sender;
            Console.WriteLine(item.CommandParameter.ToString());
            var id = item.CommandParameter.ToString();
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("LOST")
                .Document(id)
                .UpdateAsync("Status", "C");
            DisplayAlert("Information", "Lost Item Closed", "Got it");
        }
    }
}