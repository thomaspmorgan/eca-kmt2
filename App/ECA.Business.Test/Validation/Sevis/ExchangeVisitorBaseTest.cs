using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis;
using FluentValidation;
using FluentValidation.Results;
using ECA.Data;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using System.Collections.Generic;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.Finance;

namespace ECA.Business.Test.Validation.Sevis
{
    public class TestExchangeVisitor : ExchangeVisitorBase
    {
        public override ValidationResult Validate(IValidator validator)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class ExchangeVisitorBaseTest
    {
        #region Start Date
        [TestMethod]
        public void TestSetStartDate()
        {
            var participantPerson = new ParticipantPerson();
            participantPerson.StartDate = DateTime.Now;

            var instance = new TestExchangeVisitor();
            instance.SetStartDate(participantPerson);
            Assert.AreEqual(participantPerson.StartDate, instance.ProgramStartDate);
        }

        [TestMethod]
        public void TestSetStartDate_ParticipantPersonDoesNotHaveValue()
        {
            var participantPerson = new ParticipantPerson();
            participantPerson.StartDate = default(DateTimeOffset?);

            var instance = new TestExchangeVisitor();
            instance.SetStartDate(participantPerson);
            Assert.AreEqual(default(DateTime), instance.ProgramStartDate);
        }
        #endregion

        #region End Date
        [TestMethod]
        public void TestSetEndDate()
        {
            var participantPerson = new ParticipantPerson();
            participantPerson.EndDate = DateTime.Now;

            var instance = new TestExchangeVisitor();
            instance.SetEndDate(participantPerson);
            Assert.AreEqual(participantPerson.EndDate, instance.ProgramEndDate);
        }

        [TestMethod]
        public void TestSetEndDate_ParticipantPersonDoesNotHaveValue()
        {
            var participantPerson = new ParticipantPerson();
            participantPerson.EndDate = default(DateTimeOffset?);

            var instance = new TestExchangeVisitor();
            instance.SetEndDate(participantPerson);
            Assert.AreEqual(default(DateTime), instance.ProgramEndDate);
        }
        #endregion

        #region Category Cdoe
        [TestMethod]
        public void TestSetCategoryCode()
        {
            var category = new ProgramCategory();
            category.ProgramCategoryCode = "code";
            var instance = new TestExchangeVisitor();
            instance.SetCategoryCode(category);
            Assert.AreEqual(category.ProgramCategoryCode, instance.CategoryCode);
        }

        [TestMethod]
        public void TestSetCategoryCode_NullCategoryCode()
        {
            var instance = new TestExchangeVisitor();
            instance.SetCategoryCode(null);
            Assert.IsNull(instance.CategoryCode);
        }
        #endregion

        #region Position Code
        [TestMethod]
        public void TestSetPositionCode()
        {
            var position = new Position();
            position.PositionCode = "code";
            var instance = new TestExchangeVisitor();
            instance.SetPositionCode(position);
            Assert.AreEqual(position.PositionCode, instance.PositionCode);
        }

        [TestMethod]
        public void TestSetPositionCode_NullCategoryCode()
        {
            var instance = new TestExchangeVisitor();
            instance.SetPositionCode(null);
            Assert.IsNull(instance.PositionCode);
        }
        #endregion

        #region Occupation Category Code
        [TestMethod]
        public void TestSetOccupationCategoryCode()
        {
            var code = "code";
            var instance = new TestExchangeVisitor();
            instance.SetOccupationCategoryCode(code);
            Assert.AreEqual(code, instance.OccupationCategoryCode);
        }
        #endregion

        #region Person
        [TestMethod]
        public void TestSetPerson()
        {
            var biography = new BiographicalDTO();
            biography.PersonId = 10;
            var instance = new TestExchangeVisitor();
            instance.SetPerson(biography);
            Assert.IsNotNull(instance.Person);
            Assert.AreEqual(biography.PersonId, instance.PersonId);
        }

        [TestMethod]
        public void TestSetPerson_NullBiography()
        {
            var instance = new TestExchangeVisitor();
            var biography = new BiographicalDTO();
            biography.PersonId = 10;
            instance.SetPerson(biography);
            Assert.IsNotNull(instance.Person);
            Assert.AreEqual(biography.PersonId, instance.PersonId);

            instance.SetPerson(null);
            Assert.IsNull(instance.Person);
            Assert.AreEqual(0, instance.PersonId);
        }
        #endregion

        #region Participant
        [TestMethod]
        public void TestSetParticipantId()
        {
            var participant = new Participant();
            participant.ParticipantId = 1;
            var instance = new TestExchangeVisitor();
            instance.SetParticipantId(participant);
            Assert.AreEqual(participant.ParticipantId, instance.ParticipantId);
        }
        #endregion

        #region Subject Field
        [TestMethod]
        public void TestSetSubjectField()
        {
            var fieldOfStudy = new FieldOfStudy();
            fieldOfStudy.FieldOfStudyCode = "code";
            var instance = new TestExchangeVisitor();
            instance.SetSubjectField(fieldOfStudy);
            Assert.IsNotNull(instance.SubjectField);
            Assert.AreEqual(fieldOfStudy.FieldOfStudyCode, instance.SubjectField);
        }

        [TestMethod]
        public void TestSetSubjectField_NullFieldOfStudy()
        {
            var instance = new TestExchangeVisitor();
            instance.SetSubjectField(null);
            Assert.IsNull(instance.SubjectField);
        }
        #endregion

        #region SetUSAddress
        [TestMethod]
        public void TestSetUSAddress()
        {
            var address = new AddressDTO();
            address.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            var instance = new TestExchangeVisitor();
            instance.SetUSAddress(address);
            Assert.IsNotNull(instance.USAddress);
        }

        [TestMethod]
        public void TestSetUSAddress_NullAddress()
        {
            var instance = new TestExchangeVisitor();
            instance.SetUSAddress(null);
            Assert.IsNull(instance.USAddress);
        }
        #endregion

        #region Set Mailing Address
        [TestMethod]
        public void TestSetMailAddress()
        {
            var address = new AddressDTO();
            address.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            var instance = new TestExchangeVisitor();
            instance.SetMailAddress(address);
            Assert.IsNotNull(instance.MailAddress);
        }

        [TestMethod]
        public void TestSetMailAddress_NullAddress()
        {
            var instance = new TestExchangeVisitor();
            instance.SetMailAddress(null);
            Assert.IsNull(instance.MailAddress);
        }
        #endregion

        #region Dependents
        [TestMethod]
        public void TestSetDependents()
        {
            var instance = new TestExchangeVisitor();
            var dependents = new List<Dependent>();
            instance.SetDependents(dependents);
            Assert.IsTrue(object.ReferenceEquals(dependents, instance.Dependents));
        }

        [TestMethod]
        public void TestSetDependents_NullList()
        {
            var instance = new TestExchangeVisitor();
            instance.SetDependents(null);
            Assert.IsNotNull(instance.Dependents);
            Assert.AreEqual(0, instance.Dependents.Count());
        }
        #endregion

        #region Set Financial Info
        [TestMethod]
        public void TestSetFinancialInfo()
        {
            var financialInfo = new FinancialInfo();
            var instance = new TestExchangeVisitor();
            instance.SetFinancialInfo(financialInfo);
            Assert.IsTrue(Object.ReferenceEquals(financialInfo, instance.FinancialInfo));
        }

        [TestMethod]
        public void TestSetFinancialInfo_NullFinancialInfo()
        {
            var instance = new TestExchangeVisitor();
            instance.SetFinancialInfo(null);
            Assert.IsNull(instance.FinancialInfo);
        }
        #endregion
    }
}
