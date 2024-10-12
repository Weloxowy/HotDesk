using HotDeskApp.Server.Models.Desk.Repositories;
using HotDeskApp.Server.Models.Location.Dtos;
using HotDeskApp.Server.Models.Location.Repositories;

namespace HotDeskApp.Server.Models.Location.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<LocationDto?> GetLocationInfo(Guid deskId)
    {
        var location = await _locationRepository.Get(deskId);
        return location != null ? location.ToDto() : null;
    }

    public async Task<IEnumerable<LocationDto>> GetAllLocationsInfo()
    {
        var list = await _locationRepository.GetAll();
        var newList = new List<LocationDto>();
        foreach (var desk in list) newList.Add(desk.ToDto());
        return newList;
    }

    public async Task<Guid> CreateNewLocation(Entities.Location location)
    {
        return await _locationRepository.Save(location);
    }

    public async Task UpdateLocation(Entities.Location location)
    {
        await _locationRepository.Update(location);
    }

    public async Task DeleteLocation(Guid locationId)
    {
        await _locationRepository.Delete(locationId);
    }
}