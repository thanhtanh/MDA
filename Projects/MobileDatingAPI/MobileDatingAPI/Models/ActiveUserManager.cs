using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using Wintellect.PowerCollections;

namespace MobileDatingAPI.Models
{

    public class ActiveUserManager : IDisposable
    {

        private const int SecondToBeInactive = 120;
        private const int CleanUpInterval = 30000;

        public const double DistanceEpsilon = 100;
        public const int StayTime = 120;

        private object cleanUpLocker = new object();

        public ConcurrentDictionary<string, ActiveUser> ActiveUsers { get; private set; }

        public bool Disposed { get; private set; }
        private Thread CleanUpThread { get; set; }

        public ActiveUserManager()
        {
            this.ActiveUsers = new ConcurrentDictionary<string, ActiveUser>();

            this.CleanUpThread = new Thread(() =>
            {
                while (!this.Disposed)
                {
                    this.CleanUp();

                    Thread.Sleep(CleanUpInterval);
                }
            });
            this.CleanUpThread.Start();
        }

        public void ActivateUser(string userId)
        {
            this.ActivateUser(userId, null);
        }

        public void ActivateUser(string userId, string loginToken)
        {
            ActiveUser activeUser;

            if (!this.ActiveUsers.TryGetValue(userId, out activeUser))
            {
                activeUser = new ActiveUser()
                {
                    UserId = userId,
                };
                this.ActiveUsers.TryAdd(userId, activeUser);
            }

            if (!string.IsNullOrEmpty(loginToken))
            {
                activeUser.LoginToken = loginToken;
            }

            activeUser.LastActivity = DateTime.Now;
        }

        public bool UpdateUserLocation(string userId, Coordinate location)
        {
            ActiveUser activeUser;
            if (this.ActiveUsers.TryGetValue(userId, out activeUser))
            {
                if (activeUser.LastCoordination != null && location != null)
                {
                    var oldPos = activeUser.LastCoordination;
                    var newPos = location;

                    var distance = MathUtils.CalculateFlyingDistance(oldPos.Latitude, oldPos.Longitude, newPos.Latitude, newPos.Longitude);
                    if (distance < ActiveUserManager.DistanceEpsilon)
                    {
                        if ((DateTime.Now - activeUser.StayStartTime).Seconds >= ActiveUserManager.StayTime)
                        {
                            activeUser.StayStartTime = DateTime.MaxValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        activeUser.LastCoordination = location;
                        activeUser.StayStartTime = DateTime.Now;

                        return false;
                    }
                }
                else
                {
                    activeUser.LastCoordination = location;
                    activeUser.StayStartTime = DateTime.Now;

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool UpdateFriendListStatus(IUserActiveInfo item)
        {
            ActiveUser activeUser;
            if (item.Online = this.ActiveUsers.TryGetValue(item.UserID, out activeUser))
            {
                item.Location = activeUser.LastCoordination;
                item.LastActivityTime = null;
                item.LastActivityTimeToNow = null;

                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateFriendListStatus(IEnumerable<IUserActiveInfo> list)
        {
            foreach (var item in list)
            {
                this.UpdateFriendListStatus(item);
            }
        }

        public void CleanUp()
        {
            lock (this.cleanUpLocker)
            {
                Debug.WriteLine("Cleaning up Active Users...");
                var now = DateTime.Now;

                var keys = this.ActiveUsers.Keys.ToList();

                using (var dc = new MobileDatingApiEntities())
                {
                    foreach (var userId in keys)
                    {
                        var user = this.ActiveUsers[userId];

                        if ((now - user.LastActivity).Seconds > SecondToBeInactive)
                        {
                            ActiveUser temp;
                            this.ActiveUsers.TryRemove(userId, out temp);
                        }

                        var userEntity = dc.AspNetUsers.Where(q => q.Id == userId).FirstOrDefault();
                        if (userEntity != null)
                        {
                            userEntity.LastActivity = user.LastActivity;
                        }
                    }

                    dc.SaveChanges();
                }

            }

        }

        public void Dispose()
        {
            if (this.Disposed)
            {
                return;
            }

            this.Disposed = true;

            // Perform last clean up
            this.CleanUp();

            // Clear
            this.ActiveUsers.Clear();
        }

    }

    //public class UserLocationManager
    //{

    //    public OrderedMultiDictionary<double, ActiveUser> LongitudeBasedUserList { get; private set; }
    //    public OrderedMultiDictionary<double, ActiveUser> LatitudeBasedUserList { get; private set; }

    //    public UserLocationManager()
    //    {
    //        this.LongitudeBasedUserList = new OrderedMultiDictionary<double, ActiveUser>(true);
    //        this.LatitudeBasedUserList = new OrderedMultiDictionary<double, ActiveUser>(true);

    //    }

    //    public void AddUserToList(ActiveUser user)
    //    {
    //        if (user.LastCoordination == null) { return; }

    //        var lat = user.LastCoordination.Value.Latitude;
    //        var lon = user.LastCoordination.Value.Longitude;

    //        this.LatitudeBasedUserList.Add(lat, user);
    //        this.LongitudeBasedUserList.Add(lon, user);
    //    }

    //    public void RemoveUserFromList(ActiveUser user)
    //    {
    //        if (user.LastCoordination == null) { return; }

    //        var lat = user.LastCoordination.Value.Latitude;
    //        var lon = user.LastCoordination.Value.Longitude;

    //        this.LatitudeBasedUserList.Remove(lat, user);
    //        this.LongitudeBasedUserList.Remove(lon, user);
    //    }

    //}

    public class ActiveUser
    {

        public string LoginToken { get; set; }
        public string UserId { get; set; }
        public DateTime LastActivity { get; set; }
        public Coordinate LastCoordination { get; set; }
        public DateTime StayStartTime { get; set; }

    }



}
