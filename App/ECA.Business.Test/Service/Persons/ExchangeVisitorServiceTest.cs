﻿using System;
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
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ExchangeVisitorServiceTest
    {
        private TestEcaContext context;
        private ExchangeVisitorService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ExchangeVisitorService(context);
        }

        #region GetCreateExchangeVisitor

        [TestMethod]
        public void TestGetCreateExchangeVisitor_CheckProperties()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };
            var position = new Position
            {
                PositionId = 30,
                PositionCode = "posCode"
            };
            var category = new ProgramCategory
            {
                ProgramCategoryId = 20,
                ProgramCategoryCode = "catCode"
            };
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                Position = position,
                PositionId = position.PositionId,
                ProgramCategory = category,
                ProgramCategoryId = category.ProgramCategoryId
            };

            var instance = service.GetCreateExchangeVisitor(participant, user, project, visitor);
            Assert.AreEqual(participant.ParticipantId.ToString(), instance.requestID);
            Assert.AreEqual(user.Id.ToString(), instance.userID);
            Assert.AreEqual(project.StartDate.UtcDateTime, instance.PrgStartDate);
            Assert.AreEqual(project.EndDate.Value.UtcDateTime, instance.PrgEndDate.Value);
            Assert.AreEqual(ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE, instance.OccupationCategoryCode);
            Assert.AreEqual(position.PositionCode, instance.PositionCode);
            Assert.AreEqual(category.ProgramCategoryCode, instance.CategoryCode);
        }

        [TestMethod]
        public void TestGetCreateExchangeVisitor_ProjectEndDateAndPositionAndCategoryAreNull()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
            };
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };
            var instance = service.GetCreateExchangeVisitor(participant, user, project, visitor);
            Assert.IsNull(instance.PositionCode);
            Assert.IsNull(instance.CategoryCode);
            Assert.IsFalse(instance.PrgEndDate.HasValue);
        }
        #endregion

        #region Set Biography
        [TestMethod]
        public async Task TestSetBiography_CheckProperties()
        {
            ExchangeVisitor visitor = null;
            var person = new Person
            {
                PersonId = 100,
                FullName = "full name",
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
            };

            var participant = new Participant
            {
                ParticipantId = 10,
                PersonId = person.PersonId,
                Person = person,
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            participant.ParticipantPerson = participantPerson;


            context.SetupActions.Add(() =>
            {
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.People.Add(person);
                visitor = new ExchangeVisitor();
            });
            Action tester = () =>
            {
                Assert.IsNotNull(visitor.Biographical);
                Assert.AreEqual(person.Alias, visitor.Biographical.FullName.PreferredName);
                Assert.AreEqual(person.FirstName, visitor.Biographical.FullName.FirstName);
                Assert.AreEqual(person.LastName, visitor.Biographical.FullName.LastName);
                Assert.AreEqual(person.NameSuffix, visitor.Biographical.FullName.Suffix);
            };
            context.Revert();
            service.SetBiography(participant, visitor);
            tester();

            context.Revert();
            await service.SetBiographyAsync(participant, visitor);
            tester();
        }

        [TestMethod]
        public async Task TestSetBiography_BiographyIsNull()
        {
            Participant participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitor visitor = new ExchangeVisitor();
            Assert.IsNull(ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.context, participant.ParticipantId).FirstOrDefault());

            var message = String.Format("The participant with id [{0}] must have biographical information.", participant.ParticipantId);
            Action a = () => service.SetBiography(participant, visitor);
            Func<Task> f = () => service.SetBiographyAsync(participant, visitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }
        #endregion

        #region Set Subject Field
        [TestMethod]
        public async Task TestSetSubjectField_CheckProperties()
        {
            var fieldOfStudy = new FieldOfStudy
            {
                Description = "desc",
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var exchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId
            };
            participant.ParticipantExchangeVisitor = exchangeVisitor;

            ExchangeVisitor visitor = null;
            context.SetupActions.Add(() =>
            {
                context.Participants.Add(participant);
                context.ParticipantExchangeVisitors.Add(exchangeVisitor);
                context.FieldOfStudies.Add(fieldOfStudy);
                visitor = new ExchangeVisitor();
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNotNull(testInstance);
                Assert.IsNotNull(testInstance.SubjectField);
                Assert.AreEqual(fieldOfStudy.FieldOfStudyCode, testInstance.SubjectField.SubjectFieldCode);
                Assert.AreEqual(fieldOfStudy.Description, testInstance.SubjectField.Remarks);
            };
            context.Revert();
            service.SetSubjectField(participant, visitor);
            tester(visitor);

            context.Revert();
            await service.SetSubjectFieldAsync(participant, visitor);
            tester(visitor);
        }

        [TestMethod]
        public async Task TestSetSubjectField_FieldOfStudyDescriptionGreaterThanMaxLength()
        {
            var fieldOfStudy = new FieldOfStudy
            {
                Description = new String('d', ExchangeVisitorService.SUBJECT_FIELD_REMARKS_MAX_LENGTH + 1),
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var exchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId
            };
            participant.ParticipantExchangeVisitor = exchangeVisitor;

            ExchangeVisitor visitor = null;
            context.SetupActions.Add(() =>
            {
                context.Participants.Add(participant);
                context.ParticipantExchangeVisitors.Add(exchangeVisitor);
                context.FieldOfStudies.Add(fieldOfStudy);
                visitor = new ExchangeVisitor();
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNotNull(testInstance);
                Assert.IsNotNull(testInstance.SubjectField);
                Assert.AreEqual(fieldOfStudy.Description.Substring(0, ExchangeVisitorService.SUBJECT_FIELD_REMARKS_MAX_LENGTH), testInstance.SubjectField.Remarks);
            };
            context.Revert();
            service.SetSubjectField(participant, visitor);
            tester(visitor);

            context.Revert();
            await service.SetSubjectFieldAsync(participant, visitor);
            tester(visitor);
        }

        [TestMethod]
        public async Task TestSetSubjectField_FieldOfStudyDescriptionIsNull()
        {
            var fieldOfStudy = new FieldOfStudy
            {
                Description = null,
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var exchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId
            };
            participant.ParticipantExchangeVisitor = exchangeVisitor;

            ExchangeVisitor visitor = null;
            context.SetupActions.Add(() =>
            {
                context.Participants.Add(participant);
                context.ParticipantExchangeVisitors.Add(exchangeVisitor);
                context.FieldOfStudies.Add(fieldOfStudy);
                visitor = new ExchangeVisitor();
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNotNull(testInstance);
                Assert.IsNotNull(testInstance.SubjectField);
                Assert.IsNull(testInstance.SubjectField.Remarks);
            };
            context.Revert();
            service.SetSubjectField(participant, visitor);
            tester(visitor);

            context.Revert();
            await service.SetSubjectFieldAsync(participant, visitor);
            tester(visitor);
        }

        [TestMethod]
        public async Task TestSetSubjectField_ExchangeVisitorDoesNotHaveFieldOfStudy()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var exchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };
            participant.ParticipantExchangeVisitor = exchangeVisitor;

            ExchangeVisitor visitor = null;
            context.SetupActions.Add(() =>
            {
                context.Participants.Add(participant);
                context.ParticipantExchangeVisitors.Add(exchangeVisitor);
                visitor = new ExchangeVisitor();
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNotNull(testInstance);
                Assert.IsNull(testInstance.SubjectField);
            };
            context.Revert();
            service.SetSubjectField(participant, visitor);
            tester(visitor);

            context.Revert();
            await service.SetSubjectFieldAsync(participant, visitor);
            tester(visitor);
        }
        #endregion

        #region SetMailingAddress
        [TestMethod]
        public async Task TestSetMailingAddress_CheckProperties()
        {
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitor exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HomeInstitutionAddressId = address.AddressId
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitor();
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.AddressTypes.Add(addressType);
                context.Locations.Add(division);
                context.Locations.Add(country);
                context.Locations.Add(city);
                context.Locations.Add(addressLocation);
                context.Addresses.Add(address);
                context.LocationTypes.Add(addressLocationType);
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNotNull(testInstance.MailAddress);
                Assert.AreEqual(addressLocation.Street1, testInstance.MailAddress.Address1);
                Assert.AreEqual(addressLocation.Street2, testInstance.MailAddress.Address2);
                Assert.AreEqual(city.LocationName, testInstance.MailAddress.City);
                Assert.AreEqual(division.LocationName, testInstance.MailAddress.State);
                Assert.AreEqual(addressLocation.PostalCode, testInstance.MailAddress.PostalCode);
            };
            context.Revert();
            service.SetMailingAddress(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);

            context.Revert();
            await service.SetMailingAddressAsync(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);
        }

        [TestMethod]
        public async Task TestSetMailingAddress_HomeInstitutionAddressNotSet()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitor exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HomeInstitutionAddressId = null
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitor();
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNull(testInstance.MailAddress);
            };
            context.Revert();
            service.SetMailingAddress(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);

            context.Revert();
            await service.SetMailingAddressAsync(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);
        }

        [TestMethod]
        public async Task TestSetMailingAddress_AddressDoesNotExist()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitor exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HomeInstitutionAddressId = 1
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitor();
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNull(testInstance.MailAddress);
            };
            context.Revert();
            service.SetMailingAddress(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);

            context.Revert();
            await service.SetMailingAddressAsync(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);
        }
        #endregion

        #region SetMUSAddress
        [TestMethod]
        public async Task TestSetUSAddress_CheckProperties()
        {
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitor exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HostInstitutionAddressId = address.AddressId
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitor();
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
                context.AddressTypes.Add(addressType);
                context.Locations.Add(division);
                context.Locations.Add(country);
                context.Locations.Add(city);
                context.Locations.Add(addressLocation);
                context.Addresses.Add(address);
                context.LocationTypes.Add(addressLocationType);
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNotNull(testInstance.USAddress);
                Assert.AreEqual(addressLocation.Street1, testInstance.USAddress.Address1);
                Assert.AreEqual(addressLocation.Street2, testInstance.USAddress.Address2);
                Assert.AreEqual(city.LocationName, testInstance.USAddress.City);
                Assert.AreEqual(division.LocationName, testInstance.USAddress.State);
                Assert.AreEqual(addressLocation.PostalCode, testInstance.USAddress.PostalCode);
            };
            context.Revert();
            service.SetUSAddress(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);

            context.Revert();
            await service.SetUSAddressAsync(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);
        }

        [TestMethod]
        public async Task TestSetUSAddress_HostInstitutionAddressNotSet()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitor exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HostInstitutionAddressId = null
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitor();
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNull(testInstance.USAddress);
            };
            context.Revert();
            service.SetUSAddress(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);

            context.Revert();
            await service.SetUSAddressAsync(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);
        }

        [TestMethod]
        public async Task TestSetUSAddress_AddressDoesNotExist()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitor exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HostInstitutionAddressId = 1
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitor();
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });

            Action<ExchangeVisitor> tester = (testInstance) =>
            {
                Assert.IsNull(testInstance.USAddress);
            };
            context.Revert();
            service.SetUSAddress(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);

            context.Revert();
            await service.SetUSAddressAsync(participant, exchangeVisitor, participantPerson);
            tester(exchangeVisitor);
        }
        #endregion

        #region GetFinancialInfo
        [TestMethod]
        public void TestGetFinancialInfo_CheckProperties()
        {
            var exchangeVisitor = new ExchangeVisitor();
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingGovtAgency1 = 1.0m,
                FundingSponsor = 2.2m,
                FundingVisGovt = 3.3m,
                FundingVisBNC = 4.4m,
                FundingPersonal = 5.5m
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(exchangeVisitor, participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsTrue(instance.ReceivedUSGovtFunds);
            Assert.AreEqual("2", instance.ProgramSponsorFunds);
            Assert.IsNotNull(instance.OtherFunds);
            Assert.IsTrue(Object.ReferenceEquals(orgFunding, instance.OtherFunds.International));
            Assert.IsTrue(Object.ReferenceEquals(usGovFunding, instance.OtherFunds.USGovt));

            Assert.AreEqual("3", instance.OtherFunds.EVGovt);
            Assert.AreEqual("4", instance.OtherFunds.BinationalCommission);
            Assert.AreEqual("5", instance.OtherFunds.Personal);
        }

        [TestMethod]
        public void TestGetFinancialInfo_HasFundingGovtAgency1()
        {
            var exchangeVisitor = new ExchangeVisitor();
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingGovtAgency1 = 1.0m,
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(exchangeVisitor, participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsTrue(instance.ReceivedUSGovtFunds);
        }

        [TestMethod]
        public void TestGetFinancialInfo_HasFundingGovtAgency2()
        {
            var exchangeVisitor = new ExchangeVisitor();
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingGovtAgency2 = 1.0m,
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(exchangeVisitor, participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsTrue(instance.ReceivedUSGovtFunds);
        }
        [TestMethod]
        public void TestGetFinancialInfo_DoesNotHaveUsGovAgencyFunding()
        {
            var exchangeVisitor = new ExchangeVisitor();
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(exchangeVisitor, participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsFalse(instance.ReceivedUSGovtFunds);
        }
        #endregion

        #region Site of Activity
        [TestMethod]
        public void TestSetAddSiteOfActivity_CheckProperties()
        {
            var exchangeVisitor = new ExchangeVisitor();

            service.SetAddSiteOfActivity(exchangeVisitor);
            Assert.IsNotNull(exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA);
            Assert.IsNotNull(exchangeVisitor.AddSiteOfActivity.SiteOfActivityExempt);

            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_ADDRESS_1, exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.Address1);
            Assert.IsNull(exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.Address2);
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_CITY, exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.City);
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_STATE, exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.State);
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_POSTAL_CODE, exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.PostalCode);
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_NAME, exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.SiteName);
            Assert.AreEqual(true, exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.PrimarySite);
            Assert.AreEqual(string.Empty, exchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.Remarks);

            Assert.AreEqual(string.Empty, exchangeVisitor.AddSiteOfActivity.SiteOfActivityExempt.Remarks);
        }

        [TestMethod]
        public void TestGetStateDepartmentSiteOfActivity_CheckProperities()
        {
            var instance = service.GetStateDepartmentSiteOfActivity();
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_ADDRESS_1, instance.Address1);
            Assert.IsNull(instance.Address2);
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_CITY, instance.City);
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_STATE, instance.State);
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_POSTAL_CODE, instance.PostalCode);
            Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_NAME, instance.SiteName);
            Assert.AreEqual(true, instance.PrimarySite);
            Assert.AreEqual(string.Empty, instance.Remarks);
        }
        #endregion

        #region GetCreateExchangeVisitorAsync
        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_CheckExchangeVisitor()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };
            var person = new Person
            {
                PersonId = 10,
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            var position = new Position
            {
                PositionId = 30,
                PositionCode = "posCode"
            };
            var category = new ProgramCategory
            {
                ProgramCategoryId = 20,
                ProgramCategoryCode = "catCode"
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId
            };
            participant.ParticipantPerson = participantPerson;
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                Position = position,
                PositionId = position.PositionId,
                ProgramCategory = category,
                ProgramCategoryId = category.ProgramCategoryId
            };
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            Action<CreateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor);

                Assert.AreEqual(participant.ParticipantId.ToString(), instance.ExchangeVisitor.requestID);
                Assert.AreEqual(user.Id.ToString(), instance.ExchangeVisitor.userID);
                Assert.AreEqual(project.StartDate.UtcDateTime, instance.ExchangeVisitor.PrgStartDate);
                Assert.AreEqual(project.EndDate.Value.UtcDateTime, instance.ExchangeVisitor.PrgEndDate.Value);
                Assert.AreEqual(ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE, instance.ExchangeVisitor.OccupationCategoryCode);
                Assert.AreEqual(position.PositionCode, instance.ExchangeVisitor.PositionCode);
                Assert.AreEqual(category.ProgramCategoryCode, instance.ExchangeVisitor.CategoryCode);
            };

            var serviceResult = service.GetCreateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            var serviceResultAsync = await service.GetCreateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_CheckBiography()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };
            var person = new Person
            {
                PersonId = 10,
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            var position = new Position
            {
                PositionId = 30,
                PositionCode = "posCode"
            };
            var category = new ProgramCategory
            {
                ProgramCategoryId = 20,
                ProgramCategoryCode = "catCode"
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId
            };
            participant.ParticipantPerson = participantPerson;
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                Position = position,
                PositionId = position.PositionId,
                ProgramCategory = category,
                ProgramCategoryId = category.ProgramCategoryId
            };
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            Action<CreateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor);
                Assert.IsNotNull(instance.ExchangeVisitor.Biographical);

                Assert.AreEqual(person.Alias, instance.ExchangeVisitor.Biographical.FullName.PreferredName);
                Assert.AreEqual(person.FirstName, instance.ExchangeVisitor.Biographical.FullName.FirstName);
                Assert.AreEqual(person.LastName, instance.ExchangeVisitor.Biographical.FullName.LastName);
                Assert.AreEqual(person.NameSuffix, instance.ExchangeVisitor.Biographical.FullName.Suffix);
            };

            var serviceResult = service.GetCreateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            var serviceResultAsync = await service.GetCreateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_CheckSubjectField()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };
            var fieldOfStudy = new FieldOfStudy
            {
                Description = "desc",
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var person = new Person
            {
                PersonId = 10,
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId
            };
            participant.ParticipantPerson = participantPerson;
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId
            };
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            context.FieldOfStudies.Add(fieldOfStudy);
            Action<CreateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor);
                Assert.IsNotNull(instance.ExchangeVisitor.SubjectField);
                Assert.AreEqual(fieldOfStudy.FieldOfStudyCode, instance.ExchangeVisitor.SubjectField.SubjectFieldCode);
            };

            var serviceResult = service.GetCreateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            var serviceResultAsync = await service.GetCreateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_CheckingMailingAddress()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
            };

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };
            var person = new Person
            {
                PersonId = 10,
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                HomeInstitutionAddressId = address.AddressId
            };
            participant.ParticipantPerson = participantPerson;
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.LocationTypes.Add(addressLocationType);
            context.ParticipantExchangeVisitors.Add(visitor);
            context.Projects.Add(project);

            Action<CreateExchVisitor> tester = (testInstance) =>
            {
                Assert.IsNotNull(testInstance.ExchangeVisitor.MailAddress);
                Assert.AreEqual(addressLocation.Street1, testInstance.ExchangeVisitor.MailAddress.Address1);
                Assert.AreEqual(addressLocation.Street2, testInstance.ExchangeVisitor.MailAddress.Address2);
                Assert.AreEqual(city.LocationName, testInstance.ExchangeVisitor.MailAddress.City);
                Assert.AreEqual(division.LocationName, testInstance.ExchangeVisitor.MailAddress.State);
                Assert.AreEqual(addressLocation.PostalCode, testInstance.ExchangeVisitor.MailAddress.PostalCode);
            };
            var serviceResult = service.GetCreateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            var serviceResultAsync = await service.GetCreateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_CheckingUSAddress()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
            };

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };
            var person = new Person
            {
                PersonId = 10,
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                HostInstitutionAddressId = address.AddressId
            };
            participant.ParticipantPerson = participantPerson;
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.LocationTypes.Add(addressLocationType);
            context.ParticipantExchangeVisitors.Add(visitor);
            context.Projects.Add(project);

            Action<CreateExchVisitor> tester = (testInstance) =>
            {
                Assert.IsNotNull(testInstance.ExchangeVisitor.USAddress);
                Assert.AreEqual(addressLocation.Street1, testInstance.ExchangeVisitor.USAddress.Address1);
                Assert.AreEqual(addressLocation.Street2, testInstance.ExchangeVisitor.USAddress.Address2);
                Assert.AreEqual(city.LocationName, testInstance.ExchangeVisitor.USAddress.City);
                Assert.AreEqual(division.LocationName, testInstance.ExchangeVisitor.USAddress.State);
                Assert.AreEqual(addressLocation.PostalCode, testInstance.ExchangeVisitor.USAddress.PostalCode);
            };
            var serviceResult = service.GetCreateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            var serviceResultAsync = await service.GetCreateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_CheckFinancialInfo()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };
            
            var person = new Person
            {
                PersonId = 10,
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId
            };
            participant.ParticipantPerson = participantPerson;
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FundingGovtAgency1 = 1.0m,
                FundingSponsor = 2.2m,
                FundingVisGovt = 3.3m,
                FundingVisBNC = 4.4m,
                FundingPersonal = 5.5m

            };
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);            
            Action<CreateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor);
                Assert.IsNotNull(instance.ExchangeVisitor.FinancialInfo);
                Assert.AreEqual("2", instance.ExchangeVisitor.FinancialInfo.ProgramSponsorFunds);
                Assert.AreEqual("3", instance.ExchangeVisitor.FinancialInfo.OtherFunds.EVGovt);
                Assert.AreEqual("4", instance.ExchangeVisitor.FinancialInfo.OtherFunds.BinationalCommission);
                Assert.AreEqual("5", instance.ExchangeVisitor.FinancialInfo.OtherFunds.Personal);
            };

            var serviceResult = service.GetCreateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            var serviceResultAsync = await service.GetCreateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_CheckSiteOfActivity()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate
            };

            var person = new Person
            {
                PersonId = 10,
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId
            };
            participant.ParticipantPerson = participantPerson;
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,

            };
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            Action<CreateExchVisitor> tester = (instance) =>
            {
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_ADDRESS_1, instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.Address1);
                Assert.IsNull(instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.Address2);
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_CITY, instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.City);
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_STATE, instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.State);
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_POSTAL_CODE, instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.PostalCode);
                Assert.AreEqual(ExchangeVisitorService.SITE_OF_ACTIVITY_STATE_DEPT_NAME, instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.SiteName);
                Assert.AreEqual(true, instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.PrimarySite);
                Assert.AreEqual(string.Empty, instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivitySOA.Remarks);

                Assert.AreEqual(string.Empty, instance.ExchangeVisitor.AddSiteOfActivity.SiteOfActivityExempt.Remarks);
            };

            var serviceResult = service.GetCreateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            var serviceResultAsync = await service.GetCreateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }
        #endregion
    }
}
