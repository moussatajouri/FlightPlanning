using FlightPlanning.Services.Flights.Transverse;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FlightPlanning.Services.Flights.Tests.UnitTests.Transverse
{
    public class DistanceCalculatorTests
    {
        [Theory]
        [InlineData(0,0,0,0)]
        [InlineData(30, 10, 30, 10)]
        [InlineData(-15, -30, -15, -30)]
        [InlineData(15, -30, 15, -30)]
        [InlineData(-15, 30, -15, 30)]
        public void Should_DistanceCalculator_Return_Zero_When_SameGpsPosition(double latitude_1, double longitude_1, double latitude_2, double longitude_2)
        {
            Assert.Equal(0, new DistanceCalculator().HaversineInKM(latitude_1, longitude_1, latitude_2, longitude_2));
        }

        [Theory]
        [InlineData(-5.826789855957031, 144.29600524902344, -6.081689834590001, 145.391998291, 124.620491255242)]
        [InlineData(-5.826789855957031, 144.29600524902344, 65.2833023071289, -14.401399612426758, 13210.3326005712)]
        [InlineData(-5.826789855957031, 144.29600524902344, 79.9946975708, -85.814201355, 11373.578267078)]
        [InlineData(-5.826789855957031, 144.29600524902344, -6.081689834590001, 48.37189865112305, 10602.1724282828)]
        public void Should_DistanceCalculator_Return_Distance_When_DifferentGpsPosition(double latitude_1, double longitude_1, double latitude_2, double longitude_2,double expectedDistance)
        {
            Assert.Equal(expectedDistance, new DistanceCalculator().HaversineInKM(latitude_1, longitude_1, latitude_2, longitude_2), 5);
        }

        [Theory]
        [InlineData(-5.826789855957031, 144.29600524902344, -6.081689834590001, 145.391998291, 124.620491255242)]
        [InlineData(-5.826789855957031, 144.29600524902344, 65.2833023071289, -14.401399612426758, 13210.3326005712)]
        [InlineData(-5.826789855957031, 144.29600524902344, 79.9946975708, -85.814201355, 11373.578267078)]
        [InlineData(-5.826789855957031, 144.29600524902344, -6.081689834590001, 48.37189865112305, 10602.1724282828)]
        public void Should_DistanceCalculator_Return_SameDistance_When_InverseDirection(double latitude_1, double longitude_1, double latitude_2, double longitude_2, double expectedDistance)
        {
            var distanceDirection_1 = new DistanceCalculator().HaversineInKM(latitude_1, longitude_1, latitude_2, longitude_2);
            var distanceDirection_2 = new DistanceCalculator().HaversineInKM(latitude_2, longitude_2, latitude_1, longitude_1);
            Assert.Equal(distanceDirection_1, distanceDirection_2, 5);
        }

        [Theory]
        [InlineData(180, 90, 180, 90)]
        [InlineData(180, -90, 180, -90)]
        [InlineData(-180, 90, -180, 90)]
        [InlineData(-180, -90, -180, -90)]        
        public void Should_DistanceCalculator_Return_Zero_When_InverseDirection(double latitude_1, double longitude_1, double latitude_2, double longitude_2)
        {
            Assert.Equal(0, new DistanceCalculator().HaversineInKM(latitude_1, longitude_1, latitude_2, longitude_2));
        }
    }
}
