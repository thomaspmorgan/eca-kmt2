using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test
{
    public class TestEcaContext : EcaContext
    {
        public int SaveChangesCalledCount { get; set; }

        public override int SaveChanges()
        {
            this.SaveChangesCalledCount++;
            return this.SaveChangesCalledCount;
        }

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            this.SaveChangesCalledCount++;
            return Task.FromResult<int>(this.SaveChangesCalledCount);
        }
    }
}
