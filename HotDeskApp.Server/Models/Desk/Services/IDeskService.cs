using HotDeskApp.Server.Models.Desk.Dtos;

namespace HotDeskApp.Server.Models.Desk.Services;

public interface IDeskService
{
    public Task<DeskDto?> GetDeskInfo(Guid deskId);
    public Task<IEnumerable<DeskDto>> GetAllDesksInfo();
    public Task<Guid> CreateNewDesk(Entities.Desk desk);
    public Task<IEnumerable<Guid>> CreateNewDesks(Entities.Desk desk, int numberOfDesks); //iteracyjnie utworzy kilka biurek
    public Task UpdateDesk(Entities.Desk desk);
    public Task DeleteDesk(Guid deskId);
}