using Lost_And_Found.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
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
    public partial class DetailItemPage : ContentPage
    {
        readonly LostItem lostItem = new LostItem();
        private string phone = null;
        public DetailItemPage(Models.LostItem data)
        {
            InitializeComponent();
            lostItem = data;
            BindingContext = lostItem;

            CrossCloudFirestore
                .Current
                .Instance
                .Collection("PEOPLE")
                .Document(data.Uid)
                .AddSnapshotListener((value, error) =>
                {
                    if (value.Exists)
                    {
                        User user = value.ToObject<User>();
                        phone = user.Phone;
                    }
                });
            if(lostItem.Status == "C")
            {
                //BtnCallUser.IsVisible = false;
            }
            LoadComments();
        }
        private ObservableCollection<Comments> comments = new ObservableCollection<Comments>();
        public ObservableCollection<Comments> Comments { get { return comments; } } 
        private void LoadComments()
        {
            CommentItems.ItemsSource = Comments;
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("COMMENTS")
                .WhereEqualsTo("Pid", lostItem.Id)
                .OrderBy("TimeStamp", false)
                .AddSnapshotListener(async (value, errors) =>
                {
                    if (value !=null)
                    {
                        foreach (var item in value.DocumentChanges)
                        {
                            switch (item.Type)
                            {
                                case DocumentChangeType.Added:
                                    var data = item.Document.ToObject<Comments>();
                                    data.User = await GetUserAsync(data.Uid);
                                    comments.Add(data);
                                    break;
                                case DocumentChangeType.Modified:
                                    break;
                                case DocumentChangeType.Removed:
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
                Console.WriteLine(":::::::::::::::::" + e.Message);
            }
            return null;
        }

        private void BtnCallUser_Clicked(object sender, EventArgs e)
        {
            if(phone!= null)
            {
                Xamarin.Essentials.PhoneDialer.Open(phone.Trim());
            }
        }

        private void BtnSend_Clicked(object sender, EventArgs e)
        {
            if(InputMessage.Text.Trim() != null)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("Uid", CrossFirebaseAuth.Current.Instance.CurrentUser.Uid);
                data.Add("CommentText", InputMessage.Text);
                data.Add("Pid", lostItem.Id);
                data.Add("TimeStamp", FieldValue.ServerTimestamp);



                CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("COMMENTS")
                    .AddAsync(data);
            }
            InputMessage.Text = string.Empty;
        }
    }
    public class Comments
    {
        public string Uid { get; set; }
        public string CommentText { get; set; }
        public string Id { get; set; }
        public string Pid { get; set; }
        public string User { get; set; }
    }
}