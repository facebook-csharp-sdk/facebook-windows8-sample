using System;
using Facebook;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace facebook_metro_sample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FacebookInfoPage : Page
    {
        private readonly FacebookClient _fb = new FacebookClient();
        private string _userId;

        public FacebookInfoPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dynamic parameter = e.Parameter;
            _fb.AccessToken = parameter.access_token;
            _userId = parameter.id;

            LoadFacebookData();
        }

        private void LoadFacebookData()
        {
            GetUserProfilePicture();
        }

        private async void GetUserProfilePicture()
        {
            try
            {
                dynamic result = await _fb.GetTaskAsync("me?fields=picture");
                string id = result.id;

                // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                // for more info visit http://developers.facebook.com/docs/reference/api
                string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", _userId, "square", _fb.AccessToken);

                picProfile.Source = new BitmapImage(new Uri(profilePictureUrl));
            }
            catch (FacebookApiException ex)
            {
                // handel error message
            }
        }

        private async void PostToWall_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void DeleteLastMessage_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
