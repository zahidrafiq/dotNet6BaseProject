using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.Repository.Infrastructure;

namespace Core.Repository
{
    public abstract class FakeDbSet<TEntity> : DbSet<TEntity>/*, IDbSet<TEntity> */where TEntity : Entity, new()
    {
        #region Private Fields
        private readonly ObservableCollection<TEntity> _items;
        private readonly IQueryable _query;
        #endregion Private Fields

        protected FakeDbSet()
        {
            _items = new ObservableCollection<TEntity>();
            _query = _items.AsQueryable();
        }

        //IEnumerator IEnumerable.GetEnumerator() { return _items.GetEnumerator(); }
        public IEnumerator<TEntity> GetEnumerator() { return _items.GetEnumerator(); }

        public Expression Expression { get { return _query.Expression; } }

        public Type ElementType { get { return _query.ElementType; } }

        public IQueryProvider Provider { get { return _query.Provider; } }

        public override EntityEntry<TEntity> Add(TEntity entity)
        {
            _items.Add(entity);
            //return  entity;
            return null;
        }

        public override EntityEntry<TEntity> Remove(TEntity entity)
        {
            _items.Remove(entity);
            //return entity;
            return null;
        }

        public override EntityEntry<TEntity> Attach(TEntity entity)
        {
            switch (entity.ObjectState)
            {
                case ObjectState.Modified:
                    _items.Remove(entity);
                    _items.Add(entity);
                    break;

                case ObjectState.Deleted:
                    _items.Remove(entity);
                    break;

                case ObjectState.Unchanged:
                case ObjectState.Added:
                    _items.Add(entity);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            //return entity;
            return null;
        }

        //public override EntityEntry<TEntity> Create() { return new TEntity(); }

        //public override TDerivedEntity Create<TDerivedEntity>() { return Activator.CreateInstance<TDerivedEntity>(); }

        //public override ObservableCollection<TEntity> Local { get { return _items; } }
    }
}
