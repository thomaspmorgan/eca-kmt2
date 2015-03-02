using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Service;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using System.Reflection;

namespace ECA.Core.Test.Service
{
    public class SampleSaveAction : ISaveAction<SampleDbContext>
    {

        public int BeforeSaveChangesCount { get; set; }
        public int AfterSaveChangesCount { get; set; }
        public int BeforeSaveChangesCountAsync { get; set; }
        public int AfterSaveChangesCountAsync { get; set; }

        public SampleDbContext GivenContext { get; set; }

        public void BeforeSaveChanges(SampleDbContext context)
        {
            this.BeforeSaveChangesCount++;
            this.GivenContext = context;
        }

        public System.Threading.Tasks.Task BeforeSaveChangesAsync(SampleDbContext context)
        {
            this.BeforeSaveChangesCountAsync++;
            this.GivenContext = context;
            return Task.FromResult<object>(null);
        }

        public void AfterSaveChanges(SampleDbContext context)
        {
            this.AfterSaveChangesCount++;
            this.GivenContext = context; ;
        }

        public System.Threading.Tasks.Task AfterSaveChangesAsync(SampleDbContext context)
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

    public class SampleService : DbContextService<SampleDbContext>
    {
        public SampleService(SampleDbContext context) : base(context) { }
    }

    [TestClass]
    public class DbContextServiceTest
    {
        [TestMethod]
        public void TestSaveChanges()
        {
            var context = new SampleDbContext();
            var service = new DbContextService<SampleDbContext>(context);
            var saveAction = new SampleSaveAction();
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCount);
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCountAsync);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCountAsync);
            Assert.IsFalse(context.SaveChangesCalled);
            Assert.IsFalse(context.SaveChangesAsyncCalled);

            service.SaveChanges(new List<ISaveAction<SampleDbContext>> { saveAction });
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
            var context = new SampleDbContext();
            var service = new DbContextService<SampleDbContext>(context);
            var saveAction = new SampleSaveAction();
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCount);
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCountAsync);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCountAsync);
            Assert.IsFalse(context.SaveChangesCalled);
            Assert.IsFalse(context.SaveChangesAsyncCalled);

            await service.SaveChangesAsync(new List<ISaveAction<SampleDbContext>> { saveAction });
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
        #endregion
    }
}
