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

    public class CommunityController : Controller
    {

        private const int StatusDefaultCount = 10;

        private UserProfileBusiness userProfileBusiness = new UserProfileBusiness();

        [HttpPost]
        public ActionResult FriendList(string token)
        {
            var result = new FriendListViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.userProfileBusiness.GetFriendlist(user, result);

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult StatusUpdate(string token, DateTime? from, DateTime? to, int? count)
        {
            var result = new FriendUpdatesViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            if (count == null)
            {
                count = StatusDefaultCount;
            }

            result = this.userProfileBusiness.GetUpdates(user, from, to, count.Value, result);

            return this.Json(result);
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NewStatusUpdate(string token, string content, double? latitude, double? longitude)
        {
            var result = new StatusUpdateViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.userProfileBusiness.CreateNewStatusUpdate(user, content, longitude, latitude, result);

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult UserProfileDetails(string token, string userId)
        {
            var result = new UserProfileDetailsViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.userProfileBusiness.GetProfileDetails(userId, result);

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult UpdateActivity(string token, double? longitude, double? latitude)
        {
            var result = new BaseApiViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.userProfileBusiness.UpdateUserActivity(user, token, longitude, latitude, result);

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult SendFriendRequest(string token, string targetUserId)
        {
            var result = new FriendRequestViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.userProfileBusiness.RequestFriend(user, targetUserId, result);

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult AcceptFriendRequest(string token, int? requestID)
        {
            if (requestID == null)
            {
                return this.ReturnBadRequest();
            }

            var result = new FriendRequestReplyViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.userProfileBusiness.ReplyFriendRequest(user, requestID.Value, true, result);

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult DenyFriendRequest(string token, int? requestID)
        {
            if (requestID == null)
            {
                return this.ReturnBadRequest();
            }

            var result = new FriendRequestReplyViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.userProfileBusiness.ReplyFriendRequest(user, requestID.Value, false, result);

            return this.Json(result);
        }

        public ActionResult Unfriend(string token, string friendID)
        {
            if (friendID == null)
            {
                return this.ReturnBadRequest();
            }

            var result = new BaseApiViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            

            return this.Json(result);
        }

    }
}