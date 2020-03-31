using System;
using System.Collections.Generic;
using System.Text;
using RightmovePostcodeToLocationId.LocationIdProcessor.Core.Entities.Rightmove;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Core.Entities
{
    public class RightmovePropertyDto
    {
        public int Id { get; set; }
        public int Bedrooms { get; set; }
        public string DisplayAddress { get; set; }
        public LocationDto Location { get; set; }
        public string PropertySubType { get; set; }
        public RightmovePropertyPricingDto Price { get; set; }
    }
}
