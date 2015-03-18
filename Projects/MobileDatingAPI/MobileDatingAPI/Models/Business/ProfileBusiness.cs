using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models.Business
{

    public class ProfileBusiness : BaseBusiness
    {

        public UserProfileDetailsViewModels GetUserProfileDetails(string userId, UserProfileDetailsViewModels model)
        {
            var user = this.ApiEntities.AspNetUsers.Where(q => q.Id == userId).FirstOrDefault();

            if (user == null)
            {
                return (UserProfileDetailsViewModels)model.ReturnError("Invalid User ID.", 1);
            }

            var profile = user.UserProfile;

            model.Username = user.UserName;
            model.Fullname = user.FullName;
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
            model.Subtitle = profile.Subtitle;

            return model;
        }

        public UserProfileDetailsViewModels UpdateUserProfile(string userId, UserProfileDetailsViewModels model)
        {
            var user = this.ApiEntities.AspNetUsers.Where(q => q.Id == userId).FirstOrDefault();
            var profile = user.UserProfile;

            user.FullName = model.Fullname;
            profile.Birthday = model.Birthday;
            profile.MobilePhone = model.MobilePhone;
            profile.GenderID = model.GenderID;
            profile.InterestedInID = model.InterestedInID;
            profile.ReligiousViewID = model.ReligiousViewID;
            profile.RelationshipID = model.RelationshipID;
            profile.About = model.About;
            profile.School = model.School;
            profile.Graduated = model.Graduated;
            profile.Work = model.Work;
            profile.WorkPlace = model.WorkPlace;
            profile.Subtitle = model.Subtitle;

            this.ApiEntities.SaveChanges();

            return model;
        }

    }

}