namespace RightmovePostcodeToLocationId.LocationIdProcessor.Core.Interfaces.Domain
{
    public interface IExternalSourceProperty<T>
    {
        T ExternalSourceId { get; }        
    }
}