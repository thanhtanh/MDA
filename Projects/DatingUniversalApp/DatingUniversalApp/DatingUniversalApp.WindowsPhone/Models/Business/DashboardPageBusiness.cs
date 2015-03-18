using MobileDatingAPI.Models.ViewModels;
using DatingUniversalApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingUniversalApp.Views.Dialog;

namespace DatingUniversalApp.Models.Business
{
    public class DashboardPageBusiness
    {

        private const int FeedPerRequest = 10;

        public DashboardPage View { get; private set; }
        private DateTime LastFeedTime { get; set; }

        public List<StatusUpdateViewModels> Feeds { get; private set; }

        public DashboardPageBusiness(DashboardPage view)
        {
            this.View = view;
            this.Feeds = new List<StatusUpdateViewModels>();

            this.LastFeedTime = DateTime.UtcNow.AddDays(1);
        }

        public async Task<FriendUpdatesViewModels> FeedStatus()
        {
            using (var connector = new ApiConnector())
            {
                List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
                param.Add(new KeyValuePair<string, string>("from", this.LastFeedTime.ToString("o")));
                param.Add(new KeyValuePair<string, string>("count", FeedPerRequest.ToString()));

                var result = await connector.Request<FriendUpdatesViewModels>(
                    ApiConnector.ServerServiceTypes.CommunityService,
                    ApiConnector.ServerCommunityServicePath.StatusUpdate,
                    param);

                if (result.Succeeded)
                {
                    this.Feeds.AddRange(result.Updates);

                    if (result.Updates.Count > 0)
                    {
                        this.LastFeedTime = result.Updates.Last().CreationTime;
                    }
                }

                return result;
            }
        }

        public async Task<FriendUpdatesViewModels> FeedNewStatus()
        {
            if (this.Feeds == null || this.Feeds.Count == 0)
            {
                return null;
            }

            var firstFeed = this.Feeds[0];
            using (var connector = new ApiConnector())
            {
                List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
                param.Add(new KeyValuePair<string, string>("to", firstFeed.CreationTime.ToString("o")));

                var result = await connector.Request<FriendUpdatesViewModels>(
                    ApiConnector.ServerServiceTypes.CommunityService,
                    ApiConnector.ServerCommunityServicePath.StatusUpdate,
                    param);

                if (result.Succeeded)
                {
                    for (int i = 0; i < result.Updates.Count; i++)
                    {
                        var feed = result.Updates[i];

                        if (feed.ID != firstFeed.ID)
                        {
                            this.Feeds.Insert(0, feed);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return result;
            }
        }

        public async Task<StatusUpdateViewModels> PostStatus(NewStatusDialog diag)
        {
            using (ApiConnector connector = new ApiConnector())
            {
                var param = new List<KeyValuePair<string, string>>();

                param.Add(new KeyValuePair<string, string>("content", diag.StatusContent));

                if (diag.ShareLocation)
                {
                    var currLocation = await Utils.Utils.GetCurrentPosition();

                    param.Add(new KeyValuePair<string, string>("latitude", currLocation.Coordinate.Point.Position.Latitude.ToString()));
                    param.Add(new KeyValuePair<string, string>("longitude", currLocation.Coordinate.Point.Position.Longitude.ToString()));
                }

                var result = await connector.Request<StatusUpdateViewModels>(
                    ApiConnector.ServerServiceTypes.CommunityService,
                    ApiConnector.ServerCommunityServicePath.PostStatus,
                    param);

                return result;
            }
        }


    }
}
