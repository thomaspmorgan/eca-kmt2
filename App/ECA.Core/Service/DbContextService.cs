using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class DbContextService<T> : ISaveable<T> where T : DbContext
    {
        /// <summary>
        /// Creates a new DbContextService with the given DbContext instance.
        /// </summary>
        /// <param name="context">The DbContext instance.</param>
        public DbContextService(T context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.Context = context;
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
        public int SaveChanges(IList<ISaveAction<T>> saveActions = null)
        {
            var list = GetSaveActions(saveActions);
            list.ForEach(x => x.BeforeSaveChanges(this.Context));
            var i = this.Context.SaveChanges();
            list.ForEach(x => x.AfterSaveChanges(this.Context));
            return i;
        }

        /// <summary>
        /// Saves the changes to underlying context.
        /// </summary>
        /// <param name="saveActions">The optional list of save actions.</param>
        /// <returns>The DbContext's save changes result.</returns>
        public async Task<int> SaveChangesAsync(IList<ISaveAction<T>> saveActions = null)
        {
            var list = GetSaveActions(saveActions);
            foreach(var saveAction in list)
            {
                await saveAction.BeforeSaveChangesAsync(this.Context);
            }
            var i = await this.Context.SaveChangesAsync();
            foreach (var saveAction in list)
            {
                await saveAction.AfterSaveChangesAsync(this.Context);
            }
            return i;
        }

        private List<ISaveAction<T>> GetSaveActions(IList<ISaveAction<T>> actions)
        {
            if (actions != null)
            {
                return actions.ToList();
            }
            else
            {
                return new List<ISaveAction<T>>();
            }
        }
    }
}
