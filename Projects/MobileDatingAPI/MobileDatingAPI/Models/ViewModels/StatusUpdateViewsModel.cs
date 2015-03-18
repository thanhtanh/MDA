using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{

    public partial class StatusUpdateViewModels : UserProfileBasicInfo
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude{ get; set; }
    }

}