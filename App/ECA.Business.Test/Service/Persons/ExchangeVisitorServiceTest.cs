using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service;
using ECA.Business.Service.Persons;
using System.Threading.Tasks;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Queries.Persons;
using ECA.Business.Validation.Model;
using ECA.Core.Exceptions;
using ECA.Business.Service.Admin;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Business.Queries.Models.Persons;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ExchangeVisitorServiceTest
    {
        private TestEcaContext context;
        private ExchangeVisitorService service;

        private Action<AddressDTO> usStateDeptAddressTester;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ExchangeVisitorService(context);
            usStateDeptAddressTester = (address) =>
            {
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_ADDRESS_1, address.Street1);
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_CITY, address.City);
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_STATE, address.Division);
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_POSTAL_CODE, address.PostalCode);
                Assert.AreEqual(LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME, address.Country);
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_NAME, address.LocationName);
            };
        }

        #region GetCreateExchangeVisitor
        [TestMethod]
        public async Task TestGetExchangeVisitor_UseAllEntities()
        {
            var user = new User(1);
            var person = new Business.Validation.Sevis.Bio.Person(
                fullName: null,
                birthCity: null,
                birthCountryCode: null,
                birthCountryReason: null,
                birthDate: null,
                citizenshipCountryCode: null,
                emailAddress: null,
                genderCode: null,
                permanentResidenceCountryCode: null,
                phoneNumber: null,
                remarks: null,
                positionCode: null,
                programCategoryCode: null,
                subjectField: null,
                mailAddress: null,
                usAddress: null,
                printForm: true,
                personId: 10,
                participantId: 20);
            var financialInfo = new FinancialInfo(printForm: true, receivedUSGovtFunds: true, programSponsorFunds: "100", otherFunds: null);
            var participantPerson = new ParticipantPerson
            {
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                EndDate = DateTime.UtcNow.AddDays(1.0),
            };
            var occupationCategoryCode = "occupation category code";
            var dependents = new List<Dependent>();
            var siteOfActivity = service.GetStateDepartmentCStreetAddress();
            var instance = service.GetExchangeVisitor(
                user: user,
                person: person,
                financialInfo: financialInfo,
                participantPerson: participantPerson,
                occupationCategoryCode: occupationCategoryCode,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            Assert.AreEqual(occupationCategoryCode, instance.OccupationCategoryCode);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.User));
            Assert.IsTrue(Object.ReferenceEquals(person, instance.Person));
            Assert.IsTrue(Object.ReferenceEquals(financialInfo, instance.FinancialInfo));
            Assert.IsTrue(Object.ReferenceEquals(siteOfActivity, instance.SiteOfActivity));
            Assert.IsTrue(Object.ReferenceEquals(dependents, instance.Dependents));
            Assert.AreEqual(participantPerson.StartDate.Value.DateTime, instance.ProgramStartDate);
            Assert.AreEqual(participantPerson.EndDate.Value.DateTime, instance.ProgramEndDate);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantPersonDoesNotHaveStartAndEndDates()
        {
            var user = new User(1);
            var person = new Business.Validation.Sevis.Bio.Person(
                fullName: null,
                birthCity: null,
                birthCountryCode: null,
                birthCountryReason: null,
                birthDate: null,
                citizenshipCountryCode: null,
                emailAddress: null,
                genderCode: null,
                permanentResidenceCountryCode: null,
                phoneNumber: null,
                remarks: null,
                positionCode: null,
                programCategoryCode: null,
                subjectField: null,
                mailAddress: null,
                usAddress: null,
                printForm: true,
                personId: 10,
                participantId: 20);
            var financialInfo = new FinancialInfo(printForm: true, receivedUSGovtFunds: true, programSponsorFunds: "100", otherFunds: null);
            var participantPerson = new ParticipantPerson
            {
                StartDate = null,
                EndDate = null
            };
            var occupationCategoryCode = "occupation category code";
            var dependents = new List<Dependent>();
            var siteOfActivity = service.GetStateDepartmentCStreetAddress();
            var instance = service.GetExchangeVisitor(
                user: user,
                person: person,
                financialInfo: financialInfo,
                participantPerson: participantPerson,
                occupationCategoryCode: occupationCategoryCode,
                dependents: dependents,
                siteOfActivity: siteOfActivity);
            Assert.AreEqual(default(DateTime), instance.ProgramStartDate);
            Assert.AreEqual(default(DateTime), instance.ProgramEndDate);
        }
        #endregion

        #region GetPerson
        [TestMethod]
        public void TestGetPerson()
        {
            var mailAddress = new AddressDTO
            {
                AddressId = 1
            };
            var permanentResidenceAddress = new AddressDTO
            {
                AddressId = 3
            };
            var biographyDTO = new BiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO
                {
                    FirstName = "first",
                    LastName = "last",
                    PassportName = "passport",
                    PreferredName = "preferred",
                    Suffix = "suffix"
                },
                Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE,
                GenderId = Gender.Male.Id,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                PermanentResidenceAddressId = permanentResidenceAddress.AddressId,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 3,
                PhoneNumber = "123-455-6789",
                PhoneNumberId = 4,
                PositionCode = "position code"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {

                ParticipantId = 10,
                ProgramCategory = new ProgramCategory
                {
                    ProgramCategoryId = 100,
                    ProgramCategoryCode = "program category code"
                }
            };
            var siteOfActivityAddress = service.GetStateDepartmentCStreetAddress();
            var subjectFieldDTO = new SubjectFieldDTO
            {
                SubjectFieldCode = "subject field code"
            };

            var person = service.GetPerson(biography: biographyDTO, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectFieldDTO, siteOfActivityAddress: siteOfActivityAddress);

            //full name sanity checks
            Assert.IsNotNull(person.FullName);
            Assert.AreEqual(biographyDTO.FullName.FirstName, person.FullName.FirstName);

            //subjectfield checks
            Assert.IsNotNull(person.SubjectField);
            Assert.AreEqual(subjectFieldDTO.SubjectFieldCode, person.SubjectField.SubjectFieldCode);

            Assert.IsTrue(Object.ReferenceEquals(siteOfActivityAddress, person.USAddress));
            Assert.IsTrue(Object.ReferenceEquals(mailAddress, person.MailAddress));

            Assert.AreEqual(participantExchangeVisitor.ProgramCategory.ProgramCategoryCode, person.ProgramCategoryCode);
            Assert.IsNull(person.Remarks);
        }

        [TestMethod]
        public void TestGetPerson_NullSubjectField()
        {
            var mailAddress = new AddressDTO
            {
                AddressId = 1
            };
            var permanentResidenceAddress = new AddressDTO
            {
                AddressId = 3
            };
            var biographyDTO = new BiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO
                {
                    FirstName = "first",
                    LastName = "last",
                    PassportName = "passport",
                    PreferredName = "preferred",
                    Suffix = "suffix"
                },
                Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE,
                GenderId = Gender.Male.Id,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                PermanentResidenceAddressId = permanentResidenceAddress.AddressId,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 3,
                PhoneNumber = "123-455-6789",
                PhoneNumberId = 4,
                PositionCode = "position code"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {

                ParticipantId = 10,
                ProgramCategory = new ProgramCategory
                {
                    ProgramCategoryId = 100,
                    ProgramCategoryCode = "program category code"
                }
            };
            var siteOfActivityAddress = service.GetStateDepartmentCStreetAddress();
            SubjectFieldDTO subjectFieldDTO = null;

            var person = service.GetPerson(biography: biographyDTO, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectFieldDTO, siteOfActivityAddress: siteOfActivityAddress);
            Assert.IsNotNull(person);
            Assert.IsNull(person.SubjectField);
        }

        [TestMethod]
        public void TestGetPerson_NullFullName()
        {
            var mailAddress = new AddressDTO
            {
                AddressId = 1
            };
            var permanentResidenceAddress = new AddressDTO
            {
                AddressId = 3
            };
            var biographyDTO = new BiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@isp.com",
                EmailAddressId = 1,
                FullName = null,
                Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE,
                GenderId = Gender.Male.Id,
                MailAddress = mailAddress,
                NumberOfCitizenships = 1,
                PermanentResidenceAddressId = permanentResidenceAddress.AddressId,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 3,
                PhoneNumber = "123-455-6789",
                PhoneNumberId = 4,
                PositionCode = "position code"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {

                ParticipantId = 10,
                ProgramCategory = new ProgramCategory
                {
                    ProgramCategoryId = 100,
                    ProgramCategoryCode = "program category code"
                }
            };
            var siteOfActivityAddress = service.GetStateDepartmentCStreetAddress();
            var subjectFieldDTO = new SubjectFieldDTO
            {
                SubjectFieldCode = "subject field code"
            };

            var person = service.GetPerson(biography: biographyDTO, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectFieldDTO, siteOfActivityAddress: siteOfActivityAddress);
            Assert.IsNull(person.FullName);
        }

        [TestMethod]
        public void TestGetPerson_NullMailAddress()
        {
            var permanentResidenceAddress = new AddressDTO
            {
                AddressId = 3
            };
            var biographyDTO = new BiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO(),
                Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE,
                GenderId = Gender.Male.Id,
                NumberOfCitizenships = 1,
                PermanentResidenceAddressId = permanentResidenceAddress.AddressId,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 3,
                PhoneNumber = "123-455-6789",
                PhoneNumberId = 4,
                PositionCode = "position code"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {

                ParticipantId = 10,
                ProgramCategory = new ProgramCategory
                {
                    ProgramCategoryId = 100,
                    ProgramCategoryCode = "program category code"
                }
            };
            var siteOfActivityAddress = service.GetStateDepartmentCStreetAddress();
            var subjectFieldDTO = new SubjectFieldDTO
            {
                SubjectFieldCode = "subject field code"
            };

            var person = service.GetPerson(biography: biographyDTO, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectFieldDTO, siteOfActivityAddress: siteOfActivityAddress);
            Assert.IsNull(person.MailAddress);
        }

        [TestMethod]
        public void TestGetPerson_NullSiteOfActivity()
        {
            var mailAddress = new AddressDTO
            {
                AddressId = 2
            };
            var permanentResidenceAddress = new AddressDTO
            {
                AddressId = 3
            };
            var biographyDTO = new BiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@isp.com",
                EmailAddressId = 1,
                FullName = new FullNameDTO(),
                Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE,
                GenderId = Gender.Male.Id,
                NumberOfCitizenships = 1,
                PermanentResidenceAddressId = permanentResidenceAddress.AddressId,
                PermanentResidenceCountryCode = "perm residence country code",
                PersonId = 3,
                MailAddress = mailAddress,
                PhoneNumber = "123-455-6789",
                PhoneNumberId = 4,
                PositionCode = "position code"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {

                ParticipantId = 10,
                ProgramCategory = new ProgramCategory
                {
                    ProgramCategoryId = 100,
                    ProgramCategoryCode = "program category code"
                }
            };
            AddressDTO siteOfActivityAddress = null;
            var subjectFieldDTO = new SubjectFieldDTO
            {
                SubjectFieldCode = "subject field code"
            };

            var person = service.GetPerson(biography: biographyDTO, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectFieldDTO, siteOfActivityAddress: siteOfActivityAddress);
            Assert.IsNull(person.USAddress);
        }
        #endregion


        #region State Dept US Address
        [TestMethod]
        public void TestGetStateDepartmentCStreetAddress()
        {
            var address = service.GetStateDepartmentCStreetAddress();
            usStateDeptAddressTester(address);
        }
        #endregion

        //[TestMethod]
        //public async Task TestSetDependents_ExchangeVisitor_HasDependent()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        var dependents = new List<DependentBiographicalDTO>();
        //        var dependent = new DependentBiographicalDTO
        //        {
        //            FullName = new FullNameDTO()
        //        };
        //        dependents.Add(dependent);
        //        ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetParticipantDependentsBiographicalQueryEcaContextInt32 = (ctx, participantId) =>
        //        {
        //            return dependents.AsQueryable();
        //        };
        //        System.Data.Entity.Fakes.ShimQueryableExtensions.ToListAsyncOf1IQueryableOfM0<DependentBiographicalDTO>((src) =>
        //        {
        //            return Task<List<SimplePersonDTO>>.FromResult(src.ToList());
        //        });
        //        var participant = new Participant
        //        {
        //            ParticipantId = 1,
        //        };
        //        ExchangeVisitor exchangeVisitor = null;
        //        context.SetupActions.Add(() =>
        //        {
        //            exchangeVisitor = new ExchangeVisitor
        //            {

        //            };
        //        });
        //        Action tester = () =>
        //        {
        //            Assert.IsNotNull(exchangeVisitor.CreateDependent);
        //            Assert.AreEqual(1, exchangeVisitor.CreateDependent.Count());
        //            var firstDto = exchangeVisitor.CreateDependent.First();
        //        };
        //        context.Revert();
        //        service.SetDependents(participant, exchangeVisitor);
        //        tester();

        //        context.Revert();
        //        await service.SetDependentsAsync(participant, exchangeVisitor);
        //        tester();
        //    }

        //}


    }
}
