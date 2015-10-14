using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Test.Queries.Model.Admin
{
    [TestClass]
    public class AddressDTOTest
    {
        [TestMethod]
        public void TestToString_HasStreetsCityDivisionCountryPostalCode()
        {
            var dto = new AddressDTO();
            dto.Street1 = "1";
            dto.Street2 = "2";
            dto.Street3 = "3";
            dto.City = "city";
            dto.Division = "division";
            dto.Country = "country";
            dto.PostalCode = "12345";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.Street1));
            Assert.IsTrue(s.Contains(dto.Street2));
            Assert.IsTrue(s.Contains(dto.Street3));
            Assert.IsTrue(s.Contains(dto.City));
            Assert.IsTrue(s.Contains(dto.Division));
            Assert.IsTrue(s.Contains(dto.Country));
            Assert.IsTrue(s.Contains(dto.PostalCode));
        }

        [TestMethod]
        public void TestToString_DoesNotContainStreets()
        {
            var dto = new AddressDTO();
            dto.City = "city";
            dto.Division = "division";
            dto.Country = "country";
            dto.PostalCode = "12345";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.City));
            Assert.IsTrue(s.Contains(dto.Division));
            Assert.IsTrue(s.Contains(dto.Country));
            Assert.IsTrue(s.Contains(dto.PostalCode));
        }

        [TestMethod]
        public void TestToString_OnlyHasCity()
        {
            var dto = new AddressDTO();
            dto.City = "city";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.City));
        }

        [TestMethod]
        public void TestToString_OnlyHasCountry()
        {
            var dto = new AddressDTO();
            dto.Country = "Country";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.Country));
        }

        [TestMethod]
        public void TestToString_OnlyHasDivision()
        {
            var dto = new AddressDTO();
            dto.Division = "Division";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.Division));
        }

        [TestMethod]
        public void TestToString_OnlyHasPostCode()
        {
            var dto = new AddressDTO();
            dto.PostalCode = "PostalCode";

            var s = dto.ToString();
            Assert.IsNotNull(s);
            Assert.IsTrue(s.Contains(dto.PostalCode));
        }
    }
}
