using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    /// <summary>
    /// An ISaveable service is a service capable of saving changes to a persistence store.
    /// </summary>
    public interface ISaveable
    {
        int SaveChanges(IList<ISaveAction> saveActions = null);

        Task<int> SaveChangesAsync(IList<ISaveAction> saveActions = null);
    }
}
