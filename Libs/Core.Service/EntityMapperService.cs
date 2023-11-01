using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.Repository.DataContext;
using Core.Repository.Infrastructure;
using Core.Repository.Repositories;
using Core.Repository.UnitOfWork;

namespace Core.Service
{
    public abstract class EntityMapperService<TEntity, UEntity>
        where TEntity : class, IServiceModel<UEntity, TEntity>, IObjectState, new()
        where UEntity : class, IObjectState, new()
    {
        #region Private Fields

        private IRepositoryAsync<UEntity> _repository;
        private TEntity temp;
        private UEntity Utemp;
        private Service<UEntity> _service;
        protected UnitOfWork uow;

        // for unit testing 
        public UnitOfWork UOW
        {
            get { return uow; } 
        }

        #endregion Private Fields

        #region Constructor

        protected EntityMapperService(IDataContextAsync context, UnitOfWork uow)
        {
            Initialize(context);
        }

        protected EntityMapperService()
        {
            _service = new Service<UEntity>(_repository);
        }

        #endregion Constructor

        public virtual TEntity Find(params object[] keyValues)
        {
            return transform(_service.Find(keyValues));
        }

        //public virtual TEntity GetAll() {return _repository.g }
        // public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters) { return _repository.SelectQuery(query, parameters).AsQueryable(); }

        public virtual void Insert(TEntity entity)
        {
            Utemp = transform(entity);
            _service.Insert(Utemp);
        }

        public virtual TEntity Get()
        {
            return transform(Utemp);
        }


        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            _service.InsertRange(transform(entities));
        }

        public virtual void InsertOrUpdateGraph(TEntity entity)
        {
            _service.InsertOrUpdateGraph(transform(entity));
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            _service.InsertGraphRange(transform(entities));
        }

        public virtual void Update(TEntity entity)
        {
            _service.Update(transform(entity));
        }
        public virtual void Activate(int id)
        {
            UEntity uEntity = new UEntity();
            string idColumn = typeof(UEntity).GetProperty(typeof(UEntity).Name.Substring(4) + "_Id").Name;
            uEntity.GetType().GetProperty(idColumn).SetValue(uEntity, id);
            uEntity.GetType().GetProperty("Effective_Start_Date").SetValue(uEntity, DateTime.UtcNow);
            uEntity.GetType().GetProperty("Effective_End_Date").SetValue(uEntity, Convert.ToDateTime("12/30/2099"));
            _service.Update(uEntity, GetPropertyExpression("Effective_Start_Date"), GetPropertyExpression("Effective_End_Date"));
        }
        public virtual void Inactive(int id)
        {
            UEntity uEntity = new UEntity();
            string idColumn = typeof(UEntity).GetProperty(typeof(UEntity).Name.Substring(4) + "_Id").Name;
            uEntity.GetType().GetProperty(idColumn).SetValue(uEntity, id);
            uEntity.GetType().GetProperty("Effective_End_Date").SetValue(uEntity, DateTime.UtcNow);
            _service.Update(uEntity, GetPropertyExpression("Effective_End_Date"));
        }

        public virtual void Inactive(Expression<Func<UEntity, bool>> where)
        {
            IEnumerable<UEntity> UData = _service.Queryable().Where(where).AsEnumerable();
            foreach (UEntity entity in UData)
            {
                entity.GetType().GetProperty("Effective_End_Date").SetValue(entity, DateTime.UtcNow);
                _service.Update(entity, GetPropertyExpression("Effective_End_Date"));
            }

        }

        private Expression<Func<UEntity, object>> GetPropertyExpression(string property)
        {
            Type uType = typeof(UEntity);
            ParameterExpression pe = Expression.Parameter(uType);

            Expression exp = Expression.Property(pe, property);

            if (uType.IsGenericType && uType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Expression<Func<UEntity, DateTime?>> original = Expression.Lambda<Func<UEntity, DateTime?>>(exp, new ParameterExpression[] { pe });
                return Expression.Lambda<Func<UEntity, object>>(Expression.Convert(original.Body, typeof(object)), original.Parameters);
            }
            else
            {
                Expression<Func<UEntity, DateTime>> original = Expression.Lambda<Func<UEntity, DateTime>>(exp, new ParameterExpression[] { pe });
                return Expression.Lambda<Func<UEntity, object>>(Expression.Convert(original.Body, typeof(object)), original.Parameters);
            }
        }

        public virtual void Delete(object id)
        {
            _service.Delete(id);
        }

        public virtual IQueryable<UEntity> Queryable()
        {
            return _service.Queryable();
        }

        public virtual Repository.IQueryFluent<UEntity> Query()
        {
            return _service.Query();
        }

        public virtual void Delete(TEntity entity)
        {
            _service.Delete(transform(entity));
        }

        public virtual void Delete(Expression<Func<UEntity, bool>> where)
        {
            IEnumerable<UEntity> UData = _service.Queryable().Where(where).AsEnumerable();
            _service.DeleteRange(UData);
        }

        protected void Initialize(IDataContextAsync dataContext)
        {
            uow = new UnitOfWork(dataContext);
            _repository = new Repository<UEntity>(dataContext, uow);
            _service = new Service<UEntity>(_repository);

        }

        //Transformation functions
        IEnumerable<UEntity> transform(IEnumerable<TEntity> entities)
        {
            var transformedEntities = new List<UEntity>();
            foreach (TEntity entity in entities)
            {
                transformedEntities.Add(entity.ToDataModel(entity));
            }
            return transformedEntities;
        }

        UEntity transform(TEntity entity)
        {

            return entity.ToDataModel(entity);
        }

        TEntity transform(UEntity entity)
        {
            temp = new TEntity();
            return temp.FromDataModel(entity);
        }

        IEnumerable<TEntity> transform(IEnumerable<UEntity> entities)
        {
            var transformedEntities = new List<TEntity>();
            foreach (UEntity entity in entities)
            {
                temp = new TEntity();
                temp.FromDataModel(entity);
                transformedEntities.Add(temp);
            }
            return transformedEntities;
        }
        public IEnumerable<TEntity> GetAll()
        {
            IEnumerable<UEntity> UData = _service.Queryable().AsEnumerable();
            TEntity TModel = new TEntity();
            return TModel.FromDataModelList(UData);

        }
        public IEnumerable<TEntity> GetPartial(int page, int pageSize, TEntity filter, out int totalCount, out int totalPages)
        {
            UEntity UObj = null;
            if (filter != null)
            {
                UObj = filter.ToDataModel(filter);
            }

            IEnumerable<UEntity> UData = _service.GetPartial(page, pageSize, UObj, out totalCount);

            totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            TEntity TModel = new TEntity();
            return TModel.FromDataModelList(UData);

        }
    }
}
