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

        [TestMethod]
        public void TestIsEmpty_AllValuesNull()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = null,
                Amount2 = null,
                Org1 = null,
                Org2 = null,
                OtherName1 = null,
                OtherName2 = null
            };
            Assert.IsTrue(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_AllValuesEmpty()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = String.Empty,
                Amount2 = String.Empty,
                Org1 = String.Empty,
                Org2 = String.Empty,
                OtherName1 = String.Empty,
                OtherName2 = String.Empty
            };
            Assert.IsTrue(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_AllValuesWhitespace()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = " ",
                Amount2 = " ",
                Org1 = " ",
                Org2 = " ",
                OtherName1 = " ",
                OtherName2 = " "
            };
            Assert.IsTrue(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_Amount1HasValue()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = "1",
                Amount2 = null,
                Org1 = null,
                Org2 = null,
                OtherName1 = null,
                OtherName2 = null
            };
            Assert.IsFalse(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_Amount1HasZeroValue()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = "0",
                Amount2 = null,
                Org1 = null,
                Org2 = null,
                OtherName1 = null,
                OtherName2 = null
            };
            Assert.IsTrue(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_Amount2HasValue()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = null,
                Amount2 = "1",
                Org1 = null,
                Org2 = null,
                OtherName1 = null,
                OtherName2 = null
            };
            Assert.IsFalse(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_Amount2HasZeroValue()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = null,
                Amount2 = "0",
                Org1 = null,
                Org2 = null,
                OtherName1 = null,
                OtherName2 = null
            };
            Assert.IsTrue(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_Org1HasValue()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = null,
                Amount2 = null,
                Org1 = "1",
                Org2 = null,
                OtherName1 = null,
                OtherName2 = null
            };
            Assert.IsFalse(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_Org2HasValue()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = null,
                Amount2 = null,
                Org1 = null,
                Org2 = "1",
                OtherName1 = null,
                OtherName2 = null
            };
            Assert.IsFalse(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_Other1HasValue()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = null,
                Amount2 = null,
                Org1 = null,
                Org2 = null,
                OtherName1 = "1",
                OtherName2 = null
            };
            Assert.IsFalse(dto.IsEmpty());
        }

        [TestMethod]
        public void TestIsEmpty_Other2HasValue()
        {
            var dto = new ExchangeVisitorFundingDTO
            {
                Amount1 = null,
                Amount2 = null,
                Org1 = null,
                Org2 = null,
                OtherName1 = null,
                OtherName2 = "1"
            };
            Assert.IsFalse(dto.IsEmpty());
        }
    }
}
