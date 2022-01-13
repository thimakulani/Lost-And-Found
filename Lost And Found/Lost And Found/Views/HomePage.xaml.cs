using Lost_And_Found.Models;
using Plugin.CloudFirestore;
using Rg.Plugins.Popup.Services;
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
    public partial class HomePage : ContentPage
    {
        private ObservableCollection<LostItem> _lostItems = new ObservableCollection<LostItem>();
        public ObservableCollection<LostItem> LoseItems { get { return _lostItems; } }

        public HomePage()
        {

            InitializeComponent();
            LostFoundItems.BindingContext = this;
            LostFoundItems.ItemsSource = LoseItems;
            LoadList();
        }

        private void LoadList()
        {
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("LOST")
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

        private async void ImgBtnAdd_Clicked(object sender, EventArgs e)
        {
            AddItemDialog page = new AddItemDialog();
            await PopupNavigation.Instance.PushAsync(page);
        }

        private void LostFoundItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}