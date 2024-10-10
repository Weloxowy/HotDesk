using HotDeskApp.Server.Infrastructure;

namespace HotDeskApp.Server.Models.UserEntity.Repositories;

public interface IUserEntityRepository : IRepository<Entities.UserEntity>
{
    public Task<Entities.UserEntity?> GetByEmailAsync(string email);
}