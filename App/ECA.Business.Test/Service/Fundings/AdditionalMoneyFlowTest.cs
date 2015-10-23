using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Models.Fundings;
using ECA.Business.Service;
using ECA.Core.Exceptions;
using System.Linq.Expressions;
using System.Reflection;

namespace ECA.Business.Test.Service.Fundings
{
    [TestClass]
    public class AdditionalMoneyFlowTest
    {
        private Action<Expression<Func<MoneyFlow, int?>>, MoneyFlow> nullablePropertyTester;

        public AdditionalMoneyFlowTest()
        {
            nullablePropertyTester = (exp, moneyFlow) =>
            {
                var memberSelectorExpression = exp.Body as MemberExpression;
                var property = memberSelectorExpression.Member as PropertyInfo;
                var value = (int?)property.GetValue(moneyFlow);
                Assert.IsFalse(value.HasValue);
            };
        }


        [TestMethod]
        public void TestConstructor()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);
            Assert.AreEqual(sourceEntityId, instance.SourceEntityId);
            Assert.AreEqual(recipientEntityId, instance.RecipientEntityId);
            Assert.AreEqual(sourceEntityTypeId, instance.SourceEntityTypeId);
            Assert.AreEqual(recipientEntityTypeId, instance.RecipientEntityTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(description, instance.Description);
            Assert.AreEqual(value, instance.Value);
            Assert.AreEqual(fiscalYear, instance.FiscalYear);
            Assert.AreEqual(transactionDate, instance.TransactionDate);
            Assert.AreEqual(moneyFlowStatusId, instance.MoneyFlowStatusId);
            Assert.AreEqual(MoneyFlowType.Incoming.Id, instance.MoneyFlowTypeId);
            Assert.AreEqual(parentMoneyFlowId, instance.ParentMoneyFlowId);
        }

        [TestMethod]
        public void TestConstructor_UnknownMoneyFlowStatus()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = -1;
            var parentMoneyFlowId = 5;
            Action a = () => new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);
            a.ShouldThrow<UnknownStaticLookupException>().WithMessage(String.Format("The money flow status [{0}] is not supported.", moneyFlowStatusId));
        }

        [TestMethod]
        public void TestConstructor_UnknownSourceEntityType()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = -1;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            Action a = () => new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);
            a.ShouldThrow<UnknownStaticLookupException>().WithMessage(String.Format("The source type [{0}] is not supported.", sourceEntityTypeId));
        }

        [TestMethod]
        public void TestConstructor_UnknownRecipientEntityType()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var recipientEntityTypeId = -1;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            Action a = () => new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);
            a.ShouldThrow<UnknownStaticLookupException>().WithMessage(String.Format("The recipient type [{0}] is not supported.", recipientEntityTypeId));
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckBasicProperties()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(description, moneyFlow.Description);
            Assert.AreEqual(value, moneyFlow.Value);
            Assert.AreEqual(fiscalYear, moneyFlow.FiscalYear);
            Assert.AreEqual(transactionDate, moneyFlow.TransactionDate);
            Assert.AreEqual(moneyFlowStatusId, moneyFlow.MoneyFlowStatusId);
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceItineraryStopId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientOrganizationId);
            Assert.AreEqual(userId, moneyFlow.History.CreatedBy);
            Assert.AreEqual(userId, moneyFlow.History.RevisedBy);
            Assert.AreEqual(parentMoneyFlowId, moneyFlow.ParentMoneyFlowId);
            DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlow.History.CreatedOn, 2000);
            DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlow.History.RevisedOn, 2000);
            Assert.AreEqual(MoneyFlowType.Incoming.Id, moneyFlow.MoneyFlowTypeId);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_ItineraryStop()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceItineraryStopId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientItineraryStopId);

            nullablePropertyTester(x => x.SourceOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceProjectId, moneyFlow);

            nullablePropertyTester(x => x.RecipientOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientParticipantId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Organization()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceOrganizationId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientOrganizationId);

            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceProjectId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);            
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientParticipantId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Office()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Office.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Office.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceOrganizationId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientOrganizationId);

            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceProjectId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientParticipantId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Participant()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceParticipantId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientParticipantId);

            nullablePropertyTester(x => x.SourceOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceProjectId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Post()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceOrganizationId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientOrganizationId);

            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceProjectId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientParticipantId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Program()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Program.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Program.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceProgramId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientProgramId);

            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.SourceOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.SourceProjectId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Project()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceProjectId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientProjectId);

            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.SourceOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Transportation()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Transportation.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceProjectId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientTransportationId);

            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.SourceOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Accomodation()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Accomodation.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceProjectId);
            Assert.AreEqual(recipientEntityId, moneyFlow.RecipientAccommodationId);

            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.SourceOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
        }

        [TestMethod]
        public void TestGetMoneyFlow_CheckSourceAndRecipientEntityId_Expense()
        {
            var sourceEntityId = 1;
            var recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Expense.Id;
            var userId = 1;
            var user = new User(userId);
            var description = "description";
            var value = 1.00m;
            var fiscalYear = 2015;
            var transactionDate = DateTimeOffset.UtcNow;
            var moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var parentMoneyFlowId = 5;
            var instance = new AdditionalMoneyFlow(user, parentMoneyFlowId, description, value, moneyFlowStatusId, transactionDate, fiscalYear, sourceEntityId, recipientEntityId, sourceEntityTypeId, recipientEntityTypeId);

            var moneyFlow = instance.GetMoneyFlow();
            Assert.AreEqual(sourceEntityId, moneyFlow.SourceProjectId);
            
            nullablePropertyTester(x => x.SourceParticipantId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.SourceOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.SourceProgramId, moneyFlow);
            nullablePropertyTester(x => x.SourceItineraryStopId, moneyFlow);

            nullablePropertyTester(x => x.RecipientItineraryStopId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientOrganizationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProjectId, moneyFlow);
            nullablePropertyTester(x => x.RecipientTransportationId, moneyFlow);
            nullablePropertyTester(x => x.RecipientProgramId, moneyFlow);
            nullablePropertyTester(x => x.RecipientAccommodationId, moneyFlow);
        }
    }
}
