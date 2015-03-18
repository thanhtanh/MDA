using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace DatingUniversalApp.Models.Utils
{
    public static partial class Utils
    {
        public static async Task<StatusBarProgressIndicator> ShowNotificationStatus(string status)
        {
            await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ShowAsync();
            var progInd = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ProgressIndicator;
            progInd.Text = status;
            await progInd.ShowAsync();

            return progInd;
        }

    }
}
