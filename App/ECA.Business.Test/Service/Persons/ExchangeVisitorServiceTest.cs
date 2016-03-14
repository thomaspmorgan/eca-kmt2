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
using ECA.Business.Validation.Sevis;

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

        #region GetExchangeVisitor
        [TestMethod]
        public async Task TestGetExchangeVisitor()
        {
            var projectId = 10;
            var participantId = 100;
            var personId = 11;
            var user = new User(1000);
            var person = new Data.Person
            {
                PersonId = personId
            };
            var project = new Project
            {
                ProjectId = projectId,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var programCategory = new ProgramCategory
            {
                ProgramCategoryCode = "program category code",
                ProgramCategoryId = 234
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            project.Participants.Add(participant);
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                Participant = participant,
                ProgramCategoryId = programCategory.ProgramCategoryId,
                ProgramCategory = programCategory
            };
            participant.ParticipantExchangeVisitor = participantExchangeVisitor;
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                StartDate = DateTime.UtcNow.AddDays(-1.0),
                EndDate = DateTime.UtcNow.AddDays(1.0),
                SevisId = "sevis Id"
            };
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);
            context.ProgramCategories.Add(programCategory);
            using (ShimsContext.Create())
            {
                var biographicalDTO = new BiographicalDTO
                {

                };
                var dependentBiographicalDTO = new DependentBiographicalDTO
                {

                };
                var subjectFieldDTO = new SubjectFieldDTO
                {

                };

                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQueryEcaContextInt32 = (ctx, pId) =>
                {
                    return new List<BiographicalDTO> { biographicalDTO }.AsQueryable();
                };
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetParticipantDependentsBiographicalQueryEcaContextInt32 = (ctx, pId) =>
                {
                    return new List<DependentBiographicalDTO> { dependentBiographicalDTO }.AsQueryable();
                };
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQueryEcaContextInt32 = (ctx, pId) =>
                {
                    return new List<SubjectFieldDTO> { subjectFieldDTO }.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.ToListAsyncOf1IQueryableOfM0<DependentBiographicalDTO>((src) =>
                {
                    return Task<List<DependentBiographicalDTO>>.FromResult(src.ToList());
                });
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<BiographicalDTO>((src) =>
                {
                    return Task<List<BiographicalDTO>>.FromResult(src.FirstOrDefault());
                });
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<SubjectFieldDTO>((src) =>
                {
                    return Task<List<SubjectFieldDTO>>.FromResult(src.FirstOrDefault());
                });
                Action<ExchangeVisitor> tester = (exchangeVisitor) =>
                {
                    Assert.AreEqual(1, exchangeVisitor.Dependents.Count());
                    Assert.IsNotNull(exchangeVisitor.FinancialInfo);
                    Assert.AreEqual(ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE, exchangeVisitor.OccupationCategoryCode);
                    Assert.IsNotNull(exchangeVisitor.Person);
                    Assert.AreEqual(participantPerson.StartDate.Value.DateTime, exchangeVisitor.ProgramStartDate);
                    Assert.AreEqual(participantPerson.EndDate.Value.DateTime, exchangeVisitor.ProgramEndDate);
                    Assert.AreEqual(participantPerson.SevisId, exchangeVisitor.SevisId);
                    Assert.IsTrue(Object.ReferenceEquals(user, exchangeVisitor.User));

                    Assert.IsNotNull(exchangeVisitor.SiteOfActivity);
                    var cStreetAddress = service.GetStateDepartmentCStreetAddress();
                    Assert.AreEqual(cStreetAddress.Street1, exchangeVisitor.SiteOfActivity.Street1);
                    Assert.AreEqual(cStreetAddress.LocationName, exchangeVisitor.SiteOfActivity.LocationName);
                };
                var result = service.GetExchangeVisitor(user, projectId, participantId);
                var resultAsync = await service.GetExchangeVisitorAsync(user, projectId, participantId);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ProjectIsNotAnExchangeVisitorProject()
        {
            var projectId = 10;
            var participantId = 100;
            var personId = 11;
            var user = new User(1000);
            var person = new Data.Person
            {
                PersonId = personId
            };
            var project = new Project
            {
                ProjectId = projectId,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };
            var programCategory = new ProgramCategory
            {
                ProgramCategoryCode = "program category code",
                ProgramCategoryId = 234
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            project.Participants.Add(participant);
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                Participant = participant,
                ProgramCategoryId = programCategory.ProgramCategoryId,
                ProgramCategory = programCategory
            };
            participant.ParticipantExchangeVisitor = participantExchangeVisitor;
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                StartDate = DateTime.UtcNow.AddDays(-1.0),
                EndDate = DateTime.UtcNow.AddDays(1.0),
                SevisId = "sevis Id"
            };
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);
            context.ProgramCategories.Add(programCategory);

            var message = String.Format("The participant with id [{0}] belongs to a project with id [{1}] that is not an exchange visitor project.", participant.ParticipantId, project.ProjectId);
            Action a = () => service.GetExchangeVisitor(user, projectId, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(user, projectId, participantId);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ProjectDoesNotExist()
        {
            var projectId = 10;
            var participantId = 100;
            var personId = 11;
            var user = new User(1000);
            var person = new Data.Person
            {
                PersonId = personId
            };
            var programCategory = new ProgramCategory
            {
                ProgramCategoryCode = "program category code",
                ProgramCategoryId = 234
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId,
                Person = person,
                PersonId = person.PersonId
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                Participant = participant,
                ProgramCategoryId = programCategory.ProgramCategoryId,
                ProgramCategory = programCategory
            };
            participant.ParticipantExchangeVisitor = participantExchangeVisitor;
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                StartDate = DateTime.UtcNow.AddDays(-1.0),
                EndDate = DateTime.UtcNow.AddDays(1.0),
                SevisId = "sevis Id"
            };
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);
            context.ProgramCategories.Add(programCategory);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Project).Name, projectId);
            Action a = () => service.GetExchangeVisitor(user, projectId, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(user, projectId, participantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantPersonDoesNotExist()
        {
            var projectId = 10;
            var participantId = 100;
            var personId = 11;
            var user = new User(1000);
            var person = new Data.Person
            {
                PersonId = personId
            };
            var programCategory = new ProgramCategory
            {
                ProgramCategoryCode = "program category code",
                ProgramCategoryId = 234
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId,
                Person = person,
                PersonId = person.PersonId
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                Participant = participant,
                ProgramCategoryId = programCategory.ProgramCategoryId,
                ProgramCategory = programCategory
            };
            participant.ParticipantExchangeVisitor = participantExchangeVisitor;
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.People.Add(person);
            context.ProgramCategories.Add(programCategory);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(ParticipantPerson).Name, participantId);
            Action a = () => service.GetExchangeVisitor(user, projectId, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(user, projectId, participantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantIsNotAPerson()
        {
            var projectId = 10;
            var participantId = 100;
            var user = new User(1000);
            var programCategory = new ProgramCategory
            {
                ProgramCategoryCode = "program category code",
                ProgramCategoryId = 234
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId,
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                Participant = participant,
                ProgramCategoryId = programCategory.ProgramCategoryId,
                ProgramCategory = programCategory
            };
            participant.ParticipantExchangeVisitor = participantExchangeVisitor;
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.ProgramCategories.Add(programCategory);

            var message = String.Format("The participant with id [0] is not a person participant.", participant.ParticipantId);
            Action a = () => service.GetExchangeVisitor(user, projectId, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(user, projectId, participantId);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantDoesNotBelongToProject()
        {
            var projectId = 10;
            var participantId = 100;
            var user = new User(1000);
            var programCategory = new ProgramCategory
            {
                ProgramCategoryCode = "program category code",
                ProgramCategoryId = 234
            };
            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId,
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                Participant = participant,
                ProgramCategoryId = programCategory.ProgramCategoryId,
                ProgramCategory = programCategory
            };
            participant.ParticipantExchangeVisitor = participantExchangeVisitor;
            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.ProgramCategories.Add(programCategory);

            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        projectId + 1);
            Action a = () => service.GetExchangeVisitor(user, projectId + 1, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(user, projectId + 1, participantId);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantDoesNotExist()
        {
            var projectId = 10;
            var participantId = 100;
            var user = new User(1000);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, participantId);
            Action a = () => service.GetExchangeVisitor(user, projectId + 1, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(user, projectId + 1, participantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_HasAllEntityRelationships()
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
                SevisId = "sevis Id",
            };
            var occupationCategoryCode = "occupation category code";
            var dependents = new List<DependentBiographicalDTO>();
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
            Assert.IsNotNull(instance.Dependents);
            Assert.AreEqual(participantPerson.StartDate.Value.DateTime, instance.ProgramStartDate);
            Assert.AreEqual(participantPerson.EndDate.Value.DateTime, instance.ProgramEndDate);
            Assert.AreEqual(participantPerson.SevisId, instance.SevisId);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_CheckDependents()
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
                SevisId = "sevis Id",
            };
            var occupationCategoryCode = "occupation category code";
            var dependents = new List<DependentBiographicalDTO>();
            var dependent = new DependentBiographicalDTO
            {

            };
            dependents.Add(dependent);

            var siteOfActivity = service.GetStateDepartmentCStreetAddress();
            var instance = service.GetExchangeVisitor(
                user: user,
                person: person,
                financialInfo: financialInfo,
                participantPerson: participantPerson,
                occupationCategoryCode: occupationCategoryCode,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            Assert.AreEqual(1, instance.Dependents.Count());
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
            var dependents = new List<DependentBiographicalDTO>();
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

        [TestMethod]
        public void TestGetPerson_NullProgramCategory()
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
                ProgramCategory = null
            };
            var siteOfActivityAddress = service.GetStateDepartmentCStreetAddress();
            var subjectFieldDTO = new SubjectFieldDTO
            {
                SubjectFieldCode = "subject field code"
            };

            var person = service.GetPerson(biography: biographyDTO, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectFieldDTO, siteOfActivityAddress: siteOfActivityAddress);
            Assert.IsNull(person.ProgramCategoryCode);
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

        #region Financial Info
        [TestMethod]
        public async Task TestGetFinancialInfo_HasUSAndInternationFunding()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId
            };
            using (ShimsContext.Create())
            {
                var usFunding = new ExchangeVisitorFundingDTO
                {
                    Amount1 = "100",
                    Amount2 = "101",
                    Org1 = "us org 1",
                    Org2 = "us org 2",
                    OtherName1 = "us other 1",
                    OtherName2 = "us other 2"
                };
                var internationalFunding = new ExchangeVisitorFundingDTO
                {
                    Amount1 = "200",
                    Amount2 = "201",
                    Org1 = "international org 1",
                    Org2 = "international org 2",
                    OtherName1 = "international other 1",
                    OtherName2 = "international other 2"

                };
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetInternationalFundingQueryEcaContextInt32 = (ctx, partId) =>
                {
                    return new List<ExchangeVisitorFundingDTO> { internationalFunding }.AsQueryable();
                };
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetUSFundingQueryEcaContextInt32 = (ctx, partId) =>
                {
                    return new List<ExchangeVisitorFundingDTO> { usFunding }.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<ExchangeVisitorFundingDTO>((src) =>
                {
                    return Task<ExchangeVisitorFundingDTO>.FromResult(src.FirstOrDefault());
                });

                Action<FinancialInfo> tester = (financialInfo) =>
                {
                    Assert.IsNotNull(financialInfo);
                    Assert.IsNotNull(financialInfo.OtherFunds);
                    Assert.IsNotNull(financialInfo.OtherFunds.USGovt);
                    Assert.IsNotNull(financialInfo.OtherFunds.International);

                    Assert.AreEqual(usFunding.Amount1, financialInfo.OtherFunds.USGovt.Amount1);
                    Assert.AreEqual(usFunding.Amount2, financialInfo.OtherFunds.USGovt.Amount2);
                    Assert.AreEqual(usFunding.Org1, financialInfo.OtherFunds.USGovt.Org1);
                    Assert.AreEqual(usFunding.Org2, financialInfo.OtherFunds.USGovt.Org2);
                    Assert.AreEqual(usFunding.OtherName1, financialInfo.OtherFunds.USGovt.OtherName1);
                    Assert.AreEqual(usFunding.OtherName2, financialInfo.OtherFunds.USGovt.OtherName2);

                    Assert.AreEqual(internationalFunding.Amount1, financialInfo.OtherFunds.International.Amount1);
                    Assert.AreEqual(internationalFunding.Amount2, financialInfo.OtherFunds.International.Amount2);
                    Assert.AreEqual(internationalFunding.Org1, financialInfo.OtherFunds.International.Org1);
                    Assert.AreEqual(internationalFunding.Org2, financialInfo.OtherFunds.International.Org2);
                    Assert.AreEqual(internationalFunding.OtherName1, financialInfo.OtherFunds.International.OtherName1);
                    Assert.AreEqual(internationalFunding.OtherName2, financialInfo.OtherFunds.International.OtherName2);
                };
                var results = service.GetFinancialInfo(participantExchangeVisitor);
                var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
                tester(results);
                tester(resultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_HasUsGovtAgency1Amount1Funding()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                FundingGovtAgency1 = 100.0m
            };
            Action<FinancialInfo> tester = (financialInfo) =>
            {
                Assert.IsNotNull(financialInfo);
                Assert.IsTrue(financialInfo.ReceivedUSGovtFunds);
            };
            var results = service.GetFinancialInfo(participantExchangeVisitor);
            var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_HasUsGovtAgency2Amount1Funding()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                FundingGovtAgency2 = 200.0m
            };
            Action<FinancialInfo> tester = (financialInfo) =>
            {
                Assert.IsNotNull(financialInfo);
                Assert.IsTrue(financialInfo.ReceivedUSGovtFunds);
            };
            var results = service.GetFinancialInfo(participantExchangeVisitor);
            var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
            tester(results);
            tester(resultsAsync);

        }

        [TestMethod]
        public async Task TestGetFinancialInfo_HasOtherFunding()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                FundingOther = 10.0m,
                OtherName = "other"
            };
            Action<FinancialInfo> tester = (financialInfo) =>
            {
                Assert.IsNotNull(financialInfo);
                Assert.IsNotNull(financialInfo.OtherFunds.Other);
                Assert.AreEqual("10", financialInfo.OtherFunds.Other.Amount);
            };
            var results = service.GetFinancialInfo(participantExchangeVisitor);
            var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_HasZeroOtherFunding()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                FundingOther = 0.0m,
                OtherName = "other"
            };
            Action<FinancialInfo> tester = (financialInfo) =>
            {
                Assert.IsNotNull(financialInfo);
                Assert.IsNull(financialInfo.OtherFunds.Other);
            };
            var results = service.GetFinancialInfo(participantExchangeVisitor);
            var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_HasVisitorGovernmentFunding()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                FundingVisGovt = 10.0m
            };
            Action<FinancialInfo> tester = (financialInfo) =>
            {
                Assert.IsNotNull(financialInfo);
                Assert.AreEqual("10", financialInfo.OtherFunds.EVGovt);
            };
            var results = service.GetFinancialInfo(participantExchangeVisitor);
            var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_HasBinationalCommissionGovernmentFunding()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                FundingVisBNC = 10.0m
            };
            Action<FinancialInfo> tester = (financialInfo) =>
            {
                Assert.IsNotNull(financialInfo);
                Assert.AreEqual("10", financialInfo.OtherFunds.BinationalCommission);
            };
            var results = service.GetFinancialInfo(participantExchangeVisitor);
            var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_HasPersonalFunding()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,
                FundingPersonal = 10.0m
            };
            Action<FinancialInfo> tester = (financialInfo) =>
            {
                Assert.IsNotNull(financialInfo);
                Assert.AreEqual("10", financialInfo.OtherFunds.Personal);
            };
            var results = service.GetFinancialInfo(participantExchangeVisitor);
            var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_CheckPrintForm()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId,

            };
            Action<FinancialInfo> tester = (financialInfo) =>
            {
                Assert.IsNotNull(financialInfo);
                Assert.IsTrue(financialInfo.PrintForm);
            };
            var results = service.GetFinancialInfo(participantExchangeVisitor);
            var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
            tester(results);
            tester(resultsAsync);
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
