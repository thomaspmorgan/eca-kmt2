using ECA.Business.Test.Service.Lookup;
using ECA.Core.Data;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test
{
    public class TestEcaContext : InMemoryEcaContext
    {
        public TestEcaContext()
        {
            this.LookupServiceTestDTOs = new TestDbSet<LookupServiceTestDTO>();

            //this will skip attempting to attach an entity to the context in the EcaService classes
            this.EntityStateToReturn = EntityState.Added;
            this.SetupActions = new List<Action>();
        }

        public int SaveChangesCalledCount { get; set; }

        public bool IsDisposed { get; set; }

        public EntityState EntityStateToReturn { get; set; }

        public Func<object> GetLocalDelegate { get; set; }

        public TestDbSet<LookupServiceTestDTO> LookupServiceTestDTOs { get; set; }

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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IsDisposed = true;
        }

        public override EntityState GetEntityState<T>(T x)
        {
            return this.EntityStateToReturn;
        }

        public override EntityState GetEntityState(object x)
        {
            return this.EntityStateToReturn;
        }

        public override T GetLocalEntity<T>(Func<T, bool> whereClause)
        {
            if (GetLocalDelegate != null)
            {
                return (T)GetLocalDelegate();
            }
            else
            {
                return null;
            }
        }
    }
}
