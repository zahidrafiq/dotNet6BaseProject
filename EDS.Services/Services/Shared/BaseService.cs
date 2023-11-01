using EDS.Common.Utilities;
using EDS.DB.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repository;
using Core.Repository.Infrastructure;
using Core.Repository.Repositories;
using Core.Repository.UnitOfWork;
using Core.Service;

namespace EDS.Services.Services.Shared
{
    public class BaseService<TEntity, UEntity> : EntityMapperService<TEntity, UEntity>
   where TEntity : class, IServiceModel<UEntity, TEntity>, IObjectState, new()
   where UEntity : class, IObjectState, new()
    {
        protected readonly SerializationContext _context;
        protected readonly TenantConfig _currentTenant;
        //protected readonly UserClaims _claims;

        public BaseService(UnitOfWork uow)
            : base(uow.DataContext, uow)
        {
        }

        public BaseService(TenantConfig currentTenant)
        {
            _currentTenant = currentTenant;
            _context = new SerializationContext(currentTenant);
            base.Initialize(_context);

        }

        public BaseService(SerializationContext context)
        {
            _context = context;
            base.Initialize(_context);
        }

        public static List<T> Except<T, TKey>(List<T> items, List<T> exceptItems, Func<T, TKey> getKey)
        {
            var exceptKeys = exceptItems.Select(getKey);

            return items.Where(i => !exceptKeys.Contains(getKey(i))).ToList();
        }

        public bool UpdateNestedCollection<T, TKey, N, Nkey>(List<T> existing, List<T> input, Func<T, TKey> getKey, Func<T, N> getNestedToUpdate, Func<N, Nkey> getNestedKey, out List<T> added, out List<T> modified, int? min = null, int? max = null) where T : IObjectState where N : IObjectState
        {
            added = Except(input, existing, getKey);

            var deleted = Except(existing, input, getKey);

            modified = Except(input, added, getKey);

            if (max.HasValue)
            {
                if (added.Count() + existing.Count() - deleted.Count() > max)
                {
                    return false;
                }
            }

            if (min.HasValue)
            {
                if (existing.Count() + added.Count() - deleted.Count() < min)
                {
                    return false;
                }
            }

            foreach (var x in added)
            {
                x.ObjectState = ObjectState.Added;

                if (getNestedToUpdate != null)
                {
                    getNestedToUpdate(x).ObjectState = ObjectState.Added;
                }
            }

            foreach (var x in modified)
            {
                x.ObjectState = ObjectState.Modified;

                if (getNestedToUpdate != null)
                {
                    getNestedToUpdate(x).ObjectState = ObjectState.Modified;
                }
            }

            foreach (var x in deleted)
            {
                var child = this;

                ((dynamic)Activator.CreateInstance(typeof(Repository<>).MakeGenericType(typeof(T).BaseType.GetGenericArguments()[0]), child.uow.DataContext, child.uow)).Delete(getKey(x));

                if (getNestedToUpdate != null)
                {
                    ((dynamic)Activator.CreateInstance(typeof(Repository<>).MakeGenericType(typeof(N).BaseType.GetGenericArguments()[0]), child.uow.DataContext, child.uow)).Delete(getNestedKey(getNestedToUpdate(x)));

                    //probably unnecessary, check and remove
                    typeof(N).GetProperty("ObjectState").SetValue(getNestedToUpdate(x), ObjectState.Deleted);
                }
            }

            return true;
        }

        public bool UpdateNestedCollection<T, TKey>(List<T> existing, List<T> input, Func<T, TKey> getKey, out List<T> added, out List<T> modified, out List<T> deleted, int? min = null, int? max = null)
            where T : IObjectState
        {
            added = Except(input, existing, getKey);

            deleted = Except(existing, input, getKey);

            modified = Except(input, added, getKey);

            if (max.HasValue)
            {
                if (added.Count() + existing.Count() - deleted.Count() > max)
                {
                    return false;
                }
            }

            if (min.HasValue)
            {
                if (existing.Count() + added.Count() - deleted.Count() < min)
                {
                    return false;
                }
            }

            foreach (var x in added)
            {
                x.ObjectState = ObjectState.Added;
            }

            foreach (var x in modified)
            {
                x.ObjectState = ObjectState.Modified;
            }

            foreach (var x in deleted)
            {
                var child = this;

                ((dynamic)Activator.CreateInstance(typeof(Repository<>).MakeGenericType(typeof(T).BaseType.GetGenericArguments()[0]), child.uow.DataContext, child.uow)).Delete(getKey(x));
            }

            return true;
        }

    }

}
