using Domain.GeoJson;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Utilities;
using NavSpatialData.GeoJsonDTO;
using System.IO;

namespace NavSpatialData.Mapping
{
    public class GeometryConverter 
    {
        public static GeoJsonGeometry ConvertToGeoJsonLineString(Geometry spatial)
        {
            if (spatial == null) return null;


            if (!(spatial is LineString lineString))
                return null;  // Or log an error, or handle as needed

            // Process the coordinates, ensuring no nulls are present
            var coordinates = lineString.Coordinates
                .Where(coord => coord != null)  // Ensure no null coordinates
                .Select(coord => new decimal[] { (decimal)coord.Y, (decimal)coord.X })  // Convert to decimal arrays
                .ToArray();

            return new GeoJsonLineString
            {
                Coordinates = coordinates
            };
        }
        public static GeoJsonGeometry ConvertGeometryPolygon(Geometry spatial)
        {
            if (spatial == null) return null;

            if (!(spatial is Polygon polygon))
                return null; // Or log an error

            var coordinatesList = new List<decimal[][]>();

            var exteriorCoordinates = polygon.Shell.Coordinates
                .Select(coord => new decimal[] { (decimal)coord.X, (decimal)coord.Y })
                .ToArray();
            // Ensure the exterior ring is closed
            if (!IsRingClosed(exteriorCoordinates))
            {
                exteriorCoordinates = CloseRing(exteriorCoordinates);
            }
            coordinatesList.Add(exteriorCoordinates); // Add the exterior ring

            foreach (var ring in polygon.Holes)
            {
                var interiorCoordinates = ring.Coordinates
                    .Select(coord => new decimal[] { (decimal)coord.X, (decimal)coord.Y })
                    .ToArray();
                // Ensure the interior ring is closed
                if (!IsRingClosed(interiorCoordinates))
                {
                    interiorCoordinates = CloseRing(interiorCoordinates);
                }
                coordinatesList.Add(interiorCoordinates); // Add the interior ring
            }

            return new GeoJsonPolygon
            {
                Coordinates = coordinatesList.ToArray()
            };
        }

        private static bool IsRingClosed(decimal[][] coordinates)
        {
            if (coordinates.Length == 0) return false;
            // Check if first and last coordinate are the same
            return coordinates[0][0] == coordinates[^1][0] && coordinates[0][1] == coordinates[^1][1];
        }

        private static decimal[][] CloseRing(decimal[][] coordinates)
        {
            var closedCoordinates = new List<decimal[]>(coordinates)
                {
                    coordinates[0] // Add the starting point at the end to close the polygon
                };
            return closedCoordinates.ToArray();
        }




    }
}
