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

        [TestMethod]
        public void TestValidate_NoErrors()
        {
            var expectedInstance = new TestClass
            {
                TestClassId = TestClass.X.Id,
                TestClassName = TestClass.X.Value
            };

            var simpleDbSet = new Mock<DbSet>();
            simpleDbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns(expectedInstance);
            mockContext.Setup(x => x.Set(It.IsAny<Type>())).Returns(simpleDbSet.Object);

            validator = new DbContextStaticLookupValidator(mockContext.Object);
            var errors = validator.Validate<TestClass>();
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void TestValidate_ValueDoesNotMatch()
        {
            var expectedInstance = new TestClass
            {
                TestClassId = TestClass.X.Id,
                TestClassName = "some other value"
            };

            var simpleDbSet = new Mock<DbSet>();
            simpleDbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns(expectedInstance);
            mockContext.Setup(x => x.Set(It.IsAny<Type>())).Returns(simpleDbSet.Object);

            validator = new DbContextStaticLookupValidator(mockContext.Object);
            var errors = validator.Validate<TestClass>();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(String.Format(DbContextStaticLookupValidator.LOOKUP_VALUES_DO_NOT_MATCH_FORMAT, TestClass.X.Value, expectedInstance.TestClassName), errors.First());
        }

        [TestMethod]
        public void TestValidate_LookupDoesNotExistInDatabase()
        {

            var simpleDbSet = new Mock<DbSet>();
            simpleDbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns(null);
            mockContext.Setup(x => x.Set(It.IsAny<Type>())).Returns(simpleDbSet.Object);

            validator = new DbContextStaticLookupValidator(mockContext.Object);
            var errors = validator.Validate<TestClass>();
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(String.Format(DbContextStaticLookupValidator.VALUE_DOES_NOT_EXIST_IN_DATABASE_FORMAT, TestClass.X.Value), errors.First());
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
