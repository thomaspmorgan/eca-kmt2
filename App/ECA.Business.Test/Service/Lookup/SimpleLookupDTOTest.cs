using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;

namespace ECA.Business.Test.Service.Lookup
{
    [TestClass]
    public class SimpleLookupDTOTest
    {
        [TestMethod]
        public void TestEquals_AllValuesAreEqual()
        {
            var dto = new SimpleLookupDTO
            {
                Id = 1,
                Value = "value"
            };
            var otherDto = new SimpleLookupDTO
            {
                Id = 1,
                Value = "value"
            };
            Assert.IsTrue(dto.Equals(otherDto));
            Assert.IsTrue(otherDto.Equals(dto));
        }

        [TestMethod]
        public void TestEquals_OnlyIdsEqual()
        {
            var dto = new SimpleLookupDTO
            {
                Id = 1,
                Value = "1"
            };
            var otherDto = new SimpleLookupDTO
            {
                Id = 1,
                Value = "2"
            };
            Assert.IsFalse(dto.Equals(otherDto));
            Assert.IsFalse(otherDto.Equals(dto));
        }

        [TestMethod]
        public void TestEquals_OnlyValuesEqual()
        {
            var dto = new SimpleLookupDTO
            {
                Id = 1,
                Value = "value"
            };
            var otherDto = new SimpleLookupDTO
            {
                Id = 2,
                Value = "value"
            };
            Assert.IsFalse(dto.Equals(otherDto));
            Assert.IsFalse(otherDto.Equals(dto));
        }

        [TestMethod]
        public void TestEquals_NullObject()
        {
            var dto = new SimpleLookupDTO
            {
                Id = 1,
                Value = "value"
            };
            Assert.IsFalse(dto.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentObjectType()
        {
            var dto = new SimpleLookupDTO
            {
                Id = 1,
                Value = "value"
            };
            Assert.IsFalse(dto.Equals(ComparisonType.Equal));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var dto = new SimpleLookupDTO
            {
                Id = 1,
                Value = "value"
            };
            Assert.AreEqual(dto.Value.GetHashCode(), dto.GetHashCode());
        }
    }
}
