using HotDeskApp.Server.Models.UserEntity.Repositories;
using HotDeskApp.Server.Infrastructure;
using NHibernate.Linq;
using ISession = NHibernate.ISession;

namespace HotDeskApp.Server.Persistance.UserEntity.Repositories;

public class UserEntityRepository : Repository<Models.UserEntity.Entities.UserEntity>, IUserEntityRepository
{
    private readonly ISession _session;
    private readonly IUnitOfWork _unitOfWork;

    public UserEntityRepository(ISession session, IUnitOfWork unitOfWork) : base(session, unitOfWork)
    {
        _session = session;
        _unitOfWork = unitOfWork;
    }

    public async Task<Models.UserEntity.Entities.UserEntity?> GetByEmailAsync(string email)
    {
        return await _session.Query<Models.UserEntity.Entities.UserEntity>()
            .Where(u => u.Email == email).FirstOrDefaultAsync();
    }
}