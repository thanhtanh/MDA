using MobileDatingAPI.Models.ViewModels;
using DatingUniversalApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingUniversalApp.Models.Business
{

    public class FriendListPageBusiness
    {

        public FriendListPage View { get; private set; }

        public FriendListPageBusiness(FriendListPage view)
        {
            this.View = view;
        }

        public async Task<FriendListViewModels> GetFriendList()
        {
            using (var connector = new ApiConnector())
            {
                var result = await connector.Request<FriendListViewModels>(
                    ApiConnector.ServerServiceTypes.CommunityService,
                    ApiConnector.ServerCommunityServicePath.FriendList,
                    null);

                return result;
            }
        }

    }

}
