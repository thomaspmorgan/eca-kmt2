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

        /// <summary>
        /// Creates a new DbContextService with the given DbContext instance.
        /// </summary>
        /// <param name="context">The DbContext instance.</param>
        public DbContextService(T context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.Context = context;
            this.Context.Database.Log = (log) => logger.Trace(log);
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected T Context { get; private set; }

        /// <summary>
        /// Saves the changes to underlying context.
        /// </summary>
        /// <param name="saveActions">The optional list of save actions.</param>
        /// <returns>The DbContext's save changes result.</returns>
        public int SaveChanges(IList<ISaveAction> saveActions = null)
        {
            var stopWatch = Stopwatch.StartNew();
            var list = GetSaveActions(saveActions);
            list.ForEach(x => x.BeforeSaveChanges(this.Context));            
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
            list.ForEach(x => x.AfterSaveChanges(this.Context));
            stopWatch.Stop();
            logger.Trace("Saved changes in DbContextService.");
            return i;
        }

        /// <summary>
        /// Saves the changes to underlying context.
        /// </summary>
        /// <param name="saveActions">The optional list of save actions.</param>
        /// <returns>The DbContext's save changes result.</returns>
        public async Task<int> SaveChangesAsync(IList<ISaveAction> saveActions = null)
        {
            var stopWatch = Stopwatch.StartNew();
            var list = GetSaveActions(saveActions);
            foreach(var saveAction in list)
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
            foreach (var saveAction in list)
            {
                await saveAction.AfterSaveChangesAsync(this.Context);
            }
            logger.Trace("Saved changes async in DbContextService.");
            return i;
        }

        private List<ISaveAction> GetSaveActions(IList<ISaveAction> actions)
        {
            if (actions != null)
            {
                return actions.ToList();
            }
            else
            {
                return new List<ISaveAction>();
            }
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
