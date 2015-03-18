using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingUniversalApp.Models.PageViewModels
{
    public class NewStatusDialogViewModels : BasePageViewModels
    {

        private string content;
        public string Content
        {
            get { return content; }
            set { this.SetField(ref content, value, "Content"); }
        }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public StatusUpdateViewModels Result { get; set; }

    }
}
