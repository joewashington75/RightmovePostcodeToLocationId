using RightmovePostcodeToLocationId.LocationIdProcessor.Core.Enums;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Infrastructure.Helpers
{
    public static class PaymentFrequencyHelper
    {
        public static PaymentFrequency GetFrequencyType(this string propertyType)
        {
            switch (propertyType)
            {
                case "weekly":
                    return PaymentFrequency.Weekly;
                default:
                    return PaymentFrequency.Monthly;
            }
        }
    }
}