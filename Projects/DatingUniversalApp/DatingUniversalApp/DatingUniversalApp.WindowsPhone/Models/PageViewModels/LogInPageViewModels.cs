using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingUniversalApp.Models.PageViewModels
{

    public class LogInPageViewModels : BasePageViewModels
    {

        private string username;
        public string Username
        {
            get { return username; }
            set { this.SetField(ref username, value, "Username"); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { this.SetField(ref password, value, "Password"); }
        }

    }

}
