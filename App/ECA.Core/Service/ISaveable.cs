using System.Threading.Tasks;

namespace ECA.Core.Service
{
    public interface ISaveable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
