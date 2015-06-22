using ECA.Core.Data;
using ECA.Core.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    /// <summary>
    /// A DbContextService is a service that utitlizes an Entity Framework DbContext and provides some additional functionality 
    /// around saving changes.
    /// </summary>
    /// <typeparam name="T">The DbContext type.</typeparam>
    public class DbContextService<T> : IDisposable, ISaveable where T : DbContext
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly List<ISaveAction> saveActions;

        /// <summary>
        /// Creates a new DbContextService with the given DbContext instance.
        /// </summary>
        /// <param name="context">The DbContext instance.</param>
        /// <param name="saveActions">Save actions for before and after the context is saved.</param>
        public DbContextService(T context, List<ISaveAction> saveActions = null)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.Context = context;
            this.saveActions = saveActions ?? new List<ISaveAction>();
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected T Context { get; private set; }

        /// <summary>
        /// Saves the changes to underlying context.
        /// </summary>
        /// <returns>The DbContext's save changes result.</returns>
        public int SaveChanges()
        {
            var stopWatch = Stopwatch.StartNew();
            this.saveActions.ForEach(x => x.BeforeSaveChanges(this.Context));            
            //var errors = this.Context.GetValidationErrors();
            int i = -1;
            try
            {
                i = this.Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw HandleDbUpdateConcurrencyException(ex);
            }
            this.saveActions.ForEach(x => x.AfterSaveChanges(this.Context));
            stopWatch.Stop();
            logger.Trace("Saved changes in DbContextService.");
            return i;
        }

        /// <summary>
        /// Saves the changes to underlying context.
        /// </summary>
        /// <returns>The DbContext's save changes result.</returns>
        public async Task<int> SaveChangesAsync()
        {
            var stopWatch = Stopwatch.StartNew();
            foreach (var saveAction in this.saveActions)
            {
                await saveAction.BeforeSaveChangesAsync(this.Context);
            }
            //var errors = this.Context.GetValidationErrors();
            int i = -1;
            try
            {
                i = await this.Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw HandleDbUpdateConcurrencyException(ex);
            }
            foreach (var saveAction in this.saveActions)
            {
                await saveAction.AfterSaveChangesAsync(this.Context);
            }
            logger.Trace("Saved changes async in DbContextService.");
            return i;
        }

        private EcaDbUpdateConcurrencyException HandleDbUpdateConcurrencyException(DbUpdateConcurrencyException ex)
        {
            var concurrentEntities = new List<IConcurrentEntity>();
            foreach (var entry in ex.Entries)
            {
                Contract.Assert(entry.Entity is IConcurrentEntity, "The entity must be an IConcurrent entity.");
                var iConcurrent = entry.Entity as IConcurrentEntity;
                concurrentEntities.Add(iConcurrent);
            }
            var likeEntityIds = from concurrentEntity in concurrentEntities
                                group concurrentEntity by new { Id = concurrentEntity.GetId() } into g
                                select new { Id = g.Key, Count = g.Count() };
            if (likeEntityIds.Where(x => x.Count > 1).Count() > 0)
            {
                throw new NotSupportedException("There are multiple entities with concurrency issues with the same Id.  The system will be unable to automatically reconcile concurrent issues.");
            }
            return new EcaDbUpdateConcurrencyException(ex.Message, ex)
            {
                ConcurrentEntities = concurrentEntities
            };
        }

        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Context != null)
                {
                    this.Context.Dispose();
                    this.Context = null;
                }
            }
        }

        #endregion
    }
}
