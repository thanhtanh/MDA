using MobileDatingAPI.Models;
using MobileDatingAPI.Models.Business;
using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingAPI.Controllers
{
    public class ProfileController : Controller
    {

        private ProfileBusiness profileBusiness = new ProfileBusiness();

        [HttpPost]
        public ActionResult GetFullProfileInfo(string token, string userId)
        {
            var model = new UserProfileDetailsViewModels();

            var user = this.GetUserFromTokenID(token, model);
            if (user == null) { return this.Json(model); }

            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = user;
            }

            model = this.profileBusiness.GetUserProfileDetails(userId, model);

            return this.Json(model);
        }

        [HttpPost]
        public ActionResult UpdateProfile(string token, UserProfileDetailsViewModels model)
        {
            var user = this.GetUserFromTokenID(token, model);
            if (user == null) { return this.Json(model); }

            model = this.profileBusiness.UpdateUserProfile(user, model);

            return this.Json(model);
        }

        

    }
}