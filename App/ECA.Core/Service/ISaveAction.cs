using System;
using System.Collections.Generic;
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
        void BeforeSaveChanges();

        /// <summary>
        /// Executed before save changes async is called.
        /// </summary>
        Task BeforeSaveChangesAsync();

        /// <summary>
        /// Executed after save changes is called.
        /// </summary>
        void AfterSaveChanges();

        /// <summary>
        /// Executed after save changes async is called.
        /// </summary>
        Task AfterSaveChangesAsync();
    }
}
