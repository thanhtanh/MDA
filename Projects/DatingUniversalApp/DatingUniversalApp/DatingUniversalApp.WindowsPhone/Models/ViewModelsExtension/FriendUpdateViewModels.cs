using DatingUniversalApp.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileDatingAPI.Models.ViewModels
{
    public partial class StatusUpdateViewModels
    {

        public string FormattedCreationTime
        {
            get
            {
                return Utils.FormatTimeDifference(this.CreationTime);
            }
        }

    }
}
