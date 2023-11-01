using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.Repository.Infrastructure;

namespace Core.Repository.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IObjectState
    {
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void InsertOrUpdateGraph(TEntity entity);
        void InsertGraphRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        void SyncObject(TEntity entity, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> include);
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity> Query();
        IQueryable<TEntity> Queryable();
        IRepository<T> GetRepository<T>() where T : class, IObjectState;
        IEnumerable<TEntity> SelectPartial(int page, int pageSize, TEntity filter, out int totalCount);
    }
}
