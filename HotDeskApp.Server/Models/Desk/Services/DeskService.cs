using HotDeskApp.Server.Models.Desk.Dtos;
using HotDeskApp.Server.Models.Desk.Repositories;

namespace HotDeskApp.Server.Models.Desk.Services;

public class DeskService : IDeskService
{
    private readonly IDeskRepository _deskRepository;

    public DeskService(IDeskRepository deskRepository)
    {
        _deskRepository = deskRepository;
    }

    public async Task<DeskDto?> GetDeskInfo(Guid deskId)
    {
        var desk = await _deskRepository.Get(deskId);
        return desk != null ? desk.ToDto() : null;
    }

    public async Task<Entities.Desk?> GetDeskEntityInfo(Guid deskId)
    {
        var desk = await _deskRepository.Get(deskId);
        return desk != null ? desk : null;
    }
    public async Task<IEnumerable<DeskDto>> GetAllDesksInfo()
    {
        var list = await _deskRepository.GetAll();
        var newList = new List<DeskDto>();
        foreach (var desk in list)
        {
            newList.Add(desk.ToDto());
        }

        return newList;
    }

    public async Task<IEnumerable<DeskDto>> GetAllDesksInfoByLocation(Guid locationId)
    {
        var list = await _deskRepository.GetAll();
        list = list.Where(x => x.Location.Id == locationId);
        var newList = new List<DeskDto>();
        foreach (var desk in list)
        {
            newList.Add(desk.ToDto());
        }

        return newList;
    }

    public async Task<Guid> CreateNewDesk(Entities.Desk desk)
    {
        return await _deskRepository.Save(desk);
    }

    public async Task<IEnumerable<Guid>> CreateNewDesks(Entities.Desk desk, int numberOfDesks)
    {
        var newList = new List<Guid>();

        var id0 = await _deskRepository.Save(desk);
        newList.Add(id0);

        for (int i = 1; i < numberOfDesks; i++)
        {
            var newDesk = new Entities.Desk
            {
                Name = desk.Name + "_" + i,
                Description = desk.Description,
                IsMaintnance = desk.IsMaintnance,
                Location = desk.Location,
            };

            var id = await _deskRepository.Save(newDesk);
            newList.Add(id);
        }

        return newList;
    }

    public async Task UpdateDesk(Entities.Desk desk)
    {
        await _deskRepository.Update(desk);
    }

    public async Task DeleteDesk(Guid deskId)
    {
        await _deskRepository.Delete(deskId);
    }
    
}