using ECA.Business.Service.Persons;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.QualityTools.Testing.Fakes.FakesDelegates;

namespace ECA.Business.Test.Service.Persons
{
    [TestFixture]
    public class ParticipantPersonSevisValidationTest
    {
        private SevisValidationService sevisService;

        [SetUp]
        public void Setup()
        {
            sevisService = new SevisValidationService();
        }
        
        [Test]
        public void TestSevisValidation()
        {
            var fullName = new FullName { };
            var personalInfo = new PersonalInfo
            {
                fullName = fullName
            };
            var usAddress = new USAddress { };
            var foreignAddress = new ForeignAddress { };
            var eduLevel = new EduLevel { };
            var engProficiency = new EngProficiency { };
            var educationalInfo = new EducationalInfo
            {
                eduLevel = eduLevel,
                engProficiency = engProficiency
            };
            var expenseOther = new ExpenseOther { };
            var expense = new Expense
            {
                Other = expenseOther
            };
            var schoolFunding = new School { };
            var otherFunding = new FundingOther { };
            var funding = new Funding
            {
                School = schoolFunding,
                Other = otherFunding
            };
            var financialInfo = new FinancialInfo
            {
                Expense = expense,
                Funding = funding
            };
            var dependent = new PersonalInfo
            {
                fullName = fullName
            };
            var createDependent = new CreateDependent
            {
                Dependent = dependent,
                Remarks = "test dependent"
            };
            var student = new Student
            {
                requestID = "1",
                userID = "1",
                printForm = false,
                UserDefinedA = "",
                UserDefinedB = "",
                personalInfo = personalInfo,
                IssueReason = "Test",
                usAddress = usAddress,
                foreignAddress = foreignAddress,
                educationalInfo = educationalInfo,
                financialInfo = financialInfo,
                createDependent = createDependent,
                Remarks = "test remarks"
            };
            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var createStudent = new CreateStudent
            {
                student = student
            };
            var updateStudent = new SEVISBatchCreateUpdateStudent
            {
                userID = "1",
                batchHeader = batchHeader,
                createStudent = createStudent
            };

            var vc = new ValidationContext(updateStudent, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(updateStudent, vc, results);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);

            //if (!isValid)
            //{
            //    foreach (var validationResult in results)
            //    {
            //        Console.WriteLine(validationResult.ErrorMessage);
            //    }
            //}
        }

        [Test]
        public void TestSevisValidation_NullStudent()
        {
            Action<IQueryable<ValidationResult>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count());
                Assert.Equals("Student is required", results.First().ErrorMessage);
            };

            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var createStudent = new CreateStudent
            {
                student = null
            };
            var updateStudent = new SEVISBatchCreateUpdateStudent
            {
                userID = "1",
                batchHeader = batchHeader,
                createStudent = createStudent
            };
            //var vc = new ValidationContext(updateStudent, null, null);
            //var results = new List<ValidationResult>();
            //var actual = Validator.TryValidateObject(updateStudent, vc, results, true);

            var serviceResults = sevisService.PreSevisValidation(updateStudent);

            tester(serviceResults.Result);
        }

    }
}
