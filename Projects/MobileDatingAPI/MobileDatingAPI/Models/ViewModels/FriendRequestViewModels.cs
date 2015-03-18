using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models.ViewModels
{

    public class FriendRequestViewModels : BaseApiViewModels
    {

        public int ID { get; set; }
        public DateTime CreationTime { get; set; }
        public UserProfileBasicInfo TargetUser { get; set; }

        public bool Exist { get; set; }

    }

}