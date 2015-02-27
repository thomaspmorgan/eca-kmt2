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

        public int SaveChanges()
        {
            BeforeSaveChanges();
            var i = this.Context.SaveChanges();
            AfterSaveChanges();
            return i;
        }

        public async Task<int> SaveChangesAsync()
        {
            await BeforeSaveChangesAsync();
            var i = await this.Context.SaveChangesAsync();
            await AfterSaveChangesAsync();
            return i;
        }

        protected virtual void BeforeSaveChanges()
        {

        }

        protected virtual Task BeforeSaveChangesAsync()
        {
            return Task.FromResult<object>(null);
        }

        protected virtual void AfterSaveChanges()
        {

        }

        protected virtual Task AfterSaveChangesAsync()
        {
            return Task.FromResult<object>(null);
        }

        
    }
}
