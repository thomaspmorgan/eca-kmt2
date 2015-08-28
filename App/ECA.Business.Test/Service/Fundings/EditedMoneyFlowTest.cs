using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service.Fundings;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Fundings
{
    [TestClass]
    public class EditedMoneyFlowTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var entityId = 1;
            var entityTypeId = MoneyFlowSourceRecipientType.Office.Id;
            var id = 2;
            var model = new EditedMoneyFlow(id, entityId, entityTypeId);
            Assert.AreEqual(id, model.Id);
            Assert.AreEqual(entityTypeId, model.SourceOrRecipientEntityTypeId);
            Assert.AreEqual(entityId, model.SourceOrRecipientEntityId);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_UnknownMoneyFlowSourceRecipientTypeId()
        {
            var entityId = 1;
            var entityTypeId = -1;
            var id = 2;
            var model = new EditedMoneyFlow(id, entityId, entityTypeId);
        }
    }
}
