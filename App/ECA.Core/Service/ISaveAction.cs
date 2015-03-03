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
    public interface ISaveAction
    {
        /// <summary>
        /// Executed before save changes is called.
        /// </summary>
        void BeforeSaveChanges(DbContext context);

        /// <summary>
        /// Executed before save changes async is called.
        /// </summary>
        Task BeforeSaveChangesAsync(DbContext context);

        /// <summary>
        /// Executed after save changes is called.
        /// </summary>
        void AfterSaveChanges(DbContext context);

        /// <summary>
        /// Executed after save changes async is called.
        /// </summary>
        Task AfterSaveChangesAsync(DbContext context);
    }
}
