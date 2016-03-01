using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Validation.Model.Shared
{
    [TestClass]
    public class SubjectFieldValidatorTest
    {
        private SubjectField GetValidSubjectField()
        {
            return new SubjectField
            {
                ForeignDegreeLevel = "level",
                ForeignFieldOfStudy = "foreign",
                Remarks = "remarks",
                SubjectFieldCode = "00.0000"
            };
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ContainsLetters()
        {
            var instance = GetValidSubjectField();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.SubjectFieldCode = "00.aaaa";
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_DoesNotContainRequiredDigits()
        {
            var instance = GetValidSubjectField();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.SubjectFieldCode = "00.000";
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_MissingDecimal()
        {
            var instance = GetValidSubjectField();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.SubjectFieldCode = "000000";
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignDegreeLevelIsNull()
        {
            var instance = GetValidSubjectField();
            Assert.IsFalse(String.IsNullOrWhiteSpace(instance.ForeignDegreeLevel));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.ForeignDegreeLevel = null;
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignDegreeLevelIsEmpty()
        {
            var instance = GetValidSubjectField();
            Assert.IsFalse(String.IsNullOrWhiteSpace(instance.ForeignDegreeLevel));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.ForeignDegreeLevel = String.Empty;
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignDegreeLevelIsWhitesppace()
        {
            var instance = GetValidSubjectField();
            Assert.IsFalse(String.IsNullOrWhiteSpace(instance.ForeignDegreeLevel));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.ForeignDegreeLevel =  " ";
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignFieldOfStudyIsNull()
        {
            var instance = GetValidSubjectField();
            Assert.IsFalse(String.IsNullOrWhiteSpace(instance.ForeignFieldOfStudy));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.ForeignFieldOfStudy = null;
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignFieldOfStudyIsEmpty()
        {
            var instance = GetValidSubjectField();
            Assert.IsFalse(String.IsNullOrWhiteSpace(instance.ForeignFieldOfStudy));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.ForeignFieldOfStudy = String.Empty;
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignFieldOfStudyIsWhitesppace()
        {
            var instance = GetValidSubjectField();
            Assert.IsFalse(String.IsNullOrWhiteSpace(instance.ForeignFieldOfStudy));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.ForeignFieldOfStudy = " ";
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestRemarks_Null()
        {
            var instance = GetValidSubjectField();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.Remarks = null;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestRemarks_EmptyString()
        {
            var instance = GetValidSubjectField();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.Remarks = String.Empty;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestRemarks_Whitespace()
        {
            var instance = GetValidSubjectField();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.Remarks = " ";
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestRemarks_ExceedsMaxLength()
        {
            var instance = GetValidSubjectField();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.Remarks = new string('a', SubjectFieldValidator.REMARKS_MAX_LENGTH + 1);
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.REMARKS_MAX_LENGTH_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }
    }
}
