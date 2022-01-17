using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Plugin.Media;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class AddItemDialog
    {
        private Plugin.Media.Abstractions.MediaFile upload_file;

        public AddItemDialog()
        {
            InitializeComponent();
        }

        private async void ImgClose_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void BtnChoseImage_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            upload_file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });
            
            
        }
        private async void Upload(IDocumentReference query)
        {
            var storage_ref = Plugin.FirebaseStorage.CrossFirebaseStorage
                     .Current
                     .Instance
                     .RootReference
                     .Child(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid);

            await storage_ref.PutStreamAsync(upload_file.GetStream());

            var url = await storage_ref.GetDownloadUrlAsync();
            await query.UpdateAsync("ImageUrl", url.ToString());


        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine(type);
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "TimePosted", FieldValue.ServerTimestamp },
                { "Description", Description.Text },
                { "LastSeen", SelectedDates.Date.ToString("dd/MMM/yyyy") },
                { "ImageUrl", null },
                { "Category", type },
                { "Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid },
                { "Status", "0" }
            };
            var query  = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("LOST")
                .AddAsync(data);
            Upload(query);
        }
        string type = "Missing Person";
        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var rb = (RadioButton)sender;
            type = rb.Content.ToString();
        }
    }
}