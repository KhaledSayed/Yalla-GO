using CarPooling.DTO;
using CarPooling.Models;
using GeoCoordinatePortable;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CarPooling.Helpers
{
    public static class TripHelper
    {
        private static int PricePerKM = 3;

        public static async Task<TripRangePayment> calculateTripFeesRange(Directions direction)
        {

            var distance = direction.Routes[0].Legs[0].Distance;

            var distanceInKm = convertMToKM(distance.CurrentValue);

            var maxFare = Math.Ceiling(distanceInKm * PricePerKM);

            var minFare = Math.Ceiling(maxFare / 3);

            return new TripRangePayment { MaxFare = maxFare , MinFare = minFare};
        }


        public static bool isLocationOnEdge(List<Location> path, Location point, int tolerance = 2)
        {
            var C = new GeoCoordinate(point.Lat, point.Lng);
            for (int i = 0; i < path.Count - 1; i++)
            {
                var A = new GeoCoordinate(path[i].Lat, path[i].Lng);
                var B = new GeoCoordinate(path[i + 1].Lat, path[i + 1].Lng);
                if (Math.Round(A.GetDistanceTo(C) + B.GetDistanceTo(C), tolerance) == Math.Round(A.GetDistanceTo(B), tolerance))
                {
                    return true;
                }
            }
            return false;
        }


        private static double convertMToKM(int value)
        {
            return value / 1000; 
        }


        public static List<ExpectedPoint> convertFromDirectionToListOfExpectedRoutes(Directions directions)
        {
            List<ExpectedPoint> expectedPoints = new List<ExpectedPoint>();
            var steps = directions.Routes[0].Legs[0].Steps;
            for (int i=0; i<steps.Count; i++)
            {
                var step = steps[i];

                var expectedPoint = new ExpectedPoint();
                expectedPoint.Distance = new Models.owned.Item { Text = step.Distance.Text , Value = step.Distance.CurrentValue};
                expectedPoint.StartLocation = new Point(step.StartLocation.Lat, step.StartLocation.Lng) { SRID = 4326 };
                expectedPoint.EndLocation = new Point(step.EndLocation.Lat, step.EndLocation.Lng) { SRID = 4326 };

                expectedPoints.Add(expectedPoint);
            }

            return expectedPoints;
        }
    }
}
