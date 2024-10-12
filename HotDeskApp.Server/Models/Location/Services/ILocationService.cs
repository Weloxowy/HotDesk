using HotDeskApp.Server.Models.Location.Dtos;

namespace HotDeskApp.Server.Models.Location.Services;

public interface ILocationService
{
    public Task<LocationDto?> GetLocationInfo(Guid deskId);
    public Task<IEnumerable<LocationDto>> GetAllLocationsInfo();
    public Task<Guid> CreateNewLocation(Entities.Location location);
    public Task UpdateLocation(Entities.Location location);
    public Task DeleteLocation(Guid locationId);
}