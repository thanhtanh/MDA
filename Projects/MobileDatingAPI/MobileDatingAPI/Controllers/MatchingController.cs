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
    public class MatchingController : Controller
    {

        private const double DefaultMaximumDistance = 10000;

        private MatchingBusiness matchingBusiness = new MatchingBusiness();

        [HttpPost]
        public ActionResult ListSurroundingUsers(string token, double? latitude, double? longitude, double? maximumDistance)
        {
            if (longitude == null || latitude == null)
            {
                return this.ReturnBadRequest();
            }

            if (maximumDistance == null)
            {
                maximumDistance = DefaultMaximumDistance;
            }

            var result = new UserMatchingInfoViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.matchingBusiness.FindNearbyUser(user, latitude.Value, longitude.Value, maximumDistance.Value, result);

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult SearchUser(string token, string keyword)
        {
            var result = new UserMatchingInfoViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.matchingBusiness.SearchUserByKeyword(keyword, result);

            return this.Json(result);
        }

        public ActionResult FilterUser(string token, UserSearchingViewModels conditions)
        {
            var result = new UserMatchingInfoViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.matchingBusiness.SearchUserByConditions(conditions, result);

            return this.Json(result);
        }

    }
}