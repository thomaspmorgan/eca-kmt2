using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class ExchangeVisitorTest
    {
		public Business.Validation.Sevis.Bio.Person GetPerson()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;

            var personId = 100;
            var participantId = 200;
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "reason";
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var fieldOfStudyCode = "01.0102";


            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReason,
                birthDate,
                citizenshipCountryCode,
                email,
                gender,
                permanentResidenceCountryCode,
                phone,
                remarks,
                positionCode.ToString(),
                programCataegoryCode,
                fieldOfStudyCode,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            return person;
        }

        public FinancialInfo GetFinancialInfo()
        {
            var other = new Other(null, null);
            var usGovt = new USGovt(null, null, null, null, null, null);
            var international = new International(null, null, null, null, null, null);
            var otherFunds = new OtherFunds(null, null, null, usGovt, international, other);

            var programSponsorFunds = "prog sponsor funds";
            var receivedUsGovtFunds = true;
            var printForm = true;
            var financialInfo = new FinancialInfo(
                    printForm: printForm,
                    receivedUSGovtFunds: receivedUsGovtFunds,
                    programSponsorFunds: programSponsorFunds,
                    otherFunds: otherFunds);
            return financialInfo;
        }

		[TestMethod]
		public void TestConstructor()
        {
            var userId = 10;
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "occupation category code";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user,
                person,
                financialInfo,
                occupationCategoryCode,
                endDate,
                startDate,
                dependents);
            Assert.IsTrue(Object.ReferenceEquals(user, exchangeVisitor.User));
            Assert.IsTrue(Object.ReferenceEquals(person, exchangeVisitor.Person));
            Assert.IsTrue(Object.ReferenceEquals(financialInfo, exchangeVisitor.FinancialInfo));
            Assert.IsTrue(Object.ReferenceEquals(dependents, exchangeVisitor.Dependents));

            Assert.AreEqual(occupationCategoryCode, exchangeVisitor.OccupationCategoryCode);
            Assert.AreEqual(endDate, exchangeVisitor.ProgramEndDate);
            Assert.AreEqual(startDate, exchangeVisitor.ProgramStartDate);
        }
    }
}
