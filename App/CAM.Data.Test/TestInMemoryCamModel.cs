using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Data.Test
{
    public class TestInMemoryCamModel : InMemoryCamModel
    {
        public int SaveChangesCount { get; set; }

        public int SaveChangesAsyncCount { get; set; }

        public override int SaveChanges()
        {
            SaveChangesCount++;
            return SaveChangesCount;
        }

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            SaveChangesAsyncCount++;
            return Task.FromResult<int>(SaveChangesAsyncCount);
        }
    }
}
