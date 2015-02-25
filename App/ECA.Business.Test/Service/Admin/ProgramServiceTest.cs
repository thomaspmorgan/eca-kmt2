using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using System.Reflection;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class ProgramServiceTest
    {
        private TestEcaContext context;
        private ProgramService service;

        [TestInitialize]
        public void TestInit()
        {
            context = DbContextHelper.GetInMemoryContext();
            service = new ProgramService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Dispose
        [TestMethod]
        public void TestDispose_Context()
        {
            var testContext = DbContextHelper.GetInMemoryContext();
            var testService = new ProgramService(testContext);

            var contextField = typeof(ProgramService).GetField("context", BindingFlags.Instance | BindingFlags.NonPublic);
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
