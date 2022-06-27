using NetTopologySuite.Geometries;

namespace Domain.Common
{
    public class Address
    {
        public string AddressLine { get; set; }

        public Point Location { get; set; }

    }
}
