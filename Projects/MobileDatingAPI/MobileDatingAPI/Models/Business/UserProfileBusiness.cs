using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models.Business
{
    public class UserProfileBusiness : BaseBusiness
    {

        private const int HistoryRecordKeep = 10;

        public static ActiveUserManager ActiveUserManger;

        static UserProfileBusiness()
        {
            UserProfileBusiness.ActiveUserManger = new ActiveUserManager();
        }

        public FriendListViewModels GetFriendlist(string userID, FriendListViewModels model)
        {
            var friendEntityList = this.ApiEntities.FriendLists
                .Where(q => q.Active && q.UserID.Equals(userID));

            model.Friends = friendEntityList
                .Select(q => new UserProfileBasicInfo()
                {
                    UserID = q.FriendUserID,
                    Username = q.FriendUser.UserName,
                    Fullname = q.FriendUser.FullName,
                    GenderID = q.FriendUser.UserProfile.GenderID,
                    Birthday = q.FriendUser.UserProfile.Birthday,
                    Age = q.FriendUser.UserProfile.Age,
                    AvatarPath = q.FriendUser.UserProfile.AvatarPath,
                    Subtitle = q.FriendUser.UserProfile.Subtitle,
                    LastActivityTime = q.FriendUser.LastActivity,
                    LastActivityTimeToNow = q.FriendUser.LastActivityToNow,
                })
                .ToList();

            UserProfileBusiness.ActiveUserManger.UpdateFriendListStatus(model.Friends);

            return model;
        }

        public FriendUpdatesViewModels GetUpdates(string userID, DateTime? from, DateTime? to, int count, FriendUpdatesViewModels model)
        {
            var userFriendList = this.ApiEntities.FriendLists
                .Where(q => q.Active && q.UserID == userID)
                .Select(q => q.FriendUserID);

            var query = this.ApiEntities.UserStatuses.AsQueryable();

            if (to.HasValue)
            {
                query = query
                    .Where(q => q.Active && q.CreationTime >= to && (q.UserID == userID || userFriendList.Contains(q.UserID)))
                    .OrderByDescending(q => q.CreationTime);
            }
            else
            {
                if (from == null)
                {
                    from = DateTime.Now;
                }

                query = query
                    .Where(q => q.Active && q.CreationTime <= from && (q.UserID == userID || userFriendList.Contains(q.UserID)))
                    .OrderByDescending(q => q.CreationTime)
                    .Take(count);
            }

            model.Updates = query
                .Select(q => new StatusUpdateViewModels()
                {
                    ID = q.ID,
                    UserID = q.UserID,
                    Username = q.AspNetUser.UserName,
                    Fullname = q.AspNetUser.FullName,
                    GenderID = q.AspNetUser.UserProfile.GenderID,
                    Birthday = q.AspNetUser.UserProfile.Birthday,
                    Age = q.AspNetUser.UserProfile.Age,
                    Subtitle = q.AspNetUser.UserProfile.Subtitle,
                    AvatarPath = q.AspNetUser.UserProfile.AvatarPath,
                    Text = q.Content,
                    Longitude = q.Longitude,
                    Latitude = q.Latitude,
                    CreationTime = q.CreationTime,
                })
                .ToList();

            UserProfileBusiness.ActiveUserManger.UpdateFriendListStatus(model.Updates);

            return model;
        }

        public UserProfileDetailsViewModels GetProfileDetails(string userID, UserProfileDetailsViewModels model)
        {
            var user = this.ApiEntities.AspNetUsers
                .Where(q => q.Id == userID)
                .FirstOrDefault();

            if (user == null)
            {
                return (UserProfileDetailsViewModels)model.ReturnError("Mã người dùng không tồn tại", 1);
            }

            if (user.LockoutEnabled)
            {
                return (UserProfileDetailsViewModels)model.ReturnError("Người dùng này đã bị khóa", 2);
            }

            #region Assign Values

            var profile = user.UserProfile;

            model.UserID = user.Id;
            model.Username = user.UserName;
            model.Fullname = user.FullName;
            model.Subtitle = profile.Subtitle;
            model.AvatarPath = profile.AvatarPath;


            model.Birthday = profile.Birthday;
            model.MobilePhone = profile.MobilePhone;
            model.GenderID = profile.GenderID;
            model.InterestedInID = profile.InterestedInID;
            model.ReligiousViewID = profile.ReligiousViewID;
            model.RelationshipID = profile.RelationshipID;
            model.About = profile.About;
            model.School = profile.School;
            model.Graduated = profile.Graduated;
            model.Work = profile.Work;
            model.WorkPlace = profile.WorkPlace;

            model.Gender = profile.Gender.Name;
            model.InterestedIn = profile.InterestedIn.Name;
            model.ReligiousView = profile.ReligiousView.Name;
            model.Relationship = profile.RelationshipStatu.Name;

            #endregion

            UserProfileBusiness.ActiveUserManger.UpdateFriendListStatus(model);

            return model;
        }

        public BaseApiViewModels UpdateUserActivity(string userID, string loginToken, double? longitude, double? latitude, BaseApiViewModels model)
        {
            var manager = UserProfileBusiness.ActiveUserManger;

            manager.ActivateUser(userID, loginToken);

            if (longitude.HasValue && latitude.HasValue)
            {
                if (manager.UpdateUserLocation(userID, new Coordinate(latitude.Value, longitude.Value)))
                {
                    this.PersistUserLocation(userID, latitude.Value, longitude.Value);
                };

                model.Message = "Coordinate updated with Online status.";
            }
            else
            {
                manager.UpdateUserLocation(userID, null);

                model.Message = "No coordinate provided. Only Online status is keep.";
            }

            return model;
        }

        public StatusUpdateViewModels CreateNewStatusUpdate(string userID, string content, double? longitude, double? latitude, StatusUpdateViewModels model)
        {
            var entity = new UserStatus()
            {
                UserID = userID,
                CreationTime = DateTime.Now,
                Content = content,
                Longitude = longitude,
                Latitude = latitude,
                Active = true,
            };

            this.ApiEntities.UserStatuses.Add(entity);
            this.ApiEntities.SaveChanges();

            model.ID = entity.ID;
            model.Text = entity.Content;
            model.CreationTime = entity.CreationTime;
            model.Longitude = entity.Longitude;
            model.Latitude = entity.Latitude;

            model.Message = "Trạng thái đã được cập nhật thành công.";

            return model;
        }

        public FriendRequestViewModels RequestFriend(string userID, string targetUserID, FriendRequestViewModels model)
        {
            var user = this.ApiEntities.AspNetUsers.Where(q => q.Id == userID && !q.LockoutEnabled).FirstOrDefault();

            if (user == null)
            {
                return (FriendRequestViewModels)model.ReturnError("Người dùng không tồn tại.", 1);
            }

            var targetUser = this.ApiEntities.AspNetUsers.Where(q => q.Id == targetUserID && !q.LockoutEnabled).FirstOrDefault();

            if (targetUser == null)
            {
                return (FriendRequestViewModels)model.ReturnError("Người dùng không tồn tại.", 1);
            }

            // Check if already request
            if (this.ApiEntities.FriendListRequests.Any(q => q.Active && q.UserID == userID && q.TargetUserID == targetUserID))
            {
                return (FriendRequestViewModels)model.ReturnError("Đã tồn tại yêu cầu kết bạn.", 2);
            }

            // Check if already friend
            if (this.ApiEntities.FriendLists.Any(q => q.Active && q.UserID == userID && q.FriendUserID == targetUserID))
            {
                return (FriendRequestViewModels)model.ReturnError("Đã có bạn trong danh sách.", 3);
            }

            // If already requested by the other, still return error, but should be redirected to accept friend
            var counterRequest = this.ApiEntities.FriendListRequests.Where(q => q.Active && q.UserID == targetUserID && q.TargetUserID == userID).FirstOrDefault();
            if (counterRequest != null)
            {
                // Return the ID of the request, so the program can redirect to accept friend
                model.ID = counterRequest.ID;

                return (FriendRequestViewModels)model.ReturnError("Người dùng còn lại đã gửi yêu cầu kết bạn.", 4);
            }

            var requestEntity = new FriendListRequest()
            {
                UserID = userID,
                TargetUserID = targetUserID,
                CreationTime = DateTime.Now,
                Active = true,
            };

            this.ApiEntities.FriendListRequests.Add(requestEntity);

            var notif = new UserNotification()
            {
                CategoryID = NotificationCategory.NotificationCategoriesDictionary[NotificationCategoryEnums.FriendRequest].ID,
                UserID = targetUserID,
                Content = user.FullName,
                Read = false,
                CreationTime = DateTime.Now,
                Active = true,
            };

            this.ApiEntities.UserNotifications.Add(notif);

            this.ApiEntities.SaveChanges();

            model.ID = requestEntity.ID;
            model.CreationTime = requestEntity.CreationTime;
            model.TargetUser = UserProfileBusiness.FromAspNetUser(targetUser);
            model.Exist = true;

            return model;
        }

        public FriendRequestReplyViewModels ReplyFriendRequest(string userID, int requestID, bool accept, FriendRequestReplyViewModels model)
        {
            var user = this.ApiEntities.AspNetUsers.Where(q => q.Id == userID).FirstOrDefault();
            if (user == null)
            {
                return (FriendRequestReplyViewModels)model.ReturnError("Người dùng không tồn tại.", 1);
            }

            var request = this.ApiEntities.FriendListRequests.Where(q => q.Active && q.ID == requestID).FirstOrDefault();
            if (request == null || request.TargetUserID != userID)
            {
                // Even if the request is available, but if the user try to fake it,
                // still return "not found" to protect privacy.

                return (FriendRequestReplyViewModels)model.ReturnError("Không tìm thấy yêu cầu này.", 2);
            }

            model.FriendRequestID = request.ID;
            if (accept)
            {
                // Add to friendlist
                FriendList requestFriendListEntity = new FriendList()
                {
                    UserID = request.UserID,
                    FriendUserID = request.TargetUserID,
                    Since = DateTime.Now,
                    Active = true,
                };
                this.ApiEntities.FriendLists.Add(requestFriendListEntity);

                FriendList replyFriendListEntity = new FriendList()
                {
                    UserID = request.TargetUserID,
                    FriendUserID = request.UserID,
                    Since = DateTime.Now,
                    Active = true,
                };
                this.ApiEntities.FriendLists.Add(replyFriendListEntity);

                // Create notification for the request
                UserNotification notification = new UserNotification()
                {
                    CategoryID = NotificationCategory.NotificationCategoriesDictionary[NotificationCategoryEnums.FriendAccept].ID,
                    UserID = request.UserID,
                    Content = request.TargetUser.FullName,
                    Read = false,
                    CreationTime = DateTime.Now,
                    Active = true,
                };
                this.ApiEntities.UserNotifications.Add(notification);

                // Delete request
                request.Active = false;

                this.ApiEntities.SaveChanges();

                model.Accepted = true;
                model.TargetUser = UserProfileBusiness.FromAspNetUser(request.RequestingUser);
            }
            else
            {
                // Simply delete it
                request.Active = false;

                this.ApiEntities.SaveChanges();
                
                model.Accepted = false;
            }

            return model;
        }

        public BaseApiViewModels Unfriend(string userID, string friendID, BaseApiViewModels model)
        {
            var user = this.ApiEntities.AspNetUsers.FirstOrDefault(q => q.Id == userID);
            if (user == null)
            {
                return (BaseApiViewModels)model.ReturnError("Người dùng không tồn tại.", 1);
            }

            var friendUser = this.ApiEntities.AspNetUsers.FirstOrDefault(q => q.Id == friendID);
            if (friendUser == null)
            {
                return (BaseApiViewModels)model.ReturnError("Người dùng không tồn tại.", 1);
            }

            // Check if exist in friendlist
            if (!this.ApiEntities.FriendLists.Any(q => q.Active && q.UserID == userID && q.FriendUserID == friendID))
            {
                return (BaseApiViewModels)model.ReturnError("Hai người dùng không phải là bạn.", 2);
            }

            { // Delete
                var sourceFriendship = this.ApiEntities.FriendLists.FirstOrDefault(q => q.Active && q.UserID == userID && q.FriendUserID == friendID);
                sourceFriendship.Active = false;

                var targetFriendship = this.ApiEntities.FriendLists.FirstOrDefault(q => q.Active && q.UserID == friendID && q.FriendUserID == userID);
                targetFriendship.Active = false;
            }

            return model;
        }

        public static UserProfileBasicInfo FromAspNetUser(AspNetUser user)
        {
            var profile = user.UserProfile;

            var result = new UserProfileBasicInfo()
            {
                UserID = user.Id,
                Username = user.UserName,
                Fullname = user.FullName,
                GenderID = user.UserProfile.GenderID,
                Birthday = user.UserProfile.Birthday,
                Age = user.UserProfile.Age,
                AvatarPath = user.UserProfile.AvatarPath,
                Subtitle = profile.Subtitle,
            };

            UserProfileBusiness.ActiveUserManger.UpdateFriendListStatus(result);

            return result;
        }

        public void PersistUserLocation(string userID, double longitude, double latitude)
        {
            var stayedPositions = this.ApiEntities.UserLocationHistories
                .Where(q => q.Active && q.UserID == userID)
                .OrderBy(q => q.AccessTime)
                .ToList();

            foreach (var stayedPos in stayedPositions)
            {
                var distance = MathUtils.CalculateFlyingDistance(latitude, longitude, stayedPos.GpsLatitude, stayedPos.GpsLongitude);

                if (distance <= ActiveUserManager.DistanceEpsilon)
                {
                    // Only update access time
                    stayedPos.AccessTime = DateTime.Now;
                    this.ApiEntities.SaveChanges();

                    return;
                }
            }

            // Deactive last one if exceed the maximum record
            if (stayedPositions.Count >= UserProfileBusiness.HistoryRecordKeep)
            {
                stayedPositions.First().Active = false;
            }

            // Add
            var newPos = new UserLocationHistory()
            {
                UserID = userID,
                GpsLatitude = latitude,
                GpsLongitude = longitude,
                AccessTime = DateTime.Now,
                Active = true,
            };
            this.ApiEntities.UserLocationHistories.Add(newPos);

            this.ApiEntities.SaveChanges();
        }

    }
}