using NetTopologySuite.Geometries;

namespace Domain.Common;

public class Address
{
    public string AddressLine { get; set; }

    public Point Location { get; set; }

    public Address()
    {

    }

    public Address(string address, double lng, double lat) : this()
    {
        AddressLine = address;
        Location = new NetTopologySuite.Geometries.Point(new NetTopologySuite.Geometries.Coordinate(lng, lat)) { SRID = 4326 };
    }
}
