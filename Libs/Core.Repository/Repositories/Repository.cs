using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Repository.DataContext;
using Core.Repository.Infrastructure;
using Core.Repository.UnitOfWork;

namespace Core.Repository.Repositories
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields

        private readonly IDataContextAsync _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;

        #endregion Private Fields

        public Repository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;

            // Temporarily for FakeDbContext, Unit Test and Fakes
            var dbContext = context as DbContext;

            if (dbContext != null)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
            else
            {
                var fakeContext = context as FakeDbContext;

                if (fakeContext != null)
                {
                    _dbSet = fakeContext.Set<TEntity>();
                }
            }
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            // Mohsin - Use Query Type in EF 2.1 for executing RAW SQL Queries

            return null;// _dbSet.SqlQuery(query, parameters).AsQueryable(); // For compiling only
        }

        public virtual void Insert(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;
            _dbSet.Add(entity);
            _context.SyncObjectState(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Add(entity);

            _context.SyncObjectState(entity);
        }

        public virtual void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {

            entity.ObjectState = ObjectState.Modified;
            var dbContext = _context as DbContext;
            dbContext.Set<TEntity>().Attach(entity);
            EntityEntry<TEntity> entry = dbContext.Entry(entity);
            // Mohsin : Changed DBEntityEntry to EntityEntry. DBEntityEntry has been removed in EF Core. Check if functionality has been changed
            //DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
            foreach (var selector in properties)
            {
                entry.Property(selector).IsModified = true;
            }

            // Mohsin - Validation does not exist in EF Core, so no need to disable it.
            //dbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        public virtual void SyncObject(TEntity entity, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> include)
        {
            TEntity obj = _dbSet.AsQueryable().Where(where).Include(include).FirstOrDefault();
            var dbContext = _context as DbContext;
            // Mohsin : Changed DBEntityEntry to EntityEntry. DBEntityEntry has been removed in EF Core. Check if functionality has been changed
            EntityEntry<TEntity> entry = dbContext.Entry(obj);
            //DbEntityEntry<TEntity> entry = dbContext.Entry(obj);
            entry.CurrentValues.SetValues(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return new QueryFluent<TEntity>(this, queryObject);
        }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public IRepository<T> GetRepository<T>() where T : class, IObjectState
        {
            return _unitOfWork.Repository<T>();
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);

            return true;
        }

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                // Mohsin - Added LinqKit.Microsoft.EntityFrameworkCore package as dependency for TLX.Core.Repository
                query = query.AsExpandable().Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public virtual IEnumerable<TEntity> SelectPartial(int page, int pageSize, TEntity filter, out int totalCount)
        {
            IQueryFluent<TEntity> query;


            if (filter != null)
            {
                query = Query(GetFilterExpressionFromModel(filter));
            }
            else
            {
                query = Query();
            }
            ParameterExpression pe = Expression.Parameter(typeof(TEntity));
            PropertyInfo prop = typeof(TEntity).GetProperty(typeof(TEntity).Name.Substring(4) + "_Id");
            Expression orderById = Expression.Property(pe, prop);
            Expression<Func<TEntity, int>> exp = Expression.Lambda<Func<TEntity, int>>(orderById, new ParameterExpression[] { pe });
            query = query.OrderBy(a => a.OrderBy(exp));

            return query.SelectPage(page, pageSize, out totalCount);
        }

        public virtual void InsertOrUpdateGraph(TEntity entity)
        {
            //SyncObjectGraph(entity);
            //_entitesChecked = null;
            //_dbSet.Attach(entity);
            _dbSet.Add(entity);
            _context.SyncObjectState(entity);
        }

        HashSet<object> _entitesChecked; // tracking of all process entities in the object graph when calling SyncObjectGraph

        private void SyncObjectGraph(object entity) // scan object graph for all 
        {
            if (_entitesChecked == null)
                _entitesChecked = new HashSet<object>();

            if (_entitesChecked.Contains(entity))
                return;

            _entitesChecked.Add(entity);

            var objectState = entity as IObjectState;

            if (objectState != null && objectState.ObjectState == ObjectState.Added)
                _context.SyncObjectState((IObjectState)entity);

            // Set tracking state for child collections
            foreach (var prop in entity.GetType().GetProperties())
            {
                // Apply changes to 1-1 and M-1 properties
                var trackableRef = prop.GetValue(entity, null) as IObjectState;
                if (trackableRef != null)
                {
                    if (trackableRef.ObjectState == ObjectState.Added)
                        _context.SyncObjectState((IObjectState)entity);

                    SyncObjectGraph(prop.GetValue(entity, null));
                }

                // Apply changes to 1-M properties
                var items = prop.GetValue(entity, null) as IEnumerable<IObjectState>;
                if (items == null) continue;

                Debug.WriteLine("Checking collection: " + prop.Name);

                foreach (var item in items)
                    SyncObjectGraph(item);
            }
        }
        private Expression<Func<TEntity, bool>> GetFilterExpressionFromModel(TEntity entity)
        {
            ParameterExpression pe = Expression.Parameter(typeof(TEntity));
            Expression joinedExpression = null;

            PropertyInfo[] propInfos = typeof(TEntity).GetProperties();
            foreach (var item in propInfos)
            {
                object itemObj = item.GetValue(entity);
                if (itemObj != null && itemObj.ToString() != "0" && (itemObj.GetType().Name.Equals("Int32") || itemObj.GetType().Name.Equals("String")))
                {
                    Expression left = Expression.Property(pe, item);
                    Expression right = Expression.Constant(item.GetValue(entity), item.GetValue(entity).GetType());
                    Expression exp = Expression.Equal(left, right);
                    joinedExpression = joinedExpression != null ? Expression.And(joinedExpression, exp) : exp;
                }
            }

            return Expression.Lambda<Func<TEntity, bool>>(joinedExpression, new ParameterExpression[] { pe });
        }

        public IEnumerable<object> Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
