using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;

namespace ECA.Business.Test.Queries.Models.Persons.ExchangeVisitor
{
    [TestClass]
    public class ExchangeVisitorFundingDTOTest
    {
        [TestMethod]
        public void TestGetUSGovt()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = "amount 1",
                Amount2 = "amount 2",
                Org1 = "org 1",
                Org2 = "org 2",
                OtherName1 = "other 1",
                OtherName2 = "other 2"
            };
            var instance = dto.GetUSGovt();
            Assert.AreEqual(dto.Amount1, instance.Amount1);
            Assert.AreEqual(dto.Amount2, instance.Amount2);
            Assert.AreEqual(dto.Org1, instance.Org1);
            Assert.AreEqual(dto.Org2, instance.Org2);
            Assert.AreEqual(dto.OtherName1, instance.OtherName1);
            Assert.AreEqual(dto.OtherName2, instance.OtherName2);
        }

        [TestMethod]
        public void TestGetInternational()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = "amount 1",
                Amount2 = "amount 2",
                Org1 = "org 1",
                Org2 = "org 2",
                OtherName1 = "other 1",
                OtherName2 = "other 2"
            };
            var instance = dto.GetInternational();
            Assert.AreEqual(dto.Amount1, instance.Amount1);
            Assert.AreEqual(dto.Amount2, instance.Amount2);
            Assert.AreEqual(dto.Org1, instance.Org1);
            Assert.AreEqual(dto.Org2, instance.Org2);
            Assert.AreEqual(dto.OtherName1, instance.OtherName1);
            Assert.AreEqual(dto.OtherName2, instance.OtherName2);
        }
    }
}
