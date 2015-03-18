using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{

    public class FriendRequestReplyViewModels : BaseApiViewModels
    {

        public int FriendRequestID { get; set; }
        public bool Accepted { get; set; }
        public UserProfileBasicInfo TargetUser { get; set; }

    }

}