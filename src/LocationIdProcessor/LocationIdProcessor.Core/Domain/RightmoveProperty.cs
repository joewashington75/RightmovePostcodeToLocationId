using RightmovePostcodeToLocationId.LocationIdProcessor.Core.Enums;
using RightmovePostcodeToLocationId.LocationIdProcessor.Core.Interfaces.Domain;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Core.Domain
{
    public class RightmoveProperty : Property<int>, IAggregate
    {
        public RightmoveProperty(int externalSourceId, int noOfBedrooms, PropertyType propertyType, string displayAddress, 
            decimal price, string priceCurrencyCode, PaymentFrequency priceFrequency, string postcode, Location location) 
            : base(externalSourceId, noOfBedrooms, propertyType, displayAddress, price, priceCurrencyCode, priceFrequency, location)
        {
            Postcode = postcode;
        }

        public string Postcode { get; private set; }
    }
}
