using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Principal;
using Microsoft.Owin.Security;

namespace MobileDatingAPI.Models
{
    public static class AuthenUtils
    {

        public static IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public static ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        public static ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public static ApplicationUser GetUser(TempDataDictionary pTempData, IIdentity pIdentity)
        {
            ApplicationUser user = pTempData["Account"] as ApplicationUser;

            if (user == null && pIdentity.IsAuthenticated)
            {
                user = UserManager.FindById(pIdentity.GetUserId());
                pTempData["Account"] = user;
            }

            return user;
        }

        public static AspNetUser GetUserAspNet(TempDataDictionary pTempData, IIdentity pIdentity, MobileDatingApiEntities dc = null)
        {
            return GetUser(pTempData, pIdentity).ToAspNetUser(dc);
        }

        public static AspNetUser ToAspNetUser(this ApplicationUser pUser, MobileDatingApiEntities dc = null)
        {
            if (dc == null)
            {
                dc = new MobileDatingApiEntities();
            }

            return dc.AspNetUsers.Where(q => q.Id.Equals(pUser.Id)).FirstOrDefault();
        }

    }
}