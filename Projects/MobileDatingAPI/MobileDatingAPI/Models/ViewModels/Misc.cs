using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{

    public interface IUserActiveInfo
    {
        string UserID { get; set; }
        bool Online { get; set; }
        Coordinate Location { get; set; }
        DateTime? LastActivityTime { get; set; }
        DateTime? LastActivityTimeToNow { get; set; }
    }

    public class Coordinate
    {
        public double Longitude;
        public double Latitude;

        public Coordinate() { }

        public Coordinate(double latitude, double longitude) : this()
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

    }

    public struct CoordinateBounding
    {
        public double MinLatitude { get; set; }
        public double MaxLatitude { get; set; }
        public double MinLongitude { get; set; }
        public double MaxLongitude { get; set; }
        
    }

}