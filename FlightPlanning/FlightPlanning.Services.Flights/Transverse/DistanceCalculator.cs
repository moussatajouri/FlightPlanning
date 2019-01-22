using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse
{
    public class DistanceCalculator : IDistanceCalculator
    {
        private static readonly double EQUATORIAL_EARTH_RADIUS = 6378.1370;
        private static readonly double D2R = (Math.PI / 180);

        public double HaversineInKM(double latitude_1, double longitude_1, double latitude_2, double longitude_2)
        {
            double dlong = (longitude_2 - longitude_1) * D2R;
            double dlat = (latitude_2 - latitude_1) * D2R;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(latitude_1 * D2R) * Math.Cos(latitude_2 * D2R) * Math.Pow(Math.Sin(dlong / 2), 2);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            double d = EQUATORIAL_EARTH_RADIUS * c;

            return d;
        }
    }
}
