using DatingUniversalApp.Models.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileDatingAPI.Models.ViewModels
{

    public partial class FriendListViewModels
    {
        
        private List<UserPositonGroup> userPositionGroupField;
        public List<UserPositonGroup> UserPositionGroups
        {
            get
            {
                Debug.WriteLine(MathUtils.CalculateFlyingDistance(10.860655, 106.612894, 10.789849, 106.708403));

                if (this.userPositionGroupField == null)
                {
                    this.userPositionGroupField = new List<UserPositonGroup>();

                    foreach (var friend in this.Friends)
                    {
                        if (!friend.Online || friend.Location == null) { continue; }


                    }
                }

                return this.userPositionGroupField;
            }
        }

    }

    public class UserPositonGroup
    {
        public Coordinate Center { get; set; }
        public List<UserProfileBasicInfo> Users { get; set; }

        public UserPositonGroup(Coordinate center)
        {
            this.Center = center;
            this.Users = new List<UserProfileBasicInfo>();
        }

        public UserPositonGroup(Coordinate center, params UserProfileBasicInfo[] users)
            : this(center)
        {
            this.Users.AddRange(users);
        }

    }

}
