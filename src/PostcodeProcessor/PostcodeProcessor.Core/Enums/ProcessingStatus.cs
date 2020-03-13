namespace RightmovePostcodeToLocationId.PostcodeProcessor.Core.Enums
{
    public enum ProcessingStatus
    {
        RetrievingLocationId = 1,
        SearchForPostcode,
        NoResultsForPostcode,
        PostcodeNotFound,
        PostcodeFound,
        LocationIdRetrieved,
        PostcodeNotFoundInUrl,
        RetrievingDataFromApi,
        ApiDataRetrieved,
        Exception
    }
}
