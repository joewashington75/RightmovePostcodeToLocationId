using RightmovePostcodeToLocationId.LocationIdProcessor.Core.Enums;
using RightmovePostcodeToLocationId.LocationIdProcessor.Core.Interfaces.Domain;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Core.Domain
{
    public abstract class Property<T> : Entity, IExternalSourceProperty<T>
    {
        protected Property(T externalSourceId, int noOfBedrooms, PropertyType propertyType, string displayAddress, decimal price, 
            string priceCurrencyCode, PaymentFrequency paymentFrequency, Location location)
        {
            ExternalSourceId = externalSourceId;
            NoOfBedrooms = noOfBedrooms;
            Type = propertyType;
            DisplayAddress = displayAddress;
            Price = price;
            Location = location;
            PriceCurrencyCode = priceCurrencyCode;
            PaymentFrequency = paymentFrequency;
        }

        public int NoOfBedrooms { get; private set; }
        public PropertyType Type { get; private set; }
        public string DisplayAddress { get; private set; }
        public decimal Price { get; private set; }
        public string PriceCurrencyCode { get; private set; }
        public PaymentFrequency PaymentFrequency { get; private set; }
        public T ExternalSourceId { get; private set; }
        public Location Location { get; private set; }
    }
}