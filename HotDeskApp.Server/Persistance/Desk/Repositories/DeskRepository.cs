using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Desk.Repositories;
using ISession = NHibernate.ISession;

namespace HotDeskApp.Server.Persistance.Desk.Repositories;

public class DeskRepository : Repository<Models.Desk.Entities.Desk>, IDeskRepository
{
    private readonly ISession _session;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeskRepository(ISession session, IUnitOfWork unitOfWork) : base(session, unitOfWork)
    {
        _session = session;
        _unitOfWork = unitOfWork;
    }
}