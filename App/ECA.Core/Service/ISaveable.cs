using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    /// <summary>
    /// An ISaveable service is a service capable of saving changes to a persistence store.
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Saves the changes to the underlying repository.
        /// </summary>
        /// <param name="saveActions">The save actions to execute.</param>
        /// <returns>The underlying datastore's save changes response.</returns>
        int SaveChanges(IList<ISaveAction> saveActions = null);

        /// <summary>
        /// Saves the changes to the underlying repository.
        /// </summary>
        /// <param name="saveActions">The save actions to execute.</param>
        /// <returns>The underlying datastore's save changes response.</returns>
        Task<int> SaveChangesAsync(IList<ISaveAction> saveActions = null);
    }
}
