using ECA.Core.Data;
using ECA.Core.Service;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Test.Service
{
    public class PermissableEntity : IPermissable
    {

        public int Id { get; set; }

        public PermissableType PermissableType { get; set; }

        public int GetId()
        {
            return this.Id;
        }

        public PermissableType GetPermissableType()
        {
            return this.PermissableType;
        }

        public int? GetParentId()
        {
            throw new NotImplementedException();
        }

        public PermissableType GetParentPermissableType()
        {
            throw new NotImplementedException();
        }
    }

    public class StandardEntity
    {
        public string S { get; set; }
    }

    public class PermissableSaveActionContext : DbContext
    {
        public PermissableSaveActionContext()
        {
            this.PermissableEntities = new TestDbSet<PermissableEntity>();
            this.StandardEntities = new TestDbSet<StandardEntity>();
        }

        public TestDbSet<PermissableEntity> PermissableEntities { get; set; }
        public TestDbSet<StandardEntity> StandardEntities { get; set; }
    }

    [TestClass]
    public class PermissableSaveActionTest
    {
        private PermissableSaveActionContext context;
        private PermissableSaveAction saveAction;
        private Mock<IPermissableService> permissableService;

        [TestInitialize]
        public void TestInit()
        {
            permissableService = new Mock<IPermissableService>();
            context = new PermissableSaveActionContext();
            saveAction = new PermissableSaveAction(permissableService.Object);
        }

        #region Permissable Entities

        [TestMethod]
        public void TestGetAddedPermissableEntities()
        {
            using (ShimsContext.Create())
            {
                var entity = new PermissableEntity
                {
                    Id = 1,
                    PermissableType = PermissableType.Project
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.PermissableEntities.Add(entity);
                var entities = saveAction.GetAddedPermissableEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetModifiedPermissableEntities()
        {
            using (ShimsContext.Create())
            {
                var entity = new PermissableEntity
                {
                    Id = 1,
                    PermissableType = PermissableType.Project
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.PermissableEntities.Add(entity);
                var entities = saveAction.GetUpdatedPermissableEntities(context);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }


        [TestMethod]
        public void TestGetPermissableEntities_AddedEntityState()
        {
            using (ShimsContext.Create())
            {
                var entity = new PermissableEntity
                {
                    Id = 1,
                    PermissableType = PermissableType.Project
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.PermissableEntities.Add(entity);
                var entities = saveAction.GetPermissableEntities(context, EntityState.Added);
                Assert.AreEqual(1, entities.Count);
                Assert.IsTrue(Object.ReferenceEquals(entity, entities.First()));
            }
        }

        [TestMethod]
        public void TestGetPermissableEntities_NoEntitiesOfState()
        {
            using (ShimsContext.Create())
            {
                var entity = new PermissableEntity
                {
                    Id = 1,
                    PermissableType = PermissableType.Project
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.PermissableEntities.Add(entity);
                var entities = saveAction.GetPermissableEntities(context, EntityState.Modified);
                Assert.AreEqual(0, entities.Count);
            }
        }


        [TestMethod]
        public void TestGetPermissableEntities_EntitiesNotPermissable()
        {
            using (ShimsContext.Create())
            {
                var entity = new StandardEntity
                {
                    S = "string"
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.StandardEntities.Add(entity);
                var entities = saveAction.GetPermissableEntities(context, EntityState.Added);
                Assert.AreEqual(0, entities.Count);
            }
        }
        #endregion

        #region AfterSaveChanges
        [TestMethod]
        public async Task TestAfterSaveChanges()
        {
            Action<IList<IPermissable>> addedEntitiesTester = (testList) => 
            {
                Assert.IsTrue(Object.ReferenceEquals(saveAction.AddedEntities, testList));
            };
            Action<IList<IPermissable>> modifiedEntitiesTester = (testList) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(saveAction.ModifiedEntities, testList));
            };
            
            permissableService.Setup(x => x.OnAdded(It.IsAny<IList<IPermissable>>())).Callback(addedEntitiesTester);            
            permissableService.Setup(x => x.OnUpdated(It.IsAny<IList<IPermissable>>())).Callback(modifiedEntitiesTester);

            permissableService.Setup(x => x.OnAddedAsync(It.IsAny<IList<IPermissable>>())).Returns(Task.FromResult<object>(null)).Callback(addedEntitiesTester);
            permissableService.Setup(x => x.OnUpdatedAsync(It.IsAny<IList<IPermissable>>())).Returns(Task.FromResult<object>(null)).Callback(modifiedEntitiesTester);
            saveAction.AfterSaveChanges(context);            
            await saveAction.AfterSaveChangesAsync(context);
        }

        #endregion

        #region BeforeSaveChanges
        [TestMethod]
        public async Task TestBeforeSaveChanges_AddedAndModifiedEntities()
        {
            using (ShimsContext.Create())
            {
                var addedEntity = new PermissableEntity
                {
                    Id = 1,
                    PermissableType = PermissableType.Project
                };
                var updatedEntity = new PermissableEntity
                {
                    Id = 2,
                    PermissableType = PermissableType.Program
                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => addedEntity,
                        StateGet = () => EntityState.Added
                    });
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => updatedEntity,
                        StateGet = () => EntityState.Modified
                    });
                    return entries;
                };
                context.PermissableEntities.Add(addedEntity);
                context.PermissableEntities.Add(updatedEntity);
                Action<IList<IPermissable>> onAddedTester = (addedPermissableEntities) =>
                {
                    Assert.AreEqual(1, addedPermissableEntities.Count);
                    Assert.IsTrue(Object.ReferenceEquals(addedEntity, addedPermissableEntities.First()));
                };

                Action<IList<IPermissable>> onModifiedTester = (updatedPermissableEntities) =>
                {
                    Assert.AreEqual(1, updatedPermissableEntities.Count);
                    Assert.IsTrue(Object.ReferenceEquals(updatedEntity, updatedPermissableEntities.First()));
                };

                saveAction.BeforeSaveChanges(context);
                onAddedTester(saveAction.AddedEntities);
                onModifiedTester(saveAction.ModifiedEntities);
                saveAction.AddedEntities.Clear();
                saveAction.ModifiedEntities.Clear();

                await saveAction.BeforeSaveChangesAsync(context);
                onAddedTester(saveAction.AddedEntities);
                onModifiedTester(saveAction.ModifiedEntities);
                saveAction.AddedEntities.Clear();
                saveAction.ModifiedEntities.Clear();
            }
        }

        [TestMethod]
        public async Task TestBeforeSaveChanges_NoPermissableEntities()
        {
            using (ShimsContext.Create())
            {
                var entity = new StandardEntity
                {

                };
                System.Data.Entity.Infrastructure.Fakes.ShimDbChangeTracker.AllInstances.Entries = (tracker) =>
                {
                    var entries = new List<DbEntityEntry>();
                    entries.Add(new System.Data.Entity.Infrastructure.Fakes.ShimDbEntityEntry
                    {
                        EntityGet = () => entity,
                        StateGet = () => EntityState.Added
                    });
                    return entries;
                };
                context.StandardEntities.Add(entity);
                Action<IList<IPermissable>> onAddedTester = (addedPermissableEntities) =>
                {
                    Assert.AreEqual(0, addedPermissableEntities.Count);
                };

                Action<IList<IPermissable>> onModifiedTester = (updatedPermissableEntities) =>
                {
                    Assert.AreEqual(0, updatedPermissableEntities.Count);
                };
                saveAction.BeforeSaveChanges(context);
                onAddedTester(saveAction.AddedEntities);
                onModifiedTester(saveAction.ModifiedEntities);
                saveAction.AddedEntities.Clear();
                saveAction.ModifiedEntities.Clear();

                await saveAction.BeforeSaveChangesAsync(context);
                onAddedTester(saveAction.AddedEntities);
                onModifiedTester(saveAction.ModifiedEntities);
                saveAction.AddedEntities.Clear();
                saveAction.ModifiedEntities.Clear();
            }
        }
        #endregion

        #region Constructor
        [TestMethod]
        public void TestConstructor()
        {
            var testInstance = new PermissableSaveAction(permissableService.Object);
            var serviceField = typeof(PermissableSaveAction).GetField("service", BindingFlags.Instance | BindingFlags.NonPublic);
            var serviceValue = serviceField.GetValue(testInstance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);
        }
        #endregion
    }
}
