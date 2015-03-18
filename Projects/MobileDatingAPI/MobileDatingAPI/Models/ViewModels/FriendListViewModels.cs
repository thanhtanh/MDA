using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{
    public partial class FriendListViewModels : BaseApiViewModels
    {

        public List<UserProfileBasicInfo> Friends { get; set; }

        public FriendListViewModels()
        {
            this.Friends = new List<UserProfileBasicInfo>();
        }

    }

}