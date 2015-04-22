using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using ECA.Core.Data;
using ECA.Core.Generation;
using Moq;
using System.Reflection;

namespace ECA.Core.Test.Generation
{
    public class TestClass
    {
        public int TestClassId { get; set; }

        public string TestClassName { get; set; }

        public static StaticLookup X { get { return new StaticLookup("X", 1); } }

    }

    public class OtherClass
    {
        public int Id { get; set; }
    }

    public class TestClassDbSet : TestDbSet<TestClass>
    {
        public override TestClass Find(params object[] keyValues)
        {
            var id = keyValues.First();
            return this.SingleOrDefault(x => x.TestClassId.Equals(id));
        }


    }

    public class TestDbContext : DbContext
    {
        public virtual DbSet<TestClass> TestClasses { get; set; }
        public virtual DbSet<OtherClass> OtherClasses { get; set; }
    }

    [TestClass]
    public class DbContextStaticLookupValidatorTest
    {
        private DbContextStaticLookupValidator validator;
        private Mock<TestDbContext> mockContext;
        private TestClassDbSet testDbSet;

        [TestInitialize]
        public void TestInit()
        {
            Database.SetInitializer<TestDbContext>(null);
            testDbSet = new TestClassDbSet();

            mockContext = new Mock<TestDbContext>();
            mockContext.Setup(x => x.TestClasses).Returns(testDbSet);
            mockContext.Setup(x => x.OtherClasses).Returns(new TestDbSet<OtherClass>());



        }

        #region Dispose
        [TestMethod]
        public void TestDispose_Context()
        {
            var testContext = new TestDbContext();
            var testService = new DbContextStaticLookupValidator(testContext);

            var contextField = typeof(DbContextStaticLookupValidator).GetField("context", BindingFlags.NonPublic | BindingFlags.Instance);
            var contextValue = contextField.GetValue(testService);
            Assert.IsNotNull(contextField);
            Assert.IsNotNull(contextValue);

            testService.Dispose();
            contextValue = contextField.GetValue(testService);
            Assert.IsNull(contextValue);

        }
        #endregion
    }
}
