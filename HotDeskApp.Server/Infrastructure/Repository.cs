using Microsoft.EntityFrameworkCore;

namespace HotDeskApp.Server.Infrastructure
{
    using NHibernate;

    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly ISession _session;
        private readonly IUnitOfWork _unitOfWork;

        public Repository(ISession session, IUnitOfWork unitOfWork)
        {
            _session = session;
            _unitOfWork = unitOfWork;
        }

        
        public async Task<IEnumerable<T>> GetAll()
        {
            return await Task.Run(() => _session.Query<T>().ToList());
        }
       
        
        public async Task<T> Get(Guid id)
        {
            return await _session.GetAsync<T>(id);
        }

        public async Task<Guid> Save(T entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                await _session.SaveAsync(entity);
                _unitOfWork.Commit();
                return entity.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task Update(T entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                await _session.UpdateAsync(entity);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _session.GetAsync<T>(id);

                if (entity != null)
                {
                    await _session.DeleteAsync(entity);
                    _unitOfWork.Commit();
                }
                else
                {
                    _unitOfWork.Rollback();
                    throw new Exception($"Entity with Id {id} does not exist.");
                }
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        
    }
}
