using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models.Business
{

    public class MatchingBusiness : BaseBusiness
    {

        public UserMatchingInfoViewModels SearchUserByKeyword(string keyword, UserMatchingInfoViewModels model)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Trim().Length < 3)
            {
                return (UserMatchingInfoViewModels)model.ReturnError("Từ khóa tìm kiếm phải từ 3 ký tự trở lên.", 1);
            }

            var matchingUsers = this.ApiEntities.AspNetUsers
                .Where(q => q.LockoutEnabled &&
                    (q.UserName == keyword ||
                    q.FullName.Contains(keyword) ||
                    q.UserProfile.MobilePhone == keyword ||
                    q.Id == keyword));

            model.MatchingUsers = matchingUsers.Select(q => new UserProfileBasicInfo()
            {
                UserID = q.Id,
                Username = q.UserName,
                Fullname = q.FullName,
                GenderID = q.UserProfile.GenderID,
                Birthday = q.UserProfile.Birthday,
                Age = q.UserProfile.Age,
                AvatarPath = q.UserProfile.AvatarPath,
                Subtitle = q.UserProfile.Subtitle,
                LastActivityTime = q.LastActivity,
                LastActivityTimeToNow = q.LastActivityToNow,
            })
            .ToList();

            UserProfileBusiness.ActiveUserManger.UpdateFriendListStatus(model.MatchingUsers);

            return model;
        }

        public UserMatchingInfoViewModels FindNearbyUser(string userId, double latitude, double longitude, double distance, UserMatchingInfoViewModels model)
        {
            var bounding = MathUtils.CalculateBoundingRectangle(latitude, longitude, distance);

            model.MatchingUsers = this.ApiEntities.UserLocationHistories
                .Where(q => q.Active &&
                    (q.GpsLatitude >= bounding.MinLatitude && q.GpsLatitude <= bounding.MaxLatitude) &&
                    (q.GpsLongitude >= bounding.MinLongitude && q.GpsLongitude <= bounding.MaxLongitude))
                .Select(q => new UserProfileBasicInfo
                {
                    UserID = q.UserID,
                    Username = q.AspNetUser.UserName,
                    Fullname = q.AspNetUser.FullName,
                    GenderID = q.AspNetUser.UserProfile.GenderID,
                    Birthday = q.AspNetUser.UserProfile.Birthday,
                    Age = q.AspNetUser.UserProfile.Age,
                    AvatarPath = q.AspNetUser.UserProfile.AvatarPath,
                    Subtitle = q.AspNetUser.UserProfile.Subtitle,
                    LastActivityTime = q.AspNetUser.LastActivity,
                    LastActivityTimeToNow = q.AspNetUser.LastActivityToNow,
                    Location = new Coordinate()
                    {
                        Latitude = q.GpsLatitude,
                        Longitude = q.GpsLongitude,
                    },
                })
                .Distinct()
                .ToList();

            HashSet<string> userIDInList = new HashSet<string>();
            foreach (var user in model.MatchingUsers)
            {
                userIDInList.Add(user.UserID);
            }

            foreach (var activeUser in UserProfileBusiness.ActiveUserManger.ActiveUsers)
            {
                if (activeUser.Value.LastCoordination == null || userIDInList.Contains(activeUser.Key)) { continue; }

                var coordinate = activeUser.Value.LastCoordination;

                if (coordinate.Latitude >= bounding.MinLatitude && coordinate.Latitude <= bounding.MaxLatitude
                    && coordinate.Longitude >= bounding.MinLongitude && coordinate.Longitude <= bounding.MaxLongitude)
                {
                    model.MatchingUsers.Add(UserProfileBusiness.FromAspNetUser(this.ApiEntities.AspNetUsers.Where(q => q.Id == activeUser.Key).First()));
                    userIDInList.Add(activeUser.Key);
                }
            }

            model.MatchingUsers = model.MatchingUsers.ToList();

            UserProfileBusiness.ActiveUserManger.UpdateFriendListStatus(model.MatchingUsers);

            return model;
        }

        public UserMatchingInfoViewModels SearchUserByConditions(UserSearchingViewModels conditions, UserMatchingInfoViewModels model)
        {
            if (!conditions.ValidateConditions())
            {
                return (UserMatchingInfoViewModels)model.ReturnError("Không có điều kiện để tìm.", 1);
            }

            var matchingUsersQuery = this.ApiEntities.AspNetUsers.Where(q => !q.LockoutEnabled);

            #region Applying Conditions

            if (conditions.AgeMin.HasValue)
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.Age >= conditions.AgeMin.Value);
            }

            if (conditions.AgeMax.HasValue)
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.Age <= conditions.AgeMin.Value);
            }

            if (conditions.GenderID.HasValue)
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.GenderID == conditions.GenderID);
            }

            if (conditions.InterestedInID.HasValue)
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.InterestedInID == conditions.InterestedInID);
            }

            if (conditions.ReligiousViewID.HasValue)
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.ReligiousViewID == conditions.ReligiousViewID);
            }

            if (!string.IsNullOrWhiteSpace(conditions.School))
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.School.Contains(conditions.School));
            }

            if (conditions.SchoolGraduated.HasValue)
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.Graduated == conditions.SchoolGraduated);
            }

            if (!string.IsNullOrWhiteSpace(conditions.Work))
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.Work.Contains(conditions.Work));
            }

            if (!string.IsNullOrWhiteSpace(conditions.Workplace))
            {
                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserProfile.WorkPlace.Contains(conditions.Workplace));
            }

            if (conditions.MaximumDistance.HasValue && conditions.Latitude.HasValue && conditions.Longitude.HasValue)
            {
                var boundingRect = MathUtils.CalculateBoundingRectangle(conditions.Latitude.Value, conditions.Longitude.Value, conditions.MaximumDistance.Value);

                matchingUsersQuery = matchingUsersQuery.Where(q => q.UserLocationHistories.Any(p => 
                    p.Active &&
                    p.GpsLatitude >= boundingRect.MinLatitude && p.GpsLatitude <= boundingRect.MaxLatitude &&
                    p.GpsLongitude >= boundingRect.MinLongitude && p.GpsLongitude <= boundingRect.MaxLongitude));
            }

            #endregion

            model.MatchingUsers = matchingUsersQuery.Select(q => new UserProfileBasicInfo()
            {
                UserID = q.Id,
                Username = q.UserName,
                Fullname = q.FullName,
                GenderID = q.UserProfile.GenderID,
                Birthday = q.UserProfile.Birthday,
                Age = q.UserProfile.Age,
                AvatarPath = q.UserProfile.AvatarPath,
                Subtitle = q.UserProfile.Subtitle,
                LastActivityTime = q.LastActivity,
                LastActivityTimeToNow = q.LastActivityToNow,
            }).ToList();

            UserProfileBusiness.ActiveUserManger.UpdateFriendListStatus(model.MatchingUsers);

            return model;
        }

    }

}