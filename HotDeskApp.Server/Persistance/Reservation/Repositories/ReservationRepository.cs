using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Reservation.Repositories;
using ISession = NHibernate.ISession;

namespace HotDeskApp.Server.Persistance.Reservation.Repositories;

public class ReservationRepository : Repository<Models.Reservation.Entities.Reservation>, IReservationRepository
{
    private readonly ISession _session;
    private readonly IUnitOfWork _unitOfWork;

    public ReservationRepository(ISession session, IUnitOfWork unitOfWork) : base(session, unitOfWork)
    {
        _session = session;
        _unitOfWork = unitOfWork;
    }
}