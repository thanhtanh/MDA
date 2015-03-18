using DatingUniversalApp.Models.PageViewModels;
using DatingUniversalApp.Views;
using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DatingUniversalApp.Models.Business
{
    public class LogInPageBusiness
    {

        public LogInPage View { get; private set; }
        public LogInPageViewModels ViewModels { get; private set; }

        public LogInPageBusiness(LogInPage page)
        {
            this.ViewModels = new LogInPageViewModels();
            this.View = page;

        }


        public async Task<LogInViewModels> ProcessLogIn()
        {
            using (ApiConnector connector = new ApiConnector())
            {
                List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
                param.Add(new KeyValuePair<string, string>("username", this.ViewModels.Username));
                param.Add(new KeyValuePair<string, string>("password", this.ViewModels.Password));

                var response = await connector.Request<LogInViewModels>(
                    ApiConnector.ServerServiceTypes.IdentityService,
                    ApiConnector.ServerIdentityServicePath.LogIn,
                    param);

                if (response.Succeeded)
                {
                    BasePageViewModels.CurrentLoginToken = response.LoginToken;
                    ApiConnector.LoginToken = response.LoginToken;
                }

                return response;
            }

        }

    }
}
