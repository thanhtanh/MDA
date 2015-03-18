using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{

    public class UserProfileDetailsViewModels : BaseApiViewModels, IUserActiveInfo
    {

        public string UserID { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Subtitle { get; set; }
        public string AvatarPath { get; set; }
        public bool Online { get; set; }
        public Coordinate Location { get; set; }
        public DateTime? LastActivityTime { get; set; }
        public DateTime? LastActivityTimeToNow { get; set; }

        public DateTime? Birthday { get; set; }
        public string MobilePhone { get; set; }
        public int? GenderID { get; set; }
        public int? InterestedInID { get; set; }
        public int? ReligiousViewID { get; set; }
        public int? RelationshipID { get; set; }
        public string About { get; set; }
        public string School { get; set; }
        public bool? Graduated { get; set; }
        public string Work { get; set; }
        public string WorkPlace { get; set; }

        public string Gender { get; set; }
        public string InterestedIn { get; set; }
        public string ReligiousView { get; set; }
        public string Relationship { get; set; }

    }

}