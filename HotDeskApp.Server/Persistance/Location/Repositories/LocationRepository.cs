using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Location.Repositories;
using ISession = NHibernate.ISession;

namespace HotDeskApp.Server.Persistance.Location.Repositories;

public class LocationRepository : Repository<Models.Location.Entities.Location>, ILocationRepository
{
    private readonly ISession _session;
    private readonly IUnitOfWork _unitOfWork;

    public LocationRepository(ISession session, IUnitOfWork unitOfWork) : base(session, unitOfWork)
    {
        _session = session;
        _unitOfWork = unitOfWork;
    }
}