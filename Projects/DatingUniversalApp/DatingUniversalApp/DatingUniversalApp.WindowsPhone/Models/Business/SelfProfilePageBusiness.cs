using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingUniversalApp.Models.Business
{
    public class SelfProfilePageBusiness
    {

        public PlainInfoListViewModels PlainInfoLists { get; set; }
        public UserProfileDetailsViewModels ProfileModel { get; set; }

        public async Task<PlainInfoListViewModels> GetPlainInfoLists()
        {
            using (var connector = new ApiConnector())
            {

                var result = await connector.Request<PlainInfoListViewModels>(
                    ApiConnector.ServerServiceTypes.HomeService,
                    ApiConnector.ServerHomeServicePath.GetPlainInfoLists,
                    null);

                if (result.Succeeded)
                {
                    this.PlainInfoLists = result;
                }

                return result;
            }
        }

        public async Task<UserProfileDetailsViewModels> GetSelfProfile()
        {
            using (var connector = new ApiConnector())
            {
                var result = await connector.Request<UserProfileDetailsViewModels>(
                    ApiConnector.ServerServiceTypes.ProfileService,
                    ApiConnector.ServerProfileServicePath.GetFullProfileInfo,
                    null);

                if (result.Succeeded)
                {
                    this.ProfileModel = result;
                }

                return result;
            }
        }

        public async Task<UserProfileDetailsViewModels> UpdateSelfProfile(UserProfileDetailsViewModels model)
        {
            using (var connector = new ApiConnector())
            {
                var result = await connector.Request<UserProfileDetailsViewModels>(
                    ApiConnector.ServerServiceTypes.ProfileService,
                    ApiConnector.ServerProfileServicePath.UpdateProfile,
                    model);

                if (result.Succeeded)
                {
                    this.ProfileModel = result;
                }

                return result;
            }
        }

    }

}
