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

        #region GetChangeDetail
        [TestMethod]
        public void TestGetChangeDetail_SameInstance()
        {
            var code = "01.0103";
            var foreignDegreeLevel = "degree level";
            var foreignFieldOfStudy = "field of study";
            var remarks = "remarks";

            var instance = new SubjectField(code, foreignDegreeLevel, foreignFieldOfStudy, remarks);
            var changeDetail = instance.GetChangeDetail(instance);
            Assert.IsFalse(changeDetail.HasChanges());
        }

        [TestMethod]
        public void TestGetChangeDetail_NoChanges()
        {
            var code = "01.0103";
            var foreignDegreeLevel = "degree level";
            var foreignFieldOfStudy = "field of study";
            var remarks = "remarks";

            var instance = new SubjectField(code, foreignDegreeLevel, foreignFieldOfStudy, remarks);
            var otherInstance = new SubjectField(code, foreignDegreeLevel, foreignFieldOfStudy, remarks);
            var changeDetail = instance.GetChangeDetail(otherInstance);
            Assert.IsFalse(changeDetail.HasChanges());
        }

        [TestMethod]
        public void TestGetChangeDetail_HasChanges()
        {
            var code = "01.0103";
            var foreignDegreeLevel = "degree level";
            var foreignFieldOfStudy = "field of study";
            var remarks = "remarks";

            var instance = new SubjectField(code, foreignDegreeLevel, foreignFieldOfStudy, remarks);
            var otherInstance = new SubjectField("othercode", foreignDegreeLevel, foreignFieldOfStudy, remarks);
            var changeDetail = instance.GetChangeDetail(otherInstance);
            Assert.IsTrue(changeDetail.HasChanges());
        }
        #endregion
    }
}
