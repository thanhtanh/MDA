using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{
    public class ChatItemViewModels
    {

        public UserProfileBasicInfo Sender { get; set; }
        public UserProfileBasicInfo Receiver { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }

    }
}