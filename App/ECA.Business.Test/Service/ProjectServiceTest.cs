using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Data;
using ECA.Data;

namespace ECA.Business.Test.Service
{
    [TestClass]
    public class ProjectServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dbContext = InMemoryDbContext.CreateInMemoryContext<EcaContext>();
        }
    }
}
