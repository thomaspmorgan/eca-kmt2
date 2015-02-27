using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    /// <summary>
    /// An ISaveAction is an action that be executed before and/or after a save changes are called on a service.
    /// </summary>
    /// <typeparam name="T">The DbContext type.</typeparam>
    public interface ISaveAction<T> where T : DbContext
    {
        /// <summary>
        /// Executed before save changes is called.
        /// </summary>
        void BeforeSaveChanges(T context);

        /// <summary>
        /// Executed before save changes async is called.
        /// </summary>
        Task BeforeSaveChangesAsync(T context);

        /// <summary>
        /// Executed after save changes is called.
        /// </summary>
        void AfterSaveChanges(T context);

        /// <summary>
        /// Executed after save changes async is called.
        /// </summary>
        Task AfterSaveChangesAsync(T context);
    }
}
