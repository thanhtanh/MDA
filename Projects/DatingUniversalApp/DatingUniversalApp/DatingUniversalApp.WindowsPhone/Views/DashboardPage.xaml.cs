using DatingUniversalApp.Common;
using DatingUniversalApp.Models.Business;
using DatingUniversalApp.Models.Utils;
using DatingUniversalApp.Views.Dialog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
    public sealed partial class DashboardPage : Page
    {

        private DashboardPageBusiness business;

        public DashboardPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.business = new DashboardPageBusiness(this);
            this.FeedStatus();
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

        private async void FeedStatus()
        {
            var result = await this.business.FeedStatus();

            if (result.Succeeded)
            {
                this.lstStatus.ItemsSource = this.business.Feeds;
            }
            else
            {
                await Utils.ShowMessageDialog("Status feeding failed", result.Error, "Đóng");
            }

            
        }

        private void btnFriendList_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FriendListPage));
        }

        private async void btnAddStatus_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowPostingStatusDialog("");   
        }

        private async Task ShowPostingStatusDialog(string initialContent)
        {
            NewStatusDialog diag = new NewStatusDialog(initialContent);
            var diagResult = await diag.ShowAsync();

            if (diagResult == ContentDialogResult.Secondary)
            {
                return;
            }

            var notifStatus = await Utils.ShowNotificationStatus("Đang cập nhật trạng thái...");

            var postingResult = await this.business.PostStatus(diag);

            await notifStatus.HideAsync();

            if (postingResult.Succeeded)
            {
                await Utils.ShowMessageDialog("cập nhật thành công", "trạng thái mới của bạn đã được đăng", "xem");

                var result = await this.business.FeedNewStatus();
                if (result.Succeeded)
                {
                    this.lstStatus.ItemsSource = this.business.Feeds;
                }
            }
            else
            {
                await Utils.ShowMessageDialog("cập nhật không thành công", postingResult.Error, "thử lại");
                await this.ShowPostingStatusDialog(diag.StatusContent);
                return;
            }
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelfProfile));
        }
    }
}
