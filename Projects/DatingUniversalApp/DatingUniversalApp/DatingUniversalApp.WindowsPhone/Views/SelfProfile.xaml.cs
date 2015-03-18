using DatingUniversalApp.Common;
using DatingUniversalApp.Models.Business;
using DatingUniversalApp.Models.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MobileDatingAPI.Models.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DatingUniversalApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelfProfile : Page
    {

        private SelfProfilePageBusiness business;

        public SelfProfile()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.business = new SelfProfilePageBusiness();
            this.RefreshInfo();
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

        private async void RefreshInfo()
        {
            var notifStatus = await Utils.ShowNotificationStatus("Đang lấy dữ liệu...");

            this.ContentRoot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            var infoResult = await this.business.GetPlainInfoLists();

            if (infoResult.Succeeded)
            {
                this.RefreshPlainInfos();
            }
            else
            {
                await Utils.ShowMessageDialog("lấy thông tin không thành công", infoResult.Error, "quay lại");
                this.Frame.GoBack();
            }
            
            var profileResult = await this.business.GetSelfProfile();

            if (profileResult.Succeeded)
            {
                this.RefreshProfile();
            }
            else
            {
                await Utils.ShowMessageDialog("lấy thông tin không thành công", profileResult.Error, "quay lại");
                this.Frame.GoBack();
            }
            
            this.ContentRoot.Visibility = Windows.UI.Xaml.Visibility.Visible;

            await notifStatus.HideAsync();
        }

        private void RefreshPlainInfos()
        {
            {
                this.cboGender.Items.Clear();

                foreach (var gender in this.business.PlainInfoLists.InfoLists[PlainInfoCategories.Gender])
                {
                    this.cboGender.Items.Add(gender);
                }
            }

            {
                this.cboReligiousView.Items.Clear();

                foreach (var religiousView in this.business.PlainInfoLists.InfoLists[PlainInfoCategories.ReligiousView])
                {
                    this.cboReligiousView.Items.Add(religiousView);
                }
            }

            {
                this.cboRelationship.Items.Clear();

                foreach (var relationship in this.business.PlainInfoLists.InfoLists[PlainInfoCategories.Relationship])
                {
                    this.cboRelationship.Items.Add(relationship);
                }
            }

        }

        private void RefreshProfile()
        {
            var model = this.business.ProfileModel;

            this.txtUsername.Text = model.Username ?? "";
            this.txtFullname.Text = model.Fullname ?? "";

            if (model.Birthday.HasValue)
            {
                this.txtBirthday.Date = model.Birthday.Value;
            }

            this.txtMobilePhone.Text = model.MobilePhone ?? "";
            this.txtAbout.Text = model.About ?? "";
            this.txtSchool.Text = model.School ?? "";
            this.chkGraduated.IsChecked = model.Graduated;
            this.txtWork.Text = model.Work ?? "";
            this.txtWorkplace.Text = model.WorkPlace ?? "";

            this.SelectPlainInfoComboValue(model.GenderID, this.cboGender);
            this.SelectPlainInfoComboValue(model.ReligiousViewID, this.cboReligiousView);
            this.SelectPlainInfoComboValue(model.RelationshipID, this.cboRelationship);
        }

        private void SelectPlainInfoComboValue(int? id, ComboBox cbo)
        {
            if (id == null) {
                cbo.SelectedIndex = -1;
                return;
            }

            for (int i = 0; i < cbo.Items.Count; i++)
            {
                if ((cbo.Items[i] as PlainInfo).ID == id)
                {
                    cbo.SelectedIndex = i;
                    break;
                }
            }
        }

        private UserProfileDetailsViewModels GenerateViewModel()
        {
            UserProfileDetailsViewModels model = new UserProfileDetailsViewModels()
            {
                Username = this.txtUsername.Text,
                Fullname = this.txtFullname.Text,
                Birthday = this.txtBirthday.Date.Date,
                MobilePhone = this.txtMobilePhone.Text,
                About = this.txtAbout.Text,
                School = this.txtSchool.Text,
                Graduated = this.chkGraduated.IsChecked,
                Work = this.txtWork.Text,
                WorkPlace = this.txtWorkplace.Text,

                GenderID = this.GetSelectedID(this.cboGender),
                ReligiousViewID = this.GetSelectedID(this.cboReligiousView),
                RelationshipID = this.GetSelectedID(this.cboRelationship),
            };

            return model;
        }

        private int? GetSelectedID(ComboBox cbo)
        {
            if (cbo.SelectedIndex == -1)
            {
                return null;
            }
            else
            {
                return (cbo.SelectedItem as PlainInfo).ID;
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var model = this.GenerateViewModel();

            var result = await this.business.UpdateSelfProfile(model);

            if (result.Succeeded)
            {
                await Utils.ShowMessageDialog("cập nhật thành công", "thông tin cá nhân của bạn đã được cập nhật", "xem");
            }
            else
            {
                await Utils.ShowMessageDialog("cập nhật thất bại", result.Error, "xem");
            }
        }

    }
}
