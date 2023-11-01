using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Core.Repository.Infrastructure;
using Core.Repository.Repositories;

namespace Core.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
    }
}
