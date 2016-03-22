using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Shared
{
    [TestClass]
    public class SubjectFieldValidatorTest
    {
        [TestMethod]
        public void TestSubjectFieldOfCode_SubjectFieldCodeIsNull()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };

            var instance = createEntity();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            subjectFieldCode = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ContainsLetters()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };

            var instance = createEntity();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            subjectFieldCode = "00.aaaa";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_DoesNotContainRequiredDigits()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };

            var instance = createEntity();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            subjectFieldCode = "00.000";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_MissingDecimal()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };

            var instance = createEntity();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            subjectFieldCode = "000000";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_CODE_OF_STUDY_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignDegreeLevelExceedsMaxLength()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };

            var instance = createEntity();
            Assert.IsFalse(String.IsNullOrWhiteSpace(degreeLevel));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            degreeLevel = new string('c', SubjectFieldValidator.FOREIGN_FIELD_MAX_LENGTH + 1);
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_FOREIGN_DEGREE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignDegreeLevelIsNull()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };

            var instance = createEntity();
            Assert.IsFalse(String.IsNullOrWhiteSpace(degreeLevel));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            degreeLevel = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignDegreeLevelIsEmpty()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            Assert.IsFalse(String.IsNullOrWhiteSpace(degreeLevel));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            degreeLevel = String.Empty;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignDegreeLevelIsWhitesppace()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            Assert.IsFalse(String.IsNullOrWhiteSpace(degreeLevel));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            degreeLevel =  " ";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignFieldOfStudyIsNull()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            Assert.IsFalse(String.IsNullOrWhiteSpace(foreignFieldOfStudy));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            foreignFieldOfStudy = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignFieldOfStudyIsEmpty()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            Assert.IsFalse(String.IsNullOrWhiteSpace(foreignFieldOfStudy));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            foreignFieldOfStudy = String.Empty;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignFieldOfStudyIsWhitesppace()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            Assert.IsFalse(String.IsNullOrWhiteSpace(foreignFieldOfStudy));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            foreignFieldOfStudy = " ";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestSubjectFieldOfCode_ForeignFieldOfStudyExceedsMaxLength()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            Assert.IsFalse(String.IsNullOrWhiteSpace(degreeLevel));
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            foreignFieldOfStudy = new string('c', SubjectFieldValidator.FOREIGN_FIELD_MAX_LENGTH + 1);
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_OF_STUDY_MAX_LENGTH_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestRemarks_Null()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            remarks = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestRemarks_EmptyString()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            remarks = String.Empty;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestRemarks_Whitespace()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            remarks = " ";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.SUBJECT_FIELD_REMARKS_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestRemarks_ExceedsMaxLength()
        {
            string subjectFieldCode = "00.0000";
            string remarks = "remarks";
            string degreeLevel = "level";
            string foreignFieldOfStudy = "foreign";
            Func<SubjectField> createEntity = () =>
            {
                return new SubjectField(subjectFieldCode: subjectFieldCode, foreignDegreeLevel: degreeLevel, foreignFieldOfStudy: foreignFieldOfStudy, remarks: remarks);
            };
            var instance = createEntity();
            var validator = new SubjectFieldValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            remarks = new string('a', SubjectFieldValidator.REMARKS_MAX_LENGTH + 1);
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(SubjectFieldValidator.REMARKS_MAX_LENGTH_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }
    }
}
