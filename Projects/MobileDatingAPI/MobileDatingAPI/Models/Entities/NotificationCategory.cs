using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models
{
    
    public partial class NotificationCategory
    {

        public static Dictionary<string, NotificationCategory> NotificationCategoriesDictionary { get; private set; }
        
        static NotificationCategory()
        {
            NotificationCategory.RefreshCache();
        }

        public static void RefreshCache()
        {
            MobileDatingApiEntities dc = new MobileDatingApiEntities();

            var categories = dc.NotificationCategories.Where(q => q.Active);

            NotificationCategory.NotificationCategoriesDictionary = new Dictionary<string, NotificationCategory>();
            foreach (var category in categories)
            {
                NotificationCategory.NotificationCategoriesDictionary.Add(category.Name, category);
            }
        }

    }

    public static class NotificationCategoryEnums
    {

        public const string FriendRequest = "FriendRequest";
        public const string FriendAccept = "FriendAccept";

    }

}