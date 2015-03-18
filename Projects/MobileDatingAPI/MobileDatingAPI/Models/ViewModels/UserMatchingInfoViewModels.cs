using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{

    public class UserMatchingInfoViewModels : BaseApiViewModels
    {

        public List<UserProfileBasicInfo> MatchingUsers { get; set; }

        public UserMatchingInfoViewModels()
        {
            this.MatchingUsers = new List<UserProfileBasicInfo>();
        }

    }

}