using DatingUniversalApp.Models.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MobileDatingAPI.Models.ViewModels
{
    public partial class UserProfileBasicInfo
    {

        private static readonly Uri IconUri = new Uri("ms-appx:///Assets/online.png");

        public Visibility OnlineVisibility
        {
            get
            {
                return this.Online ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility OfflineTimeVisibility
        {
            get
            {
                return this.Online ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public ImageSource OnlineIcon
        {
            get
            {
                if (this.Online)
                {
                    return new BitmapImage(UserProfileBasicInfo.IconUri);
                }
                else
                {
                    return null;
                }
            }
        }

        public string FormattedLastOnlineTime
        {
            get
            {
                if (this.LastActivityTime.HasValue)
                {
                    return Utils.FormatTimeDifference(this.LastActivityTime.Value);
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
