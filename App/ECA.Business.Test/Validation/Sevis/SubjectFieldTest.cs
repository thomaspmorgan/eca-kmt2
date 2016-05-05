using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class SubjectFieldTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var code = "01.0103";
            var foreignDegreeLevel = "degree level";
            var foreignFieldOfStudy = "field of study";
            var remarks = "remarks";

            var instance = new SubjectField(code, foreignDegreeLevel, foreignFieldOfStudy, remarks);
            Assert.AreEqual(code, instance.SubjectFieldCode);
            Assert.AreEqual(foreignDegreeLevel, instance.ForeignDegreeLevel);
            Assert.AreEqual(foreignFieldOfStudy, instance.ForeignFieldOfStudy);
            Assert.AreEqual(remarks, instance.Remarks);
        }

        [TestMethod]
        public void TestGetEVPersonTypeSubjectField()
        {
            var code = "01.0103";
            var foreignDegreeLevel = "degree level";
            var foreignFieldOfStudy = "field of study";
            var remarks = "remarks";

            var instance = new SubjectField(code, foreignDegreeLevel, foreignFieldOfStudy, remarks);
            var model = instance.GetEVPersonTypeSubjectField();
            Assert.AreEqual(code.GetProgSubjectCodeType(), model.SubjectFieldCode);
            Assert.AreEqual(instance.Remarks, model.Remarks);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorProgramEditSubject()
        {
            var code = "01.0103";
            var foreignDegreeLevel = "degree level";
            var foreignFieldOfStudy = "field of study";
            var remarks = "remarks";

            var instance = new SubjectField(code, foreignDegreeLevel, foreignFieldOfStudy, remarks);
            var model = instance.GetSEVISEVBatchTypeExchangeVisitorProgramEditSubject();
            Assert.AreEqual(code.GetProgSubjectCodeType(), model.SubjectFieldCode);
            Assert.AreEqual(instance.Remarks, model.Remarks);
            Assert.AreEqual(instance.Remarks, model.SubjectFieldRemarks);
            Assert.IsFalse(model.printForm);
        }
    }
}
