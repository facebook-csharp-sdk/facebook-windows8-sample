using Facebook.Scrumptious.Windows8.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Facebook.Scrumptious.Windows8.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LandingPage : Facebook.Scrumptious.Windows8.Common.LayoutAwarePage
    {
        public LandingPage()
        {
            this.InitializeComponent();
            LoadUserInfo();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (FacebookData.SelectedFriends.Count > 0)
            {
                if (FacebookData.SelectedFriends.Count > 1)
                {
                    this.selectFriendsTextBox.Text = String.Format("with {0} and {1} others", FacebookData.SelectedFriends[0].Name, FacebookData.SelectedFriends.Count - 1);
                }
                else
                {
                    this.selectFriendsTextBox.Text = "with " + FacebookData.SelectedFriends[0].Name;
                }
            }
            else
            {
                this.selectFriendsTextBox.Text = "Select Friends";
            }

            if (FacebookData.IsRestaurantSelected)
            {
                this.selectRestaurantTextBox.Text = FacebookData.SelectedRestaurant.Name;
            }

            if (!String.IsNullOrEmpty(FacebookData.SelectedMeal.Name))
            {
                this.selectMealTextBox.Text = FacebookData.SelectedMeal.Name;
            }
        }

        private async void LoadUserInfo()
        {
            FacebookClient _fb = new FacebookClient(App.AccessToken);

            dynamic parameters = new ExpandoObject();
            parameters.access_token = App.AccessToken;
            parameters.fields = "name";

            dynamic result = await _fb.GetTaskAsync("me", parameters);

            string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", App.FacebookId, "large", _fb.AccessToken);

            this.MyImage.Source = new BitmapImage(new Uri(profilePictureUrl));
            this.MyName.Text = result.name;
        }

        private void selectMealTextBox_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MealSelector));
        }

        async private void selectRestaurantTextBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Geolocator _geolocator = new Geolocator();
            CancellationTokenSource _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            // Carry out the operation
            Geoposition pos = null;

            // default location is somewhere in redmond, WA
            double latitude = 47.627903;
            double longitude =  -122.143185;
            try
            {
                // We will wait 100 milliseconds and accept locations up to 48 hours old before we give up
                pos = await _geolocator.GetGeopositionAsync(new TimeSpan(48,0,0), new TimeSpan(0,0,0,0,100)).AsTask(token);
            }
            catch (Exception )
            {
                // this API can timeout, so no point breaking the code flow. Use
                // default latitutde and longitude and continue on.
            }

            if (pos != null)
            {
                latitude = pos.Coordinate.Latitude;
                longitude = pos.Coordinate.Longitude;
            }

            FacebookClient fb = new FacebookClient(App.AccessToken);
            dynamic restaurantsTaskResult = await fb.GetTaskAsync("/search", new { q = "restaurant", type = "place", center = latitude.ToString() + "," + longitude.ToString(), distance = "1000" });

            var result = (IDictionary<string, object>)restaurantsTaskResult;

            var data = (IEnumerable<object>)result["data"];

            foreach (var item in data)
            {
                var restaurant = (IDictionary<string, object>)item;

                var location = (IDictionary<string, object>)restaurant["location"];
                FacebookData.Locations.Add(new Location
                {
                    // the address is one level deeper within the object
                    Street = location.ContainsKey("street") ? (string)location["street"] : String.Empty,
                    City = location.ContainsKey("city") ? (string)location["city"] : String.Empty,
                    State = location.ContainsKey("state") ? (string)location["state"] : String.Empty,
                    Country = location.ContainsKey("country") ? (string)location["country"] : String.Empty,
                    Zip = location.ContainsKey("zip") ? (string)location["zip"] : String.Empty,
                    Latitude = location.ContainsKey("latitude") ? ((double)location["latitude"]).ToString() : String.Empty,
                    Longitude = location.ContainsKey("longitude") ? ((double)location["longitude"]).ToString() : String.Empty,

                    // these properties are at the top level in the object
                    Category = restaurant.ContainsKey("category") ? (string)restaurant["category"] : String.Empty,
                    Name = restaurant.ContainsKey("name") ? (string)restaurant["name"] : String.Empty,
                    Id = restaurant.ContainsKey("id") ? (string)restaurant["id"] : String.Empty,
                    PictureUri = new Uri(string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", (string)restaurant["id"], "square", App.AccessToken))
                });
            }

            Frame.Navigate(typeof(Restaurants));
        }

        async private void selectFriendsTextBox_Tapped(object sender, TappedRoutedEventArgs evtArgs)
        {
            FacebookClient fb = new FacebookClient(App.AccessToken);

            dynamic friendsTaskResult = await fb.GetTaskAsync("/me/friends");
            var result = (IDictionary<string, object>)friendsTaskResult;
            var data = (IEnumerable<object>)result["data"];
            foreach (var item in data)
            {
                var friend = (IDictionary<string, object>)item;

                FacebookData.Friends.Add(new Friend { Name = (string)friend["name"], id = (string)friend["id"], PictureUri = new Uri(string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", (string)friend["id"], "square", App.AccessToken)) });
            }

            Frame.Navigate(typeof(FriendSelector));
        }

        async private void PostButtonAppbar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (FacebookData.SelectedFriends.Count < 1
               || FacebookData.SelectedMeal.Name == String.Empty
               || FacebookData.IsRestaurantSelected == false)
            {
                MessageDialog errorMessageDialog = new MessageDialog("Please select friends, a place to eat and something you ate before attempting to share!");
                await errorMessageDialog.ShowAsync();
                return;
            }

            FacebookClient fb = new FacebookClient(App.AccessToken);

            try
            {
                dynamic fbPostTaskResult = await fb.PostTaskAsync(String.Format("/me/{0}:eat", Constants.FacebookAppGraphAction), new { meal = FacebookData.SelectedMeal.MealUri, tags = FacebookData.SelectedFriends[0].id, place = FacebookData.SelectedRestaurant.Id });
                var result = (IDictionary<string, object>)fbPostTaskResult;

                MessageDialog successMessageDialog = new MessageDialog("Posted Open Graph Action, id: " + (string)result["id"]);
                await successMessageDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                MessageDialog exceptionMessageDialog = new MessageDialog("Exception during post: " + ex.Message);
                exceptionMessageDialog.ShowAsync();
            }
        }
    }
}
