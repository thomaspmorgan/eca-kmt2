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
using ECA.Business.Validation.Model.Shared;
using ECA.Core.Exceptions;

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
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                StartDate = yesterday,
                EndDate = endDate
            };

            var instance = service.GetCreateExchangeVisitor(participant, user, participantPerson, visitor);
            Assert.AreEqual(participant.ParticipantId.ToString(), instance.requestID);
            Assert.AreEqual(user.Id.ToString(), instance.userID);
            Assert.AreEqual(yesterday.UtcDateTime, instance.PrgStartDate);
            Assert.AreEqual(endDate.UtcDateTime, instance.PrgEndDate);
            Assert.AreEqual(ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE, instance.OccupationCategoryCode);
            Assert.AreEqual(position.PositionCode, instance.PositionCode);
            Assert.AreEqual(category.ProgramCategoryCode, instance.CategoryCode);
            Assert.IsNull(instance.ResidentialAddress);
        }

        [TestMethod]
        public void TestGetCreateExchangeVisitor_ParticipantExchangeVisitorIsNull()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var user = new User(2);
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                StartDate = yesterday,
                EndDate = endDate
            };
            var instance = service.GetCreateExchangeVisitor(participant, user, participantPerson, null);
            Assert.IsNull(instance.PositionCode);
            Assert.IsNull(instance.CategoryCode);
        }

        [TestMethod]
        public void TestGetCreateExchangeVisitor_PositionAndCategoryAreNull()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var user = new User(2);
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                StartDate = yesterday,
                EndDate = endDate
            };
            var instance = service.GetCreateExchangeVisitor(participant, user, participantPerson, visitor);
            Assert.IsNull(instance.PositionCode);
            Assert.IsNull(instance.CategoryCode);
        }

        [TestMethod]
        public void TestGetCreateExchangeVisitor_ParticipantPersonStartAndEndDatesAreNull()
        {   
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var user = new User(2);
            var visitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                StartDate = null,
                EndDate = null
            };
            var instance = service.GetCreateExchangeVisitor(participant, user, participantPerson, visitor);
            Assert.AreEqual(default(DateTime), instance.PrgStartDate);
            Assert.AreEqual(default(DateTime), instance.PrgEndDate);
        }
        #endregion

        #region Set Biography
        [TestMethod]
        public async Task TestSetBiography_CheckProperties()
        {
            ExchangeVisitor visitor = null;
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 100,
                FullName = "full name",
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
                GenderId = gender.GenderId,
                Gender = gender
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
                context.Genders.Add(gender);
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
        public async Task TestSetMailingAddress_ExchangeVisitor_CheckProperties()
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
        public async Task TestSetMailingAddress_ExchangeVisitor_HomeInstitutionAddressNotSet()
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
        public async Task TestSetMailingAddress_ExchangeVisitor_AddressDoesNotExist()
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

        [TestMethod]
        public async Task TestSetMailingAddress_ExchangeVisitorUpdate_CheckProperties()
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
            ExchangeVisitorUpdate exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HomeInstitutionAddressId = address.AddressId
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitorUpdate();
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

            Action<ExchangeVisitorUpdate> tester = (testInstance) =>
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
        public async Task TestSetMailingAddress_ExchangeVisitorUpdate_HomeInstitutionAddressNotSet()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitorUpdate exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HomeInstitutionAddressId = null
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitorUpdate();
            });

            Action<ExchangeVisitorUpdate> tester = (testInstance) =>
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
        public async Task TestSetMailingAddress_ExchangeVisitorUpdate_AddressDoesNotExist()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitorUpdate exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HomeInstitutionAddressId = 1
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitorUpdate();
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });

            Action<ExchangeVisitorUpdate> tester = (testInstance) =>
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

        #region SetUSAddress
        [TestMethod]
        public async Task TestSetUSAddress_ExchangeVisitor_CheckProperties()
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
        public async Task TestSetUSAddress_ExchangeVisitor_HostInstitutionAddressNotSet()
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
        public async Task TestSetUSAddress_ExchangeVisitor_AddressDoesNotExist()
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

        [TestMethod]
        public async Task TestSetUSAddress_ExchangeVisitorUpdate_CheckProperties()
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
            ExchangeVisitorUpdate exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HostInstitutionAddressId = address.AddressId
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitorUpdate();
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

            Action<ExchangeVisitorUpdate> tester = (testInstance) =>
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
        public async Task TestSetUSAddress_ExchangeVisitorUpdate_HostInstitutionAddressNotSet()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitorUpdate exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HostInstitutionAddressId = null
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitorUpdate();
            });

            Action<ExchangeVisitorUpdate> tester = (testInstance) =>
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
        public async Task TestSetUSAddress_ExchangeVisitorUpdate_AddressDoesNotExist()
        {
            var participant = new Participant
            {
                ParticipantId = 1
            };
            ExchangeVisitorUpdate exchangeVisitor = null;
            var participantPerson = new ParticipantPerson
            {
                HostInstitutionAddressId = 1
            };

            context.SetupActions.Add(() =>
            {
                exchangeVisitor = new ExchangeVisitorUpdate();
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });

            Action<ExchangeVisitorUpdate> tester = (testInstance) =>
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
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingGovtAgency1 = 1.0m,
                FundingSponsor = 2.2m,
                FundingVisGovt = 3.3m,
                FundingVisBNC = 4.4m,
                FundingPersonal = 5.5m,
                FundingOther = 6.6m,
                OtherName = "other name"
            };
            var orgFunding = new International
            {
                Amount1 = "1"
            };
            var usGovFunding = new USGovt
            {
                Amount1 = "2"
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsTrue(instance.ReceivedUSGovtFunds);
            Assert.AreEqual("2", instance.ProgramSponsorFunds);
            Assert.IsNotNull(instance.OtherFunds);
            Assert.IsTrue(Object.ReferenceEquals(orgFunding, instance.OtherFunds.International));
            Assert.IsTrue(Object.ReferenceEquals(usGovFunding, instance.OtherFunds.USGovt));

            Assert.AreEqual("3", instance.OtherFunds.EVGovt);
            Assert.AreEqual("4", instance.OtherFunds.BinationalCommission);
            Assert.AreEqual("5", instance.OtherFunds.Personal);
            Assert.AreEqual("6", instance.OtherFunds.Other.Amount);
            Assert.AreEqual(participantExchangeVisitor.OtherName, instance.OtherFunds.Other.Name);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingSponsorNull()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingSponsor = null
            };
            var orgFunding = new International
            {
                
            };
            var usGovFunding = new USGovt
            {
                
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNull(instance.ProgramSponsorFunds);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingSponsorZero()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingSponsor = 0m,
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNull(instance.ProgramSponsorFunds);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingVisGovtNull()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingVisGovt = null
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNull(instance.OtherFunds.EVGovt);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingVisGovtZero()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingVisGovt = 0m,
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNull(instance.OtherFunds.EVGovt);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingVisBNCNull()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingVisBNC = null
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNull(instance.OtherFunds.BinationalCommission);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingVisBNCZero()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingVisBNC = 0m,
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNull(instance.OtherFunds.BinationalCommission);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingPersonalNull()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingPersonal = null
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNull(instance.OtherFunds.Personal);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingPersonalZero()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingPersonal = 0m
            };
            var orgFunding = new International
            {

            };
            var usGovFunding = new USGovt
            {

            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNull(instance.OtherFunds.Personal);
        }

        [TestMethod]
        public void TestGetFinancialInfo_ReceivedGovFundsFromUSGovAgency1()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingGovtAgency1 = 1.0m,
                FundingGovtAgency2 = 0.0m
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, null, null);
            Assert.IsTrue(instance.ReceivedUSGovtFunds);
        }

        [TestMethod]
        public void TestGetFinancialInfo_ReceivedGovFundsFromUSGovAgency2()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingGovtAgency1 = 0.0m,
                FundingGovtAgency2 = 1.0m,
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, null, null);
            Assert.IsTrue(instance.ReceivedUSGovtFunds);
        }

        [TestMethod]
        public void TestGetFinancialInfo_ReceivedZeroFundsFromUSGovAgencies()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingGovtAgency1 = 0.0m,
                FundingGovtAgency2 = 0.0m,
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, null, null);
            Assert.IsFalse(instance.ReceivedUSGovtFunds);
        }

        [TestMethod]
        public void TestGetFinancialInfo_USGovAgenciesFundingsAreNull()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingGovtAgency1 = null,
                FundingGovtAgency2 = null,
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, null, null);
            Assert.IsFalse(instance.ReceivedUSGovtFunds);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingOtherDoesNotHaveAValue()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingOther = null
            };
            var orgFunding = new International
            {
                Amount1 = "1"
            };
            var usGovFunding = new USGovt
            {
                Amount1 = "2"
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNotNull(instance.OtherFunds);
            Assert.IsNull(instance.OtherFunds.Other);
        }

        [TestMethod]
        public void TestGetFinancialInfo_FundingOtherZero()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                FundingOther = 0m
            };
            var orgFunding = new International
            {
                Amount1 = "1"
            };
            var usGovFunding = new USGovt
            {
                Amount1 = "2"
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNotNull(instance.OtherFunds);
            Assert.IsNull(instance.OtherFunds.Other);
        }

        [TestMethod]
        public void TestGetFinancialInfo_InternationalFundingAmount1IsNull()
        {
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
                Amount1 = null
            };
            var usGovFunding = new USGovt
            {
                Amount1 = "2"
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNotNull(instance.OtherFunds);
            Assert.IsNull(instance.OtherFunds.International);
        }

        [TestMethod]
        public void TestGetFinancialInfo_InternationalFundingAmount2IsNotNull()
        {
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
                Amount1 = null,
                Amount2 = "1"
            };
            var usGovFunding = new USGovt
            {
                Amount1 = "2"
            };
            var message = "The International Funding Amount1 must have a value if Amount2 has a value.";
            Action a = () => service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }


        [TestMethod]
        public void TestGetFinancialInfo_USGovernmentFundingAmount1IsNull()
        {
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
                Amount1 = "1"
            };
            var usGovFunding = new USGovt
            {
                Amount1 = null
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            Assert.IsNotNull(instance.OtherFunds);
            Assert.IsNull(instance.OtherFunds.USGovt);
        }

        [TestMethod]
        public void TestGetFinancialInfo_USGoverntmentFundingAmount2IsNotNull()
        {
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
                Amount1 = "11",
                Amount2 = "1"
            };
            var usGovFunding = new USGovt
            {
                Amount1 = null,
                Amount2 = "1"
            };
            var message = "The US Government Funding Amount1 must have a value if Amount2 has a value.";
            Action a = () => service.GetFinancialInfo(participantExchangeVisitor, orgFunding, usGovFunding);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestGetFinancialInfo_OrgFundingIsNull()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
            };
            var usGovFunding = new USGovt
            {
                Amount1 = "1"
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, null, usGovFunding);
            Assert.IsNull(instance.OtherFunds.International);
            Assert.IsNotNull(instance.OtherFunds.USGovt);
        }

        [TestMethod]
        public void TestGetFinancialInfo_USGovtFundingIsNull()
        {
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
            };
            var international = new International
            {
                Amount1 = "1"
            };
            var instance = service.GetFinancialInfo(participantExchangeVisitor, international, null);
            Assert.IsNotNull(instance.OtherFunds.International);
            Assert.IsNull(instance.OtherFunds.USGovt);
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

        #region SetResidentialAddress
        [TestMethod]
        public void TestSetResidentialAddress()
        {
            var instance = new ExchangeVisitor();
            instance.ResidentialAddress = new ResidentialAddress();

            service.SetResidentialAddress(instance);
            Assert.IsNull(instance.ResidentialAddress);
        }
        #endregion

        #region SetFinancialInfo
        [TestMethod]
        public async Task TestSetFinancialInfo_CheckProperties()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
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
                FundingPersonal = 5.5m,
                FundingOther = 6.6m,
                OtherName = "other name"

            };
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            Action<ExchangeVisitor> tester = (instance) =>
            {
                Assert.AreEqual("2", instance.FinancialInfo.ProgramSponsorFunds);
                Assert.AreEqual("3", instance.FinancialInfo.OtherFunds.EVGovt);
                Assert.AreEqual("4", instance.FinancialInfo.OtherFunds.BinationalCommission);
                Assert.AreEqual("5", instance.FinancialInfo.OtherFunds.Personal);
                Assert.AreEqual("6", instance.FinancialInfo.OtherFunds.Other.Amount);
                Assert.AreEqual(visitor.OtherName, instance.FinancialInfo.OtherFunds.Other.Name);
            };
            var exchangeVisitor = new ExchangeVisitor();
            var exchangeVisitorAsync = new ExchangeVisitor();

            service.SetFinancialInfo(exchangeVisitor, visitor);
            tester(exchangeVisitor);
            await service.SetFinancialInfoAsync(exchangeVisitorAsync, visitor);
            tester(exchangeVisitorAsync);
        }
        #endregion

        #region SetFinancialInfoUpdate
        [TestMethod]
        public async Task TestSetFinancialInfoUpdate_CheckProperties()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
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
            Action<ExchangeVisitorUpdate> tester = (instance) =>
            {
                Assert.AreEqual("2", instance.FinancialInfo.ProgramSponsorFunds);
                Assert.AreEqual("3", instance.FinancialInfo.OtherFunds.EVGovt);
                Assert.AreEqual("4", instance.FinancialInfo.OtherFunds.BinationalCommission);
                Assert.AreEqual("5", instance.FinancialInfo.OtherFunds.Personal);
                Assert.IsTrue(instance.FinancialInfo.printForm);
            };
            var exchangeVisitor = new ExchangeVisitorUpdate();
            var exchangeVisitorAsync = new ExchangeVisitorUpdate();

            service.SetFinancialInfoUpdate(exchangeVisitor, visitor);
            tester(exchangeVisitor);
            await service.SetFinancialInfoUpdateAsync(exchangeVisitorAsync, visitor);
            tester(exchangeVisitorAsync);
        }
        #endregion

        #region SetTIPP
        [TestMethod]
        public void TestSetTIPP()
        {
            var instance = new ExchangeVisitor();
            instance.AddTIPP = null;
            service.SetTIPP(instance);
            Assert.IsInstanceOfType(instance.AddTIPP, typeof(EcaAddTIPP));
            foreach(var dependent in instance.CreateDependent)
            {
                Assert.IsInstanceOfType(dependent, typeof(EcaAddTIPP));
            }
        }
        #endregion

        #region GetCreateExchVisitor
        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_CheckExchangeVisitor()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
                Gender = gender,
                GenderId = gender.GenderId
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
                ParticipantId = participant.ParticipantId,
                StartDate = yesterday,
                EndDate = endDate
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
            context.Genders.Add(gender);
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            Action<CreateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor);
                Assert.IsNotNull(instance.ExchangeVisitor.CreateDependent);
                Assert.AreEqual(0, instance.ExchangeVisitor.CreateDependent.Count());
            };
            var serviceResult = service.GetCreateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            var serviceResultAsync = await service.GetCreateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_ParticipantExchangeVisitorIsNull_CheckFinancialInfoIsNull()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
                Gender = gender,
                GenderId = gender.GenderId
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
                ParticipantId = participant.ParticipantId,
                StartDate = yesterday,
                EndDate = endDate
            };
            participant.ParticipantPerson = participantPerson;
            context.Genders.Add(gender);
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            Action<CreateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor);
                Assert.IsNull(instance.ExchangeVisitor.FinancialInfo);
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
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 10,
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth,
                GenderId = gender.GenderId,
                Gender = gender
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
            context.Genders.Add(gender);
            context.Locations.Add(cityOfBirth);
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
        public async Task TestGetCreateExchangeVisitor_CheckTIPP()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 10,
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth,
                GenderId = gender.GenderId,
                Gender = gender
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
            context.Genders.Add(gender);
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            Action<CreateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor);
                Assert.IsNotNull(instance.ExchangeVisitor.AddTIPP);
                Assert.IsInstanceOfType(instance.ExchangeVisitor.AddTIPP, typeof(EcaAddTIPP));
                foreach(var dependent in instance.ExchangeVisitor.CreateDependent)
                {
                    Assert.IsInstanceOfType(dependent, typeof(EcaAddTIPP));
                }
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
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var fieldOfStudy = new FieldOfStudy
            {
                Description = "desc",
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
                Gender = gender,
                GenderId = gender.GenderId
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
            context.Genders.Add(gender);
            context.Locations.Add(cityOfBirth);
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
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 505,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth,
                GenderId = gender.GenderId,
                Gender = gender
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
            context.Locations.Add(cityOfBirth);
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
            context.People.Add(person);
            context.Genders.Add(gender);

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
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 505,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
                Gender = gender,
                GenderId = gender.GenderId
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
            context.Locations.Add(cityOfBirth);
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
            context.People.Add(person);
            context.Genders.Add(gender);

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
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth,
                GenderId = gender.GenderId,
                Gender = gender
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
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            context.Genders.Add(gender);
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
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
                Gender = gender,
                GenderId = gender.GenderId
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
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);
            context.Genders.Add(gender);
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

        [TestMethod]
        public async Task TestGetCreateExchangeVisitorAsync_ProjectIsNotAnExchangeVisitorProject()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.Locations.Add(cityOfBirth);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var message = String.Format("The participant with id [{0}] belongs to a project with id [{1}] that is not an exchange visitor project.", participant.ParticipantId, project.ProjectId);
            Action a = () => service.GetCreateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetCreateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }


        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_ProjectDoesNotExist()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId - 1,
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
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);
            context.ParticipantPersons.Add(participantPerson);
            context.ParticipantExchangeVisitors.Add(visitor);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Project).Name, participant.ProjectId);
            Action a = () => service.GetCreateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetCreateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);

            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }


        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_ParticipantPersonDoesNotExist()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(ParticipantPerson).Name, participant.ParticipantId);
            Action a = () => service.GetCreateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetCreateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);

            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_ParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId
            };
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Projects.Add(project);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, 1);
            Action a = () => service.GetCreateExchangeVisitor(user, project.ProjectId, 1);
            Func<Task> f = () => service.GetCreateExchangeVisitorAsync(user, project.ProjectId, 1);

            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_ParticipantPersonDoesBelongToProject()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 10,
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                Person = person,
                PersonId = person.PersonId
            };
            context.Locations.Add(cityOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.Projects.Add(project);

            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        participant.ProjectId + 1);
            Action a = () => service.GetCreateExchangeVisitor(user, participant.ProjectId + 1, participant.ParticipantId);
            Func<Task> f = () => service.GetCreateExchangeVisitorAsync(user, participant.ProjectId + 1, participant.ParticipantId);

            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetCreateExchangeVisitor_ParticipantIsNotAPerson()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var endDate = DateTimeOffset.Now.AddDays(20.0);

            var user = new User(2);
            var project = new Project
            {
                ProjectId = 3,
                StartDate = yesterday,
                EndDate = endDate,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var organization = new Organization
            {
                OrganizationId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId,
                OrganizationId = organization.OrganizationId,
                Organization = organization
            };
            context.Organizations.Add(organization);
            context.Participants.Add(participant);
            context.Projects.Add(project);

            var message = String.Format("The participant with id [0] is not a person participant.", participant.ParticipantId);
            Action a = () => service.GetCreateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetCreateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);

            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        #endregion

        #region GetExchangeVisitorUpdate
        [TestMethod]
        public void TestGetExchangeVisitorUpdate()
        {
            var person = new Person
            {
                PersonId = 20

            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
            };
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            participant.ParticipantPerson = participantPerson;

            var instance = service.GetExchangeVisitorUpdate(participant, user, participantPerson);
            Assert.IsNotNull(instance);
            Assert.AreEqual(participantPerson.SevisId, instance.sevisID);
            Assert.AreEqual(participant.ParticipantId.ToString(), instance.requestID.ToString());

            Assert.IsNotNull(instance.Reprint);
            Assert.IsTrue(instance.Reprint.printForm);
            Assert.AreEqual(ExchangeVisitorService.REPRINT_FORM_UPDATE_REASON_CODE, instance.Reprint.Reason);

            Assert.IsNotNull(instance.Reprint7002);
            Assert.IsFalse(instance.Reprint7002.print7002);
        }

        [TestMethod]
        public void TestGetExchangeVisitorUpdate_SevisIdIsEmpty()
        {
            var person = new Person
            {
                PersonId = 20

            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
            };
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = String.Empty
            };
            participant.ParticipantPerson = participantPerson;
            var message = String.Format("The participant with id [{0}] does not have a sevis id.  The update can not take place.", participant.ParticipantId);
            Action a = () => service.GetExchangeVisitorUpdate(participant, user, participantPerson);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestGetExchangeVisitorUpdate_SevisIdIsWhitespace()
        {
            var person = new Person
            {
                PersonId = 20

            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
            };
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = " "
            };
            participant.ParticipantPerson = participantPerson;
            var message = String.Format("The participant with id [{0}] does not have a sevis id.  The update can not take place.", participant.ParticipantId);
            Action a = () => service.GetExchangeVisitorUpdate(participant, user, participantPerson);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }


        [TestMethod]
        public void TestGetExchangeVisitorUpdate_SevisIdIsNull()
        {
            var person = new Person
            {
                PersonId = 20

            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
            };
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = null
            };
            participant.ParticipantPerson = participantPerson;
            var message = String.Format("The participant with id [{0}] does not have a sevis id.  The update can not take place.", participant.ParticipantId);
            Action a = () => service.GetExchangeVisitorUpdate(participant, user, participantPerson);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }
        #endregion

        #region SetBiographyUpdate
        [TestMethod]
        public async Task TestSetBiographyUpdate_CheckProperties()
        {

            var project = new Project
            {
                ProjectId = 1
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                LastName = "lastName",
                GenderId = gender.GenderId,
                Gender = gender
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);

            Action<ExchangeVisitorUpdate> tester = (instance) =>
            {
                Assert.IsNotNull(instance.Biographical);
                Assert.IsNotNull(instance.Biographical.FullName);
                Assert.AreEqual(person.FirstName, instance.Biographical.FullName.FirstName);
                Assert.AreEqual(person.LastName, instance.Biographical.FullName.LastName);
            };

            var visitor = new ExchangeVisitorUpdate();
            var asyncVisitor = new ExchangeVisitorUpdate();
            service.SetBiographyUpdate(participant, participantPerson, visitor);
            await service.SetBiographyUpdateAsync(participant, participantPerson, asyncVisitor);
            tester(visitor);
            tester(asyncVisitor);
        }

        [TestMethod]
        public async Task TestSetBiographyUpdate_BiographyNotFound()
        {

            var project = new Project
            {
                ProjectId = 1
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                LastName = "lastName",
                Gender = gender,
                GenderId = gender.GenderId
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
            };
            participant.ParticipantPerson = participantPerson;
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);

            ExchangeVisitorUpdate visitor = new ExchangeVisitorUpdate();
            var otherParticipant = new Participant
            {
                ParticipantId = participant.ParticipantId + 1
            };

            Assert.IsNull(ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(this.context, otherParticipant.ParticipantId).FirstOrDefault());


            var message = String.Format("The participant with id [{0}] must have biographical information.", otherParticipant.ParticipantId);
            Action a = () => service.SetBiographyUpdate(otherParticipant, participantPerson, visitor);
            Func<Task> f = () => service.SetBiographyUpdateAsync(otherParticipant, participantPerson, visitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        #endregion

        #region Set Dependents
        [TestMethod]
        public async Task TestSetDependents_ExchangeVisitorUpdate()
        {
            var participant = new Participant
            {
                ParticipantId = 1,
            };
            ExchangeVisitorUpdate exchangeVisitorUpdate = null;
            context.SetupActions.Add(() =>
            {
                exchangeVisitorUpdate = new ExchangeVisitorUpdate
                {

                };
            });
            Action tester = () =>
            {
                Assert.IsNull(exchangeVisitorUpdate.Dependent);
            };
            context.Revert();
            service.SetDependents(participant, exchangeVisitorUpdate);
            tester();

            context.Revert();
            await service.SetDependentsAsync(participant, exchangeVisitorUpdate);
            tester();
        }

        [TestMethod]
        public async Task TestSetDependents_ExchangeVisitor()
        {
            var participant = new Participant
            {
                ParticipantId = 1,
            };
            ExchangeVisitor exchangeVisitorUpdate = null;
            context.SetupActions.Add(() =>
            {
                exchangeVisitorUpdate = new ExchangeVisitor
                {

                };
            });
            Action tester = () =>
            {
                Assert.IsNotNull(exchangeVisitorUpdate.CreateDependent);
                Assert.AreEqual(0, exchangeVisitorUpdate.CreateDependent.Count());
            };
            context.Revert();
            service.SetDependents(participant, exchangeVisitorUpdate);
            tester();

            context.Revert();
            await service.SetDependentsAsync(participant, exchangeVisitorUpdate);
            tester();
        }
        #endregion

        #region GetUpdateExchangeVisitor
        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_CheckExchangeVisitorUpdateProperty()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 20,
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth,
                GenderId = gender.GenderId,
                Gender = gender
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.Locations.Add(cityOfBirth);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);

            Action<UpdateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.AreEqual(participantPerson.SevisId, instance.ExchangeVisitor.sevisID);
                Assert.AreEqual(participant.ParticipantId.ToString(), instance.ExchangeVisitor.requestID.ToString());
                Assert.IsNotNull(instance.ExchangeVisitor.Reprint);
                Assert.IsNotNull(instance.ExchangeVisitor.Reprint7002);
            };

            var result = service.GetUpdateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            var resultAsync = await service.GetUpdateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_CheckSetDependents()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 20,
                PlaceOfBirthId = cityOfBirth.LocationId,
                PlaceOfBirth = cityOfBirth,
                GenderId = gender.GenderId,
                Gender = gender
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.Locations.Add(cityOfBirth);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);

            Action<UpdateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNull(instance.ExchangeVisitor.Dependent);
            };

            var result = service.GetUpdateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            tester(result);

            var resultAsync = await service.GetUpdateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_CheckBiographicalUpdateProperty()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
                Gender = gender,
                GenderId = gender.GenderId
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.Locations.Add(cityOfBirth);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            Action<UpdateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor.Biographical);
                Assert.AreEqual(person.FirstName, instance.ExchangeVisitor.Biographical.FullName.FirstName);
            };

            var result = service.GetUpdateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            var resultAsync = await service.GetUpdateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_CheckMailingAddressProperty()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            }; var addressLocationType = new LocationType
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
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                Gender = gender,
                GenderId = gender.GenderId
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                HomeInstitutionAddressId = address.AddressId
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.LocationTypes.Add(addressLocationType);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            Action<UpdateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor.MailAddress);
                Assert.AreEqual(addressLocation.Street1, instance.ExchangeVisitor.MailAddress.Address1);
                Assert.AreEqual(addressLocation.Street2, instance.ExchangeVisitor.MailAddress.Address2);
                Assert.AreEqual(city.LocationName, instance.ExchangeVisitor.MailAddress.City);
                Assert.AreEqual(division.LocationName, instance.ExchangeVisitor.MailAddress.State);
                Assert.AreEqual(addressLocation.PostalCode, instance.ExchangeVisitor.MailAddress.PostalCode);
            };

            var result = service.GetUpdateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            var resultAsync = await service.GetUpdateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_CheckUSAddressProperty()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            }; var addressLocationType = new LocationType
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
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                Gender = gender,
                GenderId = gender.GenderId
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                HostInstitutionAddressId = address.AddressId
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.LocationTypes.Add(addressLocationType);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            Action<UpdateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor.USAddress);
                Assert.AreEqual(addressLocation.Street1, instance.ExchangeVisitor.USAddress.Address1);
                Assert.AreEqual(addressLocation.Street2, instance.ExchangeVisitor.USAddress.Address2);
                Assert.AreEqual(city.LocationName, instance.ExchangeVisitor.USAddress.City);
                Assert.AreEqual(division.LocationName, instance.ExchangeVisitor.USAddress.State);
                Assert.AreEqual(addressLocation.PostalCode, instance.ExchangeVisitor.USAddress.PostalCode);
            };

            var result = service.GetUpdateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            var resultAsync = await service.GetUpdateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_CheckFinancialInfoUpdateProperty()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
                GenderId = gender.GenderId,
                Gender = gender
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,
                FundingGovtAgency1 = 1.0m,
                FundingSponsor = 2.2m,
                FundingVisGovt = 3.3m,
                FundingVisBNC = 4.4m,
                FundingPersonal = 5.5m

            };
            participant.ParticipantPerson = participantPerson;
            context.Locations.Add(cityOfBirth);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);

            Action<UpdateExchVisitor> tester = (instance) =>
            {
                Assert.IsNotNull(instance);
                Assert.IsNotNull(instance.ExchangeVisitor.FinancialInfo);
                Assert.AreEqual("2", instance.ExchangeVisitor.FinancialInfo.ProgramSponsorFunds);
                Assert.AreEqual("3", instance.ExchangeVisitor.FinancialInfo.OtherFunds.EVGovt);
                Assert.AreEqual("4", instance.ExchangeVisitor.FinancialInfo.OtherFunds.BinationalCommission);
                Assert.AreEqual("5", instance.ExchangeVisitor.FinancialInfo.OtherFunds.Personal);
                Assert.IsTrue(instance.ExchangeVisitor.FinancialInfo.printForm);
            };

            var result = service.GetUpdateExchangeVisitor(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            var resultAsync = await service.GetUpdateExchangeVisitorAsync(user, project.ProjectId, participant.ParticipantId);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_ProjectDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = 2,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.Locations.Add(cityOfBirth);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Project).Name, participant.ProjectId);
            Action a = () => service.GetUpdateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetUpdateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_ProjectIsNotAnExchangeVisitorProject()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            var participantExchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                ParticipantPerson = participantPerson,

            };
            participant.ParticipantPerson = participantPerson;
            context.Locations.Add(cityOfBirth);
            context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var message = String.Format("The participant with id [{0}] belongs to a project with id [{1}] that is not an exchange visitor project.", participant.ParticipantId, project.ProjectId);
            Action a = () => service.GetUpdateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetUpdateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_ParticipantExchangeVisitorDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234"
            };
            participant.ParticipantPerson = participantPerson;
            context.Locations.Add(cityOfBirth);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(ParticipantExchangeVisitor).Name, participant.ParticipantId);
            Action a = () => service.GetUpdateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetUpdateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_ParticipantPersonDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var cityOfBirth = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.City.Id,
            };
            var person = new Person
            {
                PersonId = 20,
                FirstName = "firstName",
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId,
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                Person = person,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            context.Locations.Add(cityOfBirth);
            context.Projects.Add(project);
            context.People.Add(person);
            context.Participants.Add(participant);

            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(ParticipantPerson).Name, participant.ParticipantId);
            Action a = () => service.GetUpdateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetUpdateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_ParticipantIsNotAPerson()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            context.Projects.Add(project);
            context.Participants.Add(participant);

            var message = String.Format("The participant with id [0] is not a person participant.", participant.ParticipantId);
            Action a = () => service.GetUpdateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
            Func<Task> f = () => service.GetUpdateExchangeVisitorAsync(user, participant.ProjectId, participant.ParticipantId);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_ParticipantDoesNotBelongToProject()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var participant = new Participant
            {
                ParticipantId = 10,
                ProjectId = project.ProjectId,
                Project = project
            };
            project.Participants.Add(participant);
            var user = new User(100);
            context.Projects.Add(project);
            context.Participants.Add(participant);

            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        -1);
            Action a = () => service.GetUpdateExchangeVisitor(user, -1, participant.ParticipantId);
            Func<Task> f = () => service.GetUpdateExchangeVisitorAsync(user, -1, participant.ParticipantId);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetUpdateExchangeVisitorAsync_ParticipantDoesNotExist()
        {
            var user = new User(100);
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, 1);
            Action a = () => service.GetUpdateExchangeVisitor(user, 1, 1);
            Func<Task> f = () => service.GetUpdateExchangeVisitorAsync(user, 1, 1);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion
    }
}
