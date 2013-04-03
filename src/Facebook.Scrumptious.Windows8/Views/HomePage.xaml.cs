using Facebook.Client;
using Facebook.Scrumptious.Windows8.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Facebook.Scrumptious.Windows8.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        string _permissions = "user_about_me,read_stream,publish_stream"; // Set your permissions here

        FacebookClient _fb = new FacebookClient();

        public HomePage()
        {
            this.InitializeComponent();

            //this.Loaded += HomePage_Loaded;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {  
        }

        private FacebookSession session;
        private async Task Authenticate()
        {
            string message = String.Empty;
            try
            {
                session = await App.FacebookSessionClient.LoginAsync("user_about_me,read_stream");
                App.AccessToken = session.AccessToken;
                App.FacebookId = session.FacebookId;
                
                Frame.Navigate(typeof(LandingPage));
            }
            catch (InvalidOperationException e)
            {
                message = "Login failed! Exception details: " + e.Message;
                MessageDialog dialog = new MessageDialog(message);
                dialog.ShowAsync();
            }
        }

        async private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!App.isAuthenticated)
            {
                App.isAuthenticated = true;
                await Authenticate();
            }
        }
    }
}
