using Lost_And_Found.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
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
        private readonly ObservableCollection<LostItem> _lostItems = new ObservableCollection<LostItem>();
        public ObservableCollection<LostItem> LoseItems { get { return _lostItems; } }

        public HomePage()
        {

            InitializeComponent();
            //LostFoundItems.BindingContext = this;
            LostFoundItems.ItemsSource = LoseItems;
            LoadList();
            
        }

        private void LoadList()
        {
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("LOST")
                .WhereEqualsTo("Status", "0")
                .AddSnapshotListener( async(values, error) =>
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
                                    data.User = await GetUserAsync(data.Uid);
                                    if(data.Uid == CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                                    {
                                        data.IsCurrentUser = true;
                                    }
                                    _lostItems.Add(data);
                                    break;
                                case DocumentChangeType.Modified:
                                    data = item.Document.ToObject<LostItem>();
                                    data.User = await GetUserAsync(data.Uid);
                                    _lostItems[item.OldIndex] = data;
                                    break;
                                case DocumentChangeType.Removed:
                                   // _lostItems.Remove(item.Document.ToObject<LostItem>());
                                    break;
                            }
                        }
                    }
                });
        }
        private async Task<string> GetUserAsync(string uid)
        {
            try
            {
                var query = await CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("PEOPLE")
                    .Document(uid)
                    .GetAsync();
                if (query != null)
                {
                    var user = query.ToObject<User>();
                    return $"{user.Name} {user.Lastname}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(":::::::::::::::::"+e.Message);
            }
            return null;
        }
                    

        private async void ImgBtnAdd_Clicked(object sender, EventArgs e)
        {
            AddItemDialog page = new AddItemDialog();
            await PopupNavigation.Instance.PushAsync(page);
        }

        private void LostFoundItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = e.CurrentSelection.FirstOrDefault() as LostItem;
            if(data.Uid == CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
            {
                Navigation.PushModalAsync(new DetailItemPage(data));
            }
        }

        private void BtnLogout_Clicked(object sender, EventArgs e)
        {
            CrossFirebaseAuth.Current.Instance
                .SignOut();
                
        }
    }
}