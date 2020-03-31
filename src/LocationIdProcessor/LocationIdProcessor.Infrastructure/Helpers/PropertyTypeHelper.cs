using RightmovePostcodeToLocationId.LocationIdProcessor.Core.Enums;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Infrastructure.Helpers
{
    public static class PropertyTypeHelper
    {
        public static PropertyType GetPropertyType(this string propertyType)
        {
            switch (propertyType)
            {
                case "Flat":
                    return PropertyType.Flat;
                default:
                    return PropertyType.House;
            }
        }
    }
}