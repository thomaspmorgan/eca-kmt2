using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    public interface ISaveable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
