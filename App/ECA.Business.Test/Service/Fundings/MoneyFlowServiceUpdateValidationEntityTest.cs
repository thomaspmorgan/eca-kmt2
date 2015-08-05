using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Fundings;
using ECA.Business.Service.Projects;
using ECA.Core.Exceptions;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Service.Fundings
{
    [TestClass]
    public class MoneyFlowServiceUpdateValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var value = 1.00m;
            var description = "description";
            var fiscalYear = 2015;
            var entity = new MoneyFlowServiceUpdateValidationEntity(description, value, fiscalYear);
            Assert.AreEqual(value, entity.Value);
            Assert.AreEqual(description, entity.Description);
            Assert.AreEqual(fiscalYear, entity.FiscalYear);
        }
    }
}
