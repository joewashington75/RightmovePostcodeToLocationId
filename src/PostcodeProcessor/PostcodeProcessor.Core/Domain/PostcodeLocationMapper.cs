using RightmovePostcodeToLocationId.PostcodeProcessor.Core.Enums;

namespace RightmovePostcodeToLocationId.PostcodeProcessor.Core.Domain
{
    public class PostcodeLocationMapper
    {
        public string Postcode { get; private set; }
        public string LocationId { get; private set; }
        public ProcessingStatus ProcessingStatus { get; private set; }
    }
}
