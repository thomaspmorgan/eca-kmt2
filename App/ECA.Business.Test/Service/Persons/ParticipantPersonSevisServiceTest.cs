using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using System.Threading.Tasks;
using ECA.Data;
using System.Linq;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantPersonSevisServiceTest
    {
        private TestEcaContext context;
        private ParticipantPersonsSevisService sevisService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            sevisService = new ParticipantPersonsSevisService(context);
        }

        [TestMethod]
        public async Task TestSendToSevis()
        {
            var now = DateTimeOffset.Now;
            var yesterday = now.AddDays(-1.0);

            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                AddedOn = yesterday
            };

            var status2 = new ParticipantPersonSevisCommStatus
            {
                Id = 2,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                AddedOn = now
            };

            context.ParticipantPersonSevisCommStatuses.Add(status);
            context.ParticipantPersonSevisCommStatuses.Add(status2);

            var response = await sevisService.SendToSevis(new int[] { status.ParticipantId });

            Assert.AreEqual(1, response.Length);
            Assert.AreEqual(status.ParticipantId, response[0]);

            var newStatus = context.ParticipantPersonSevisCommStatuses.Where(p => p.ParticipantId == status.ParticipantId)
                .OrderByDescending(o => o.AddedOn)
                .FirstOrDefault();

            Assert.AreEqual(SevisCommStatus.QueuedToSubmit.Id, newStatus.SevisCommStatusId);
        }

        [TestMethod]
        public async Task TestSendToSevis_EmptyArray()
        {
            var response = await sevisService.SendToSevis(new int[] {});
            Assert.AreEqual(0, response.Length);
        }

        [TestMethod]
        public async Task TestSendToSevis_Null()
        {
            var response = await sevisService.SendToSevis(null);
            Assert.AreEqual(0, response.Length);
        }

        [TestMethod]
        public void TestSendToSevis_NullPerson()
        {
            
        }
        
        [TestMethod]
        public async Task TestSendToSevis_IncorrectStatus()
        {
            var status = new ParticipantPersonSevisCommStatus
            {
                Id = 1,
                ParticipantId = 1,
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id,
                AddedOn = DateTimeOffset.Now
            };

            context.ParticipantPersonSevisCommStatuses.Add(status);

            var response = await sevisService.SendToSevis(new int[] { status.ParticipantId });

            Assert.AreEqual(0, response.Length);
        }

        [TestMethod]
        public void TestSevisValidation()
        {
            var personalInfo = new PersonalInfo { };
            var usAddress = new USAddress { };
            var foreignAddress = new ForeignAddress { };
            var eduLevel = new EduLevel { };
            var engProficiency = new EngProficiency { };
            var educationalInfo = new EducationalInfo {
                eduLevel = eduLevel,
                engProficiency = engProficiency
            };
            var expenseOther = new ExpenseOther { };
            var expense = new Expense {
                Other = expenseOther
            };
            var schoolFunding = new School { };
            var otherFunding = new FundingOther { };
            var funding = new Funding {
                School = schoolFunding,
                Other = otherFunding
            };
            var financialInfo = new FinancialInfo {
                Expense = expense,
                Funding = funding
            };
            var fullName = new FullName { };
            var dependent = new PersonalInfo
            {
                fullName = fullName
            };
            var createDependent = new CreateDependent {
                Dependent = dependent,
                Remarks = "test dependent"
            };
            var student = new Student {
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
            var createStudent = new CreateStudent
            {
                student = student
            };
            var updateStudent = new SEVISBatchCreateUpdateStudent {
                userID = "1",
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

    }
}
