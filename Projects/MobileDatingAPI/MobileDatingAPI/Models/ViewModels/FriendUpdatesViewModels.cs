using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{

    public partial class FriendUpdatesViewModels : BaseApiViewModels
    {

        public List<StatusUpdateViewModels> Updates { get; set; }

        public FriendUpdatesViewModels()
        {
            this.Updates = new List<StatusUpdateViewModels>();
        }

    }



}