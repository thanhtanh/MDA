using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

namespace DatingUniversalApp.Models.Utils
{

    public static partial class Utils
    {

        public async static Task<Geoposition> GetCurrentPosition()
        {
            Geolocator geolocator = new Geolocator();
            return await geolocator.GetGeopositionAsync();
        }

        public static async Task ShowMessageDialog(string title, string message, string buttonText)
        {
            MessageDialog diag = new MessageDialog(message, title);

            diag.Commands.Clear();
            diag.Commands.Add(new UICommand(buttonText));

            diag.CancelCommandIndex = 0;

            await diag.ShowAsync();
        }

        public static string FormatTimeDifference(DateTime from)
        {
            return Utils.FormatTimeDifference(DateTime.UtcNow - from.ToUniversalTime());
        }

        public static string FormatTimeDifference(TimeSpan difference)
        {
            if (difference.Days >= 365)
            {
                return (difference.Days / 365) + " năm trước";
            }
            else if (difference.Days >= 30)
            {
                return (difference.Days / 30) + " tháng trước";
            }
            else if (difference.Days >= 1)
            {
                return difference.Days + " ngày trước";
            }
            else if (difference.Hours >= 1)
            {
                return difference.Hours + " giờ trước";
            }
            else if (difference.Minutes >= 2)
            {
                return difference.Minutes + " phút trước";
            }
            else
            {
                return "Mới đây";
            }
        }

    }

}
