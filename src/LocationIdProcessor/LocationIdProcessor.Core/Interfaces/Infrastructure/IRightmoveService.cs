using System.Threading.Tasks;

namespace RightmovePostcodeToLocationId.LocationIdProcessor.Core.Interfaces.Infrastructure
{
    public interface IRightmoveService
    {
        Task GetDataAsync(string postcode, string locationId);
    }
}
