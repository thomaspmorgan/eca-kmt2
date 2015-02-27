using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    /// <summary>
    /// An ISaveable service is a service capable of saving changes to a persistence store.
    /// </summary>
    public interface ISaveable<T> where T : DbContext
    {
        int SaveChanges(IList<ISaveAction<T>> saveActions = null);

        Task<int> SaveChangesAsync(IList<ISaveAction<T>> saveActions = null);
    }
}
