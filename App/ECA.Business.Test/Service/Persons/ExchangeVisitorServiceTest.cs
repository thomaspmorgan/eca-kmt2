using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Service;
using ECA.Business.Service.Persons;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Core.Exceptions;
using ECA.Core.Settings;
using ECA.Data;
using FluentAssertions;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ExchangeVisitorServiceTest
    { 
        private TestEcaContext context;
        private ExchangeVisitorService service;
        private AppSettings appSettings;
        private NameValueCollection nameValueSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AddressDTO cStreetAddress;
        private string cStreetAddressJson;

        [TestInitialize]
        public void TestInit()
        {
            nameValueSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            appSettings = new AppSettings(nameValueSettings, connectionStrings);

            cStreetAddress = new AddressDTO();
            cStreetAddress.Street1 = "street1";
            cStreetAddressJson = JsonConvert.SerializeObject(cStreetAddress);
            nameValueSettings.Add(AppSettings.SEVIS_SITE_OF_ACTIVITY_ADDRESS_DTO, cStreetAddressJson);

            context = new TestEcaContext();
            service = new ExchangeVisitorService(context, appSettings);
        }

        #region GetExchangeVisitor
        [TestMethod]
        public async Task TestGetExchangeVisitor()
        {
            var projectId = 1805;
            var participantId = 100;
            var personId = 11;
            var person = new Data.Person
            {
                PersonId = personId
            };
            var project = new Project
            {
                ProjectId = projectId,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id,
                SevisOrgId = "abcde1234567890",
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
                SevisId = "sevis Id",
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
                    Assert.IsNull(exchangeVisitor.OccupationCategoryCode);
                    Assert.IsNotNull(exchangeVisitor.Person);
                    Assert.AreEqual(participantPerson.StartDate.Value.DateTime, exchangeVisitor.ProgramStartDate);
                    Assert.AreEqual(participantPerson.EndDate.Value.DateTime, exchangeVisitor.ProgramEndDate);
                    Assert.AreEqual(participantPerson.SevisId, exchangeVisitor.SevisId);
                    Assert.IsFalse(exchangeVisitor.IsValidated);

                    Assert.IsNotNull(exchangeVisitor.SevisOrgId);

                    Assert.IsNotNull(exchangeVisitor.SiteOfActivity);
                    var cStreetAddress = service.GetStateDepartmentCStreetAddress();
                    Assert.AreEqual(cStreetAddress.Street1, exchangeVisitor.SiteOfActivity.Street1);
                    Assert.AreEqual(cStreetAddress.LocationName, exchangeVisitor.SiteOfActivity.LocationName);
                };
                var result = service.GetExchangeVisitor(projectId, participantId);
                var resultAsync = await service.GetExchangeVisitorAsync(projectId, participantId);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_CheckIsValidated_ValidatedByBatch()
        {
            var projectId = 1805;
            var participantId = 100;
            var personId = 11;
            var person = new Data.Person
            {
                PersonId = personId
            };
            var project = new Project
            {
                ProjectId = projectId,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id,
                SevisOrgId = "abcde1234567890",
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
                SevisId = "sevis Id",
            };
            var commStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participantId,
                SevisCommStatusId = SevisCommStatus.ValidatedByBatch.Id
            };
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);
            context.ProgramCategories.Add(programCategory);
            context.ParticipantPersonSevisCommStatuses.Add(commStatus);
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
                    Assert.IsTrue(exchangeVisitor.IsValidated);
                };
                var result = service.GetExchangeVisitor(projectId, participantId);
                var resultAsync = await service.GetExchangeVisitorAsync(projectId, participantId);
                tester(result);
                tester(resultAsync);
            }
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_CheckIsValidated_ValidatedViaRti()
        {
            var projectId = 1805;
            var participantId = 100;
            var personId = 11;
            var person = new Data.Person
            {
                PersonId = personId
            };
            var project = new Project
            {
                ProjectId = projectId,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id,
                SevisOrgId = "abcde1234567890",
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
                SevisId = "sevis Id",
            };
            var commStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participantId,
                SevisCommStatusId = SevisCommStatus.ValidatedViaRti.Id
            };
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);
            context.ProgramCategories.Add(programCategory);
            context.ParticipantPersonSevisCommStatuses.Add(commStatus);
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
                    Assert.IsTrue(exchangeVisitor.IsValidated);
                };
                var result = service.GetExchangeVisitor(projectId, participantId);
                var resultAsync = await service.GetExchangeVisitorAsync(projectId, participantId);
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
            Action a = () => service.GetExchangeVisitor(projectId, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(projectId, participantId);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ProjectDoesNotExist()
        {
            var projectId = 10;
            var participantId = 100;
            var personId = 11;
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
            Action a = () => service.GetExchangeVisitor(projectId, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(projectId, participantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantPersonDoesNotExist()
        {
            var projectId = 10;
            var participantId = 100;
            var personId = 11;
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
            Action a = () => service.GetExchangeVisitor(projectId, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(projectId, participantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantIsNotAPerson()
        {
            var projectId = 10;
            var participantId = 100;
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
            Action a = () => service.GetExchangeVisitor(projectId, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(projectId, participantId);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantDoesNotExist()
        {
            var projectId = 10;
            var participantId = 100;

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, participantId);
            Action a = () => service.GetExchangeVisitor(projectId + 1, participantId);
            Func<Task> f = () => service.GetExchangeVisitorAsync(projectId + 1, participantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_HasAllEntityRelationships()
        {
            var person = new Business.Validation.Sevis.Bio.Person(
                fullName: null,
                birthCity: null,
                birthCountryCode: null,
                birthDate: null,
                citizenshipCountryCode: null,
                emailAddress: null,
                gender: null,
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
            var sevisOrgId = "abcde1234567890";
            var instance = service.GetExchangeVisitor(
                person: person,
                isValidated: true,
                financialInfo: financialInfo,
                participantPerson: participantPerson,
                occupationCategoryCode: occupationCategoryCode,
                dependents: dependents,
                siteOfActivity: siteOfActivity,
                sevisOrgId: sevisOrgId);

            Assert.AreEqual(occupationCategoryCode, instance.OccupationCategoryCode);
            Assert.IsTrue(Object.ReferenceEquals(person, instance.Person));
            Assert.IsTrue(Object.ReferenceEquals(financialInfo, instance.FinancialInfo));
            Assert.IsTrue(Object.ReferenceEquals(siteOfActivity, instance.SiteOfActivity));
            Assert.IsNotNull(instance.Dependents);
            Assert.AreEqual(participantPerson.StartDate.Value.DateTime, instance.ProgramStartDate);
            Assert.AreEqual(participantPerson.EndDate.Value.DateTime, instance.ProgramEndDate);
            Assert.AreEqual(participantPerson.SevisId, instance.SevisId);
            Assert.IsTrue(instance.IsValidated);
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_CheckDependents()
        {
            var sevisUserId = "sevisUserId";
            var person = new Business.Validation.Sevis.Bio.Person(
                fullName: null,
                birthCity: null,
                birthCountryCode: null,
                birthDate: null,
                citizenshipCountryCode: null,
                emailAddress: null,
                gender: null,
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
            var sevisOrgId = "abcde1234567890";
            var instance = service.GetExchangeVisitor(
                person: person,
                isValidated: true,
                financialInfo: financialInfo,
                participantPerson: participantPerson,
                occupationCategoryCode: occupationCategoryCode,
                dependents: dependents,
                siteOfActivity: siteOfActivity,
                sevisOrgId: sevisOrgId);

            Assert.AreEqual(1, instance.Dependents.Count());
        }

        [TestMethod]
        public async Task TestGetExchangeVisitor_ParticipantPersonDoesNotHaveStartAndEndDates()
        {
            var sevisUserId = "sevisUserId";
            var person = new Business.Validation.Sevis.Bio.Person(
                fullName: null,
                birthCity: null,
                birthCountryCode: null,
                birthDate: null,
                citizenshipCountryCode: null,
                emailAddress: null,
                gender: null,
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
            var sevisOrgId = "abcde1234567890";
            var instance = service.GetExchangeVisitor(
                person: person,
                isValidated: true,
                financialInfo: financialInfo,
                participantPerson: participantPerson,
                occupationCategoryCode: occupationCategoryCode,
                dependents: dependents,
                siteOfActivity: siteOfActivity,
                sevisOrgId: sevisOrgId);
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
                BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id,
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
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 10,
                ProgramCategory = new ProgramCategory
                {
                    ProgramCategoryId = 100,
                    ProgramCategoryCode = "program category code"
                },
                Position = new Position
                {
                    PositionCode = "position code",
                    PositionId = 19092
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
            Assert.AreEqual(participantExchangeVisitor.Position.PositionCode, person.PositionCode);
            Assert.IsNull(person.Remarks);
            Assert.IsTrue(person.PrintForm);
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
                BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id,
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
                BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id,
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
                BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id,
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
                BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id,
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
                BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id,
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

        [TestMethod]
        public void TestGetPerson_NullPosition()
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
                BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id,
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
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {

                ParticipantId = 10,
                ProgramCategory = new ProgramCategory
                {

                },
                Position = null
            };
            var siteOfActivityAddress = service.GetStateDepartmentCStreetAddress();
            var subjectFieldDTO = new SubjectFieldDTO
            {
                SubjectFieldCode = "subject field code"
            };

            var person = service.GetPerson(biography: biographyDTO, participantExchangeVisitor: participantExchangeVisitor, subjectFieldDTO: subjectFieldDTO, siteOfActivityAddress: siteOfActivityAddress);
            Assert.IsNull(person.PositionCode);
        }
        #endregion

        #region State Dept US Address
        [TestMethod]
        public void TestGetStateDepartmentCStreetAddress()
        {
            var address = service.GetStateDepartmentCStreetAddress();
            Assert.AreEqual(cStreetAddressJson, JsonConvert.SerializeObject(address));
            //sanity check
            Assert.AreEqual(cStreetAddress.Street1, address.Street1);
        }
        #endregion

        #region Financial Info
        [TestMethod]
        public async Task TestGetFinancialInfo_HasUSAndInternationFunding_NotEmpty()
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
                    Amount1 = 100m,
                    Amount2 = 101m,
                    Org1 = "us org 1",
                    Org2 = "us org 2",
                    OtherName1 = "us other 1",
                    OtherName2 = "us other 2"
                };
                var internationalFunding = new ExchangeVisitorFundingDTO
                {
                    Amount1 = 200m,
                    Amount2 = 200m,
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
                    Assert.IsFalse(usFunding.IsEmpty());
                    Assert.IsFalse(internationalFunding.IsEmpty());
                    Assert.IsNotNull(financialInfo);
                    Assert.IsNotNull(financialInfo.OtherFunds);
                    Assert.IsNotNull(financialInfo.OtherFunds.USGovernmentFunding);
                    Assert.IsNotNull(financialInfo.OtherFunds.InternationalFunding);

                    Assert.AreEqual(((int)usFunding.Amount1).ToString(), financialInfo.OtherFunds.USGovernmentFunding.Amount1);
                    Assert.AreEqual(((int)usFunding.Amount2).ToString(), financialInfo.OtherFunds.USGovernmentFunding.Amount2);
                    Assert.AreEqual(usFunding.Org1, financialInfo.OtherFunds.USGovernmentFunding.Org1);
                    Assert.AreEqual(usFunding.Org2, financialInfo.OtherFunds.USGovernmentFunding.Org2);
                    Assert.AreEqual(usFunding.OtherName1, financialInfo.OtherFunds.USGovernmentFunding.OtherName1);
                    Assert.AreEqual(usFunding.OtherName2, financialInfo.OtherFunds.USGovernmentFunding.OtherName2);

                    Assert.AreEqual(((int)internationalFunding.Amount1).ToString(), financialInfo.OtherFunds.InternationalFunding.Amount1);
                    Assert.AreEqual(((int)internationalFunding.Amount2).ToString(), financialInfo.OtherFunds.InternationalFunding.Amount2);
                    Assert.AreEqual(internationalFunding.Org1, financialInfo.OtherFunds.InternationalFunding.Org1);
                    Assert.AreEqual(internationalFunding.Org2, financialInfo.OtherFunds.InternationalFunding.Org2);
                    Assert.AreEqual(internationalFunding.OtherName1, financialInfo.OtherFunds.InternationalFunding.OtherName1);
                    Assert.AreEqual(internationalFunding.OtherName2, financialInfo.OtherFunds.InternationalFunding.OtherName2);
                };
                var results = service.GetFinancialInfo(participantExchangeVisitor);
                var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
                tester(results);
                tester(resultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_HasUSAndInternationFunding_EmptyFunding()
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
                };
                var internationalFunding = new ExchangeVisitorFundingDTO
                {
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
                    Assert.IsTrue(usFunding.IsEmpty());
                    Assert.IsTrue(internationalFunding.IsEmpty());
                    Assert.IsNotNull(financialInfo);
                    Assert.IsNotNull(financialInfo.OtherFunds);
                    Assert.IsNull(financialInfo.OtherFunds.USGovernmentFunding);
                    Assert.IsNull(financialInfo.OtherFunds.InternationalFunding);
                };
                var results = service.GetFinancialInfo(participantExchangeVisitor);
                var resultsAsync = await service.GetFinancialInfoAsync(participantExchangeVisitor);
                tester(results);
                tester(resultsAsync);
            }
        }

        [TestMethod]
        public async Task TestGetFinancialInfo_OrgAndInternationFundingAreNull()
        {
            var participantId = 100;
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                ParticipantId = participantId
            };
            using (ShimsContext.Create())
            {
               
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetInternationalFundingQueryEcaContextInt32 = (ctx, partId) =>
                {
                    return new List<ExchangeVisitorFundingDTO>().AsQueryable();
                };
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetUSFundingQueryEcaContextInt32 = (ctx, partId) =>
                {
                    return new List<ExchangeVisitorFundingDTO>().AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<ExchangeVisitorFundingDTO>((src) =>
                {
                    return Task<ExchangeVisitorFundingDTO>.FromResult(src.FirstOrDefault());
                });

                Action<FinancialInfo> tester = (financialInfo) =>
                {
                    Assert.IsNotNull(financialInfo);
                    Assert.IsNotNull(financialInfo.OtherFunds);
                    Assert.IsNull(financialInfo.OtherFunds.USGovernmentFunding);
                    Assert.IsNull(financialInfo.OtherFunds.InternationalFunding);
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
                Assert.AreEqual("10", financialInfo.OtherFunds.ExchangeVisitorGovernment);
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
    }
}
