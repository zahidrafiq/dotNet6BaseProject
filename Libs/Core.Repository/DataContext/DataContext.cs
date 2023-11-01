using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Repository.Infrastructure;

namespace Core.Repository.DataContext
{
    public class DataContext : DbContext, IDataContextAsync
    {
        #region Private Fields
        private readonly Guid _instanceId;
        bool _disposed;
        #endregion Private Fields

        //public DataContext(string nameOrConnectionString)
        //    : base(nameOrConnectionString)

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
            _instanceId = Guid.NewGuid();
        }

        protected DataContext(DbContextOptions options)
        : base(options)
        {
            _instanceId = Guid.NewGuid();

            // Mohsin: In EF6, DataContext.Configuration property has been removed.

            //Lazy loading is not yet supported by EF Core. So it means its already disabled as not supported yet.
            //Configuration.LazyLoadingEnabled = false;

            //Mohsin - Proxy creation doesn't exist in EF Core by default. You have to install Microsoft.EntityFrameworkCore.Proxies to use it. 
            //So there is no real need to disable it (since you can just not install that package)

            //Configuration.ProxyCreationEnabled = false;
        }

        
        //public DataContext(DbConnection existingConnection, bool contextOwnsConnection)
        //    : base(existingConnection, contextOwnsConnection)
        //{
        //    _instanceId = Guid.NewGuid();
        //    Configuration.LazyLoadingEnabled = false;
        //    Configuration.ProxyCreationEnabled = false;
        //}

        public Guid InstanceId { get { return _instanceId; } }

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChanges"/>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            try
            {
                SyncObjectsStatePreCommit();
                var changes = base.SaveChanges();
                SyncObjectsStatePostCommit();
                return changes;
            }
            catch (ValidationException e)
            {
                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                e.ValidationAttribute.ErrorMessageResourceName, e.ValidationAttribute.ErrorMessage);
                throw;
            }
            // Mohsin : in EF Core, DbEntityValidationException has been removed. Instead ValidationExcpetion is thrown using Validator in SyncObjectsStatePreCommit() function

            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}



        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChangesAsync"/>
        /// <returns>A task that represents the asynchronous save operation.  The 
        ///     <see cref="Task.Result">Task.Result</see> contains the number of 
        ///     objects written to the underlying database.</returns>
        public /*override*/ async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }
        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChangesAsync"/>
        /// <returns>A task that represents the asynchronous save operation.  The 
        ///     <see cref="Task.Result">Task.Result</see> contains the number of 
        ///     objects written to the underlying database.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                //SyncObjectsStatePreCommit();
                var changesAsync = await base.SaveChangesAsync(cancellationToken);
                //SyncObjectsStatePostCommit();
                return changesAsync;
            }
            catch (ValidationException e)
            {
                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                e.ValidationAttribute.ErrorMessageResourceName, e.ValidationAttribute.ErrorMessage);
                throw;
            }
}

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        public void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                if (dbEntityEntry.State != StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState))
                {
                    dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
                }

                // Validation of entities . It would throw ValidationException if some failure occurs
                var validationContext = new ValidationContext(dbEntityEntry);
                Validator.ValidateObject(dbEntityEntry, validationContext);
            }
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }

        public override void Dispose()
        {
            bool disposing = false; // moved parameter here as base class virtual function does not take input parameter
            if (!_disposed)
            {
                if (disposing)
                {
                    // free other managed objects that implement
                    // IDisposable only
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(); // removed disposing parameter as DBContext does not 
        }
    }
}
