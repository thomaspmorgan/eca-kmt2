using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Service;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;

namespace ECA.Core.Test.Service
{
    public class SampleSaveAction : ISaveAction
    {

        public int BeforeSaveChangesCount { get; set; }
        public int AfterSaveChangesCount { get; set; }
        public int BeforeSaveChangesCountAsync { get; set; }
        public int AfterSaveChangesCountAsync { get; set; }

        public void BeforeSaveChanges()
        {
            this.BeforeSaveChangesCount++;
        }

        public System.Threading.Tasks.Task BeforeSaveChangesAsync()
        {
            this.BeforeSaveChangesCountAsync++;
            return Task.FromResult<object>(null);
        }

        public void AfterSaveChanges()
        {
            this.AfterSaveChangesCount++;
        }

        public System.Threading.Tasks.Task AfterSaveChangesAsync()
        {
            this.AfterSaveChangesCountAsync++;
            return Task.FromResult<object>(null);
        }
    }

    public class SampleDbContext : DbContext
    {

    }

    

    [TestClass]
    public class DbContextServiceTest
    {
        [TestMethod]
        public async Task TestSaveChanges()
        {
            var service = new DbContextService<SampleDbContext>(new SampleDbContext());
            var saveAction = new SampleSaveAction();
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCount);

            service.SaveChanges(new List<ISaveAction> { saveAction });
            Assert.AreEqual(1, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(1, saveAction.AfterSaveChangesCount);

        }

        [TestMethod]
        public async Task TestSaveChangesAsync()
        {
            var service = new DbContextService<SampleDbContext>(new SampleDbContext());
            var saveAction = new SampleSaveAction();
            Assert.AreEqual(0, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(0, saveAction.AfterSaveChangesCount);

            await service.SaveChangesAsync(new List<ISaveAction> { saveAction });
            Assert.AreEqual(1, saveAction.BeforeSaveChangesCount);
            Assert.AreEqual(1, saveAction.AfterSaveChangesCount);

        }
    }
}
