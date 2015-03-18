using System;
using System.Collections.Generic;
using System.Text;

namespace DatingUniversalApp.Models.Utils
{

    public static class MathUtils
    {

        public const double Pi = Math.PI;
        public const double ToRadianMultiplier = MathUtils.Pi / 180;
        public const double ToDegreeMultiplier = 180 / MathUtils.Pi;

        /// <summary>
        /// The radius of Earth, in meters
        /// </summary>
        public const double EarthRadius = 6371000;

        /// <summary>
        /// Calculate the flying distance between 2 coordinate.
        /// </summary>
        /// <param name="long1">Longitude of the first coordinate.</param>
        /// <param name="lat1">Latitude of the first coordinate.</param>
        /// <param name="long2">Longitude of the second coordinate.</param>
        /// <param name="lat2">Latitude of the second coordinate.</param>
        /// <returns>Distance in meters between the two coordinate.</returns>
        /// <remarks>Reference: http://www.movable-type.co.uk/scripts/latlong.html. </remarks>
        public static double CalculateFlyingDistance(double lat1, double long1, double lat2, double long2)
        {
            var φ1 =  lat1 * MathUtils.ToRadianMultiplier;
            var φ2 =  lat2 * MathUtils.ToRadianMultiplier;

            var Δφ = (lat2 - lat1) * MathUtils.ToRadianMultiplier;
            var Δλ = (long2 - long1) * MathUtils.ToRadianMultiplier;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = MathUtils.EarthRadius * c;

            return d;   
        }

    }

}
