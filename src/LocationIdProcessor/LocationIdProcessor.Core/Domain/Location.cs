namespace RightmovePostcodeToLocationId.LocationIdProcessor.Core.Domain
{
    public class Location : ValueObject<Location>
    {
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
    }
}