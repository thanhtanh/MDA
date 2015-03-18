using DatingUniversalApp.Common;
using DatingUniversalApp.Models.Business;
using DatingUniversalApp.Models.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DatingUniversalApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInPage : Page
    {

        private LogInPageBusiness pageBusiness;

        public LogInPage()
        {
            this.InitializeComponent();

            this.pageBusiness = new LogInPageBusiness(this);
            this.BindModels();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.txtUsername.Text = "admin";
            this.txtPassword.Password = "123456";
        }


        private void BindModels()
        {
            this.txtUsername.DataContext =
                this.txtPassword.DataContext =
                this.pageBusiness.ViewModels;

            this.txtUsername.SetBinding(TextBox.TextProperty, new Binding() { Path = new PropertyPath("Username"), Mode = BindingMode.TwoWay, });
            this.txtPassword.SetBinding(PasswordBox.PasswordProperty, new Binding() { Path = new PropertyPath("Password"), Mode = BindingMode.TwoWay, });
        }

        #region NavigationHelper registration

        private NavigationHelper navigationHelper;

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }


        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }


        #endregion

        private async void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeUserInputState(false);

            var notifStatus = await Utils.ShowNotificationStatus("Đang đăng nhập...");

            var result = await this.pageBusiness.ProcessLogIn();

            await notifStatus.HideAsync();

            if (result.Succeeded)
            {
                this.Frame.Navigate(typeof(DashboardPage));
            }
            else
            {
                this.lblStatus.Text = result.Error;
                this.lblStatus.Visibility = Windows.UI.Xaml.Visibility.Visible;

                this.ChangeUserInputState(true);
            }
        }

        private void ChangeUserInputState(bool enabled)
        {
            if (enabled)
            {
                this.pgbSignIn.IsIndeterminate = false;
                this.pgbSignIn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                this.txtUsername.IsEnabled = true;
                this.txtPassword.IsEnabled = true;
                this.btnForgetPassword.IsEnabled = true;
                this.btnRegister.IsEnabled = true;
                this.btnLogIn.IsEnabled = true;

                this.btnLogIn.Focus(FocusState.Programmatic);
            }
            else
            {
                this.lblStatus.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                this.txtUsername.IsEnabled = false;
                this.txtPassword.IsEnabled = false;
                this.btnForgetPassword.IsEnabled = false;
                this.btnRegister.IsEnabled = false;
                this.btnLogIn.IsEnabled = false;

                this.pgbSignIn.IsIndeterminate = true;
                this.pgbSignIn.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }

        private void btnForgetPassword_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ForgotPasswordPage));
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }

    }
}
