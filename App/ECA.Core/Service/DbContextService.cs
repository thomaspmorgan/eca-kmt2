﻿using ECA.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private static readonly string COMPONENT_NAME = typeof(DbContextService<>).FullName;

        private ILogger logger;

        /// <summary>
        /// Creates a new DbContextService with the given DbContext instance.
        /// </summary>
        /// <param name="context">The DbContext instance.</param>
        public DbContextService(T context, ILogger logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.Context = context;
            this.logger = logger;
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
            var errors = this.Context.GetValidationErrors();
            var i = this.Context.SaveChanges();
            list.ForEach(x => x.AfterSaveChanges(this.Context));
            stopWatch.Stop();
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
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
            var errors = this.Context.GetValidationErrors();
            var i = await this.Context.SaveChangesAsync();
            foreach (var saveAction in list)
            {
                await saveAction.AfterSaveChangesAsync(this.Context);
            }
            logger.TraceApi(COMPONENT_NAME, stopWatch.Elapsed);
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
                this.Context.Dispose();
                this.Context = null;
            }
        }

        #endregion
    }
}
