using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Repository;
using Core.Repository.Infrastructure;
//using TLX.Core.Repository.Infrastructure;
using Core.Repository.Repositories;

namespace Core.Service
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields
        private readonly IRepositoryAsync<TEntity> _repository;
        #endregion Private Fields

        #region Constructor
        public Service(IRepositoryAsync<TEntity> repository) { _repository = repository; }
        #endregion Constructor

        public TEntity Find(params object[] keyValues) { return _repository.Find(keyValues); }

        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters) { return _repository.SelectQuery(query, parameters).AsQueryable(); }

        public void Insert(TEntity entity) { _repository.Insert(entity); }

        public void InsertRange(IEnumerable<TEntity> entities) { _repository.InsertRange(entities); }

        public void InsertOrUpdateGraph(TEntity entity) { _repository.InsertOrUpdateGraph(entity); }

        public void InsertGraphRange(IEnumerable<TEntity> entities) { _repository.InsertGraphRange(entities); }

        public void Update(TEntity entity) { _repository.Update(entity); }

        public void Delete(object id) { _repository.Delete(id); }

        public void Delete(TEntity entity) { _repository.Delete(entity); }

        public void DeleteRange(IEnumerable<TEntity> entities) { _repository.DeleteRange(entities); }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties) { _repository.Update(entity, properties); }

        public Repository.IQueryFluent<TEntity> Query() { return _repository.Query(); }

        public Repository.IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject) { return _repository.Query(queryObject); }

        public Repository.IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query) { return _repository.Query(query); }

        public async Task<TEntity> FindAsync(params object[] keyValues) { return await _repository.FindAsync(keyValues); }

        public async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues) { return await _repository.FindAsync(cancellationToken, keyValues); }

        public async Task<bool> DeleteAsync(params object[] keyValues) { return await DeleteAsync(CancellationToken.None, keyValues); }

        //IF 04/08/2014 - Before: return await DeleteAsync(cancellationToken, keyValues);
        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues) { return await _repository.DeleteAsync(cancellationToken, keyValues); }

        public IQueryable<TEntity> Queryable() { return _repository.Queryable(); }
        public IEnumerable<TEntity> GetPartial(int page, int pageSize, TEntity filter, out int totalCount)
        {
            return _repository.SelectPartial(page, pageSize, filter, out totalCount);
        }

    }
}
