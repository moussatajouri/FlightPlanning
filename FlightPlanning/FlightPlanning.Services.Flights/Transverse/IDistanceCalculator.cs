using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.Services.Flights.Transverse
{
    public interface IDistanceCalculator
    {
        double HaversineInKM(double latitude_1, double longitude_1, double latitude_2, double longitude_2);
    }
}
