using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    public class DbContextService<T> : ISaveable where T : DbContext
    {
        public DbContextService(T context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.Context = context;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected T Context { get; private set; }

        public int SaveChanges(IList<ISaveAction> saveActions = null)
        {
            var list = GetSaveActions(saveActions);
            list.ForEach(x => x.BeforeSaveChanges());
            var i = this.Context.SaveChanges();
            list.ForEach(x => x.AfterSaveChanges());
            return i;
        }

        public async Task<int> SaveChangesAsync(IList<ISaveAction> saveActions = null)
        {
            var list = GetSaveActions(saveActions);
            foreach(var saveAction in saveActions)
            {
                await saveAction.BeforeSaveChangesAsync();
            }
            var i = await this.Context.SaveChangesAsync();
            foreach (var saveAction in saveActions)
            {
                await saveAction.AfterSaveChangesAsync();
            }
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
    }
}
