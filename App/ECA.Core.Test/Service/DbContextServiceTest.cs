using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Service;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System.Reflection;
using ECA.Core.Data;
using Moq;
using System.Data.Entity.Infrastructure;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Core.Exceptions;

namespace ECA.Core.Test.Service
{
    public class SampleSaveAction : ISaveAction
    {

        public int BeforeSaveChangesCount { get; set; }
        public int AfterSaveChangesCount { get; set; }
        public int BeforeSaveChangesCountAsync { get; set; }
        public int AfterSaveChangesCountAsync { get; set; }

        public DbContext GivenContext { get; set; }

        public void BeforeSaveChanges(DbContext context)
        {
            this.BeforeSaveChangesCount++;
            this.GivenContext = context;
        }

        public System.Threading.Tasks.Task BeforeSaveChangesAsync(DbContext context)
        {
            this.BeforeSaveChangesCountAsync++;
            this.GivenContext = context;
            return Task.FromResult<object>(null);
        }

        public void AfterSaveChanges(DbContext context)
        {
            this.AfterSaveChangesCount++;
            this.GivenContext = context; ;
        }

        public System.Threading.Tasks.Task AfterSaveChangesAsync(DbContext context)
        {
            this.AfterSaveChangesCountAsync++;
            this.GivenContext = context;
            return Task.FromResult<object>(null);
        }
    }

    public class SampleDbContext : DbContext
    {
        public bool IsDisposed { get; set; }

        public bool SaveChangesCalled { get; set; }

        public bool SaveChangesAsyncCalled { get; set; }

        public override int SaveChanges()
        {
            SaveChangesCalled = true;
            return 1;
        }

        public override Task<int> SaveChangesAsync()
        {
            SaveChangesAsyncCalled = true;
            return Task.FromResult<int>(1);
        }

        protected override void Dispose(bool disposing)
        {
            this.IsDisposed = true;
        }
    }

    public class ConcurrentDbContext : DbContext
    {
        //public bool IsDisposed { get; set; }

        //public bool SaveChangesCalled { get; set; }

        //public bool SaveChangesAsyncCalled { get; set; }

        //public override int SaveChanges()
        //{
        //    SaveChangesCalled = true;
        //    return 1;
        //}

        //public override Task<int> SaveChangesAsync()
        //{
        //    SaveChangesAsyncCalled = true;
        //    return Task.FromResult<int>(1);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    this.IsDisposed = true;
        //}
    }

    public class ConcurrentEntity : IConcurrentEntity
    {
        public int Id { get; set; }

        public byte[] RowVersion
        {
            get;
            set;
        }

        public int GetId()
        {
            return this.Id;
        }
    }

    public class SampleService : DbContextService<SampleDbContext>
    {
        public SampleService(SampleDbContext context) : base(context) { }

        public TestDbSet<ConcurrentEntity> ConcurrentEntities { get; set; }
    }

    //public class ConcurrencyTestDbContext : DbContext
    //{
    //    public ConcurrencyTestDbContext()
    //    {
    //        this.ConcurrentEntities = new TestDbSet<ConcurrentEntity>();
    //    }

    //    public TestDbSet<ConcurrentEntity> ConcurrentEntities { get; set; }

    //}

    [TestClass]
    public class DbContextServiceTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Database.SetInitializer<SampleDbContext>(null);
            Database.SetInitializer<ConcurrentDbContext>(null);
        }

        #region Save Changes

        [TestMethod]
        public void TestSaveChanges()
        {
            var saveAction = new SampleSaveAction();
            var context = new SampleDbContext();
            var service = new DbContextService<SampleDbContext>(context, new List<ISaveAction> { saveAction });
            
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCount);
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCountAsync);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCountAsync);
            Assert.IsFalse(context.SaveChangesCalled);
            Assert.IsFalse(context.SaveChangesAsyncCalled);

            service.SaveChanges();
            Assert.AreEqual(1, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(1, saveAction.AfterSaveChangesCount);
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCountAsync);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCountAsync);
            Assert.IsTrue(context.SaveChangesCalled);
            Assert.IsFalse(context.SaveChangesAsyncCalled);
            Assert.IsTrue(context == saveAction.GivenContext);
        }

        [TestMethod]
        public async Task TestSaveChangesAsync()
        {
            var saveAction = new SampleSaveAction();
            var context = new SampleDbContext();
            var service = new DbContextService<SampleDbContext>(context, new List<ISaveAction> { saveAction });

            Assert.AreEqual(0, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCount);
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCountAsync);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCountAsync);
            Assert.IsFalse(context.SaveChangesCalled);
            Assert.IsFalse(context.SaveChangesAsyncCalled);

            await service.SaveChangesAsync();
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCount);
            Assert.AreEqual(1, saveAction.BeforeSaveChangesCountAsync);
            Assert.AreEqual(1, saveAction.AfterSaveChangesCountAsync);
            Assert.IsFalse(context.SaveChangesCalled);
            Assert.IsTrue(context.SaveChangesAsyncCalled);
            Assert.IsTrue(context == saveAction.GivenContext);
        }

        [TestMethod]
        public void TestSaveChanges_NoActions()
        {
            var context = new SampleDbContext();
            var service = new DbContextService<SampleDbContext>(context);
            service.SaveChanges();
            Assert.IsTrue(context.SaveChangesCalled);
            Assert.IsFalse(context.SaveChangesAsyncCalled);

        }

        [TestMethod]
        public async Task TestSaveChangesAsync_NoActions()
        {
            var context = new SampleDbContext();
            var service = new DbContextService<SampleDbContext>(context);
            await service.SaveChangesAsync();
            Assert.IsFalse(context.SaveChangesCalled);
            Assert.IsTrue(context.SaveChangesAsyncCalled);
        }

        #endregion

        #region Dispose
        [TestMethod]
        public void TestDispose_Context()
        {
            var testContext = new SampleDbContext();
            var testService = new SampleService(testContext);

            var contextField = typeof(SampleService).GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic);
            var contextValue = contextField.GetValue(testService);
            Assert.IsNotNull(contextField);
            Assert.IsNotNull(contextValue);

            testService.Dispose();
            contextValue = contextField.GetValue(testService);
            Assert.IsNull(contextValue);
            Assert.IsTrue(testContext.IsDisposed);

        }

        [TestMethod]
        public void TestDispose_DisposingAgainShouldNotThrow()
        {
            var testContext = new SampleDbContext();
            var testService = new SampleService(testContext);

            var contextField = typeof(SampleService).GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic);
            var contextValue = contextField.GetValue(testService);
            Assert.IsNotNull(contextField);
            Assert.IsNotNull(contextValue);

            testService.Dispose();
            testService.Invoking(x => x.Dispose()).ShouldNotThrow();
        }
        #endregion

        #region Concurrency
        [TestMethod]
        public void TestSaveChanges_CheckConcurrency()
        {
            using (ShimsContext.Create())
            {
                var exceptionCaught = false;
                var concurrentEntity = new ConcurrentEntity
                {
                    Id = 1,

                };
                var shimEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                {
                    EntityGet = () => 
                    { 
                        return concurrentEntity; 
                    }
                };
                try
                {
                    System.Data.Entity.Infrastructure.Fakes.ShimDbUpdateException.AllInstances.EntriesGet = (exc) => 
                    {
                        var list = new List<DbEntityEntry> { shimEntry };
                        return list;
                    };
                    System.Data.Entity.Fakes.ShimDbContext.AllInstances.SaveChanges = (c) =>
                    {
                        throw new DbUpdateConcurrencyException();
                    };
                    var service = new DbContextService<ConcurrentDbContext>(new ConcurrentDbContext());
                    service.SaveChanges();
                }
                catch (EcaDbUpdateConcurrencyException e)
                {
                    exceptionCaught = true;
                    var entries = e.Entries;
                    Assert.AreEqual(1, entries.Count());
                    Assert.AreEqual(1, e.ConcurrentEntities.Count());
                    Assert.IsTrue(Object.ReferenceEquals(concurrentEntity, e.ConcurrentEntities.First()));
                }
                Assert.IsTrue(exceptionCaught);
            }
        }

        [TestMethod]
        public async Task TestSaveChangesAsync_CheckConcurrency()
        {
            using (ShimsContext.Create())
            {
                var exceptionCaught = false;
                var concurrentEntity = new ConcurrentEntity
                {
                    Id = 1,

                };
                var shimEntry = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                {
                    EntityGet = () =>
                    {
                        return concurrentEntity;
                    }
                };
                try
                {
                    System.Data.Entity.Infrastructure.Fakes.ShimDbUpdateException.AllInstances.EntriesGet = (exc) =>
                    {
                        var list = new List<DbEntityEntry> { shimEntry };
                        return list;
                    };
                    System.Data.Entity.Fakes.ShimDbContext.AllInstances.SaveChangesAsync = (c) =>
                    {
                        throw new DbUpdateConcurrencyException();
                    };
                    var service = new DbContextService<ConcurrentDbContext>(new ConcurrentDbContext());
                    await service.SaveChangesAsync();
                }
                catch (EcaDbUpdateConcurrencyException e)
                {
                    exceptionCaught = true;
                    var entries = e.Entries;
                    Assert.AreEqual(1, entries.Count());
                    Assert.AreEqual(1, e.ConcurrentEntities.Count());
                    Assert.IsTrue(Object.ReferenceEquals(concurrentEntity, e.ConcurrentEntities.First()));
                }
                Assert.IsTrue(exceptionCaught);
            }
        }

        [TestMethod]
        public void TestSaveChanges_CheckConcurrency_MutlipleEntitiesWithEqualIds()
        {
            using (ShimsContext.Create())
            {
                var exceptionCaught = false;
                var concurrentEntity1 = new ConcurrentEntity
                {
                    Id = 1,

                };
                var concurrentEntity2 = new ConcurrentEntity
                {
                    Id = 1,

                };
                var shimEntry1 = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                {
                    EntityGet = () =>
                    {
                        return concurrentEntity1;
                    }
                };
                var shimEntry2 = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                {
                    EntityGet = () =>
                    {
                        return concurrentEntity2;
                    }
                };
                try
                {
                    System.Data.Entity.Infrastructure.Fakes.ShimDbUpdateException.AllInstances.EntriesGet = (exc) =>
                    {
                        var list = new List<DbEntityEntry> { shimEntry1, shimEntry2 };
                        return list;
                    };
                    System.Data.Entity.Fakes.ShimDbContext.AllInstances.SaveChanges = (c) =>
                    {
                        throw new DbUpdateConcurrencyException();
                    };
                    var service = new DbContextService<ConcurrentDbContext>(new ConcurrentDbContext());
                    service.SaveChanges();
                }
                catch (NotSupportedException e)
                {
                    exceptionCaught = true;
                }
                Assert.IsTrue(exceptionCaught);
            }
        }

        [TestMethod]
        public async Task TestSaveChangesAsync_CheckConcurrency_MutlipleEntitiesWithEqualIds()
        {
            using (ShimsContext.Create())
            {
                var exceptionCaught = false;
                var concurrentEntity1 = new ConcurrentEntity
                {
                    Id = 1,

                };
                var concurrentEntity2 = new ConcurrentEntity
                {
                    Id = 1,

                };
                var shimEntry1 = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                {
                    EntityGet = () =>
                    {
                        return concurrentEntity1;
                    }
                };
                var shimEntry2 = new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                {
                    EntityGet = () =>
                    {
                        return concurrentEntity2;
                    }
                };
                try
                {
                    System.Data.Entity.Infrastructure.Fakes.ShimDbUpdateException.AllInstances.EntriesGet = (exc) =>
                    {
                        var list = new List<DbEntityEntry> { shimEntry1, shimEntry2 };
                        return list;
                    };
                    System.Data.Entity.Fakes.ShimDbContext.AllInstances.SaveChangesAsync = (c) =>
                    {
                        throw new DbUpdateConcurrencyException();
                    };
                    var service = new DbContextService<ConcurrentDbContext>(new ConcurrentDbContext());
                    await service.SaveChangesAsync();
                }
                catch (NotSupportedException e)
                {
                    exceptionCaught = true;
                }
                Assert.IsTrue(exceptionCaught);
            }
        }
        #endregion
    }
}
