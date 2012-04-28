using System;
using System.Collections.Generic;
using System.Dynamic;
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

            GraphApiAsyncDynamicExample();
            GraphApiAsyncParametersDictionaryExample();
            GraphApiAsyncParametersExpandoObjectExample();

            FqlAsyncExample();
            FqlMultiQueryAsyncExample();

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

        private async void GraphApiAsyncDynamicExample()
        {
            try
            {
                // instead of casting to IDictionary<string,object> or IList<object>
                // you can also make use of the dynamic keyword.
                dynamic result = await _fb.GetTaskAsync("me");

                // You can either access it this way, using the .
                dynamic id = result.id;
                dynamic name = result.name;

                // if dynamic you don't need to cast explicitly.
                ProfileName.Text = "Hi " + name;

                // or using the indexer
                dynamic firstName = result["first_name"];
                dynamic lastName = result["last_name"];

                // checking if property exist
                var localeExists = result.ContainsKey("locale");

                // you can also cast it to IDictionary<string,object> and then check
                var dictionary = (IDictionary<string, object>)result;
                localeExists = dictionary.ContainsKey("locale");
            }
            catch (FacebookApiException ex)
            {
                // handle error
            }
        }

        private async void GraphApiAsyncParametersDictionaryExample()
        {
            try
            {
                // additional parameters can be passed and 
                // must be assignable from IDictionary<string, object>
                var parameters = new Dictionary<string, object>();
                parameters["fields"] = "first_name,last_name";

                dynamic result = await _fb.GetTaskAsync("me", parameters);

                FirstName.Text = "First Name: " + result.first_name;

            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }
        }

        private async void GraphApiAsyncParametersExpandoObjectExample()
        {
            try
            {
                // additional parameters can be passed and 
                // must be assignable from IDictionary<string, object>
                dynamic parameters = new ExpandoObject();
                parameters.fields = "last_name";

                dynamic result = await _fb.GetTaskAsync("me", parameters);

                LastName.Text = "Last Name: " + result.last_name;
            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }
        }

        private async void FqlAsyncExample()
        {
            try
            {
                // query to get all the friends
                var query = string.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()");

                dynamic parameters = new ExpandoObject();
                parameters.q = query;
                dynamic result = await _fb.GetTaskAsync("fql", parameters);

                TotalFriends.Text = string.Format("You have {0} friend(s).", result.data.Count);
            }
            catch (FacebookApiException ex)
            {
                // handle error message
            }

        }

        private async void FqlMultiQueryAsyncExample()
        {
            try
            {
                var query1 = "SELECT uid FROM user WHERE uid=me()";
                var query2 = "SELECT profile_url FROM user WHERE uid=me()";

                dynamic parameters = new ExpandoObject();
                parameters.q = new { query1, query2 };
                dynamic result = await _fb.GetTaskAsync("fql", parameters);

                dynamic resultForQuery1 = result.data[0].fql_result_set;
                dynamic resultForQuery2 = result.data[1].fql_result_set;

                var uid = resultForQuery1[0].uid;

            }
            catch (FacebookApiException ex)
            {
                // handle error message
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
