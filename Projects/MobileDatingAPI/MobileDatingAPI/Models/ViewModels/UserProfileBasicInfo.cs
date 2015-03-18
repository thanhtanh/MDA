using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{
    public partial class UserProfileBasicInfo : BaseApiViewModels, IUserActiveInfo, IComparable<UserProfileBasicInfo>
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Age { get; set; }
        public int? GenderID { get; set; }
        public string Subtitle { get; set; }
        public string AvatarPath { get; set; }
        public bool Online { get; set; }
        public Coordinate Location { get; set; }
        public DateTime? LastActivityTime { get; set; }
        public DateTime? LastActivityTimeToNow { get; set; }

        public UserProfileBasicInfo() { }

        public override bool Equals(object obj)
        {
            if (obj is UserProfileBasicInfo)
            {
                return this.UserID.Equals((obj as UserProfileBasicInfo).UserID);
            }

            return base.Equals(obj);
        }

        public int CompareTo(UserProfileBasicInfo other)
        {
            return this.UserID.CompareTo(other.UserID);
        }
    }
}