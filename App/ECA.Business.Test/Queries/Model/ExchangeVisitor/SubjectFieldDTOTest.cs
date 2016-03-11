using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;

namespace ECA.Business.Test.Queries.Model.ExchangeVisitor
{
    [TestClass]
    public class SubjectFieldDTOTest
    {
        [TestMethod]
        public void TestGetSubjectField()
        {
            var code = "01.0103";
            var foreignDegreeLevel = "degree level";
            var foreignFieldOfStudy = "field of study";
            var remarks = "remarks";

            var dto = new SubjectFieldDTO();
            dto.ForeignDegreeLevel = foreignDegreeLevel;
            dto.Remarks = remarks;
            dto.SubjectFieldCode = code;
            dto.ForeignFieldOfStudy = foreignFieldOfStudy;

            var instance = dto.GetSubjectField();
            Assert.AreEqual(code, instance.SubjectFieldCode);
            Assert.AreEqual(foreignDegreeLevel, instance.ForeignDegreeLevel);
            Assert.AreEqual(foreignFieldOfStudy, instance.ForeignFieldOfStudy);
            Assert.AreEqual(remarks, instance.Remarks);
        }
    }
}
