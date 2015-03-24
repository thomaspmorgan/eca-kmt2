using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Data.Test
{
    [TestClass]
    public class OrganizationTest
    {
        [TestMethod]
        public void TestOrganization_OfficeTypeIds()
        {
            var expectedOfficeTypeIds = new int[] { OrganizationType.Office.Id, OrganizationType.Division.Id, OrganizationType.Branch.Id };
            var officeTypeIds = Organization.OFFICE_ORGANIZATION_TYPE_IDS;
            CollectionAssert.AreEqual(expectedOfficeTypeIds, officeTypeIds);
        }
    }
}
