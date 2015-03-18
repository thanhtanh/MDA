using MobileDatingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingAPI.Controllers
{
    public class TestController : Controller
    {

        public async Task<ActionResult> CreateTestAccounts()
        {
            MobileDatingApiEntities dc = new MobileDatingApiEntities();
            var userManager = AuthenUtils.UserManager;

            for (int i = 0; i < 100; i++)
            {
                var username = "testaccount" + i;
                var fullname = "Test Account " + i;
                var email = "testaccount" + i + "@outlook.com";

                var user = dc.AspNetUsers.Where(q => q.UserName == username).FirstOrDefault();
                if (user == null)
                {
                    ApplicationUser appUser = new ApplicationUser()
                    {
                        UserName = username,
                        Email = email,
                    };

                    var opResult = await userManager.CreateAsync(appUser, "zaq@123");

                    if (!opResult.Succeeded)
                    {
                        return this.Content(opResult.Errors.First());
                    }
                    else
                    {
                        user = dc.AspNetUsers.Where(q => q.UserName == username).FirstOrDefault();
                        user.FullName = fullname;

                        UserProfile profile = new UserProfile()
                        {
                            UserID = user.Id,
                        };
                        dc.UserProfiles.Add(profile);

                        dc.SaveChanges();
                    }
                }
            }

            return null;
        }

        public ActionResult ConnectFriendTestAccounts()
        {
            MobileDatingApiEntities dc = new MobileDatingApiEntities();

            for (int i = 0; i < 99; i++)
            {
                int lower = i + 1;
                int upper = Math.Min(99, i + 25);

                var fromUser = dc.AspNetUsers.Where(q => q.UserName == "testaccount" + i).First();

                for (int j = lower; j < upper; j++)
                {
                    if (i == j) { continue; }

                    var toUser = dc.AspNetUsers.Where(q => q.UserName == "testaccount" + j).First();

                    var friendshipFrom = new FriendList()
                    {
                        UserID = fromUser.Id,
                        FriendUserID = toUser.Id,
                        Since = DateTime.Now,
                        Active = true,
                    };
                    dc.FriendLists.Add(friendshipFrom);

                    var friendshipTo = new FriendList()
                    {
                        UserID = toUser.Id,
                        FriendUserID = fromUser.Id,
                        Since = DateTime.Now,
                        Active = true,
                    };
                    dc.FriendLists.Add(friendshipTo);
                }

                
            }

            dc.SaveChanges();

            return this.Content("Done");
        }

    }
}