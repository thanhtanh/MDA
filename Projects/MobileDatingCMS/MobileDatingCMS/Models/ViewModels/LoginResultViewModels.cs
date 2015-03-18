using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingCMS.Models.ViewModels
{
    public class LoginResultViewModels
    {

        public bool Succeeded { get; set; }
        public string ReturnUrl { get; set; }
        public string Error { get; set; }

    }
}