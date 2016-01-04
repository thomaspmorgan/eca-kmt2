using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Itineraries;
using Moq;
using ECA.Business.Validation;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Business.Service;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryStopServiceTest
    {
        private TestEcaContext context;
        private ItineraryStopService service;
        private Mock<IBusinessValidator<EcaItineraryStopValidationEntity, EcaItineraryStopValidationEntity>> validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new Mock<IBusinessValidator<EcaItineraryStopValidationEntity, EcaItineraryStopValidationEntity>>();
            context = new TestEcaContext();
            service = new ItineraryStopService(context, validator.Object);
        }

        #region Create
        [TestMethod]
        public async Task TestCreate_CheckProperties()
        {
            var projectId = 1;
            var creatorId = 2;
            var itineraryCreatorId = 100;
            var itineraryId = 3;
            var locationId = 4;
            var creator = new User(creatorId);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var project = new Project
            {
                ProjectId = projectId
            };
            var destinationLocation = new Location
            {
                LocationId = locationId
            };
            Itinerary itinerary = null;

            context.SetupActions.Add(() =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Project = project,
                    ProjectId = projectId,
                    StartDate = DateTimeOffset.UtcNow.AddDays(-100.0),
                    EndDate = DateTimeOffset.UtcNow.AddDays(100.0)
                };
                itinerary.History.CreatedOn = yesterday;
                itinerary.History.RevisedOn = yesterday;
                itinerary.History.CreatedBy = itineraryCreatorId;
                itinerary.History.RevisedBy = itineraryCreatorId;
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
                context.Locations.Add(destinationLocation);
            });

            var addedItineraryStop = new AddedEcaItineraryStop(
                creator: creator,
                itineraryId: itineraryId,
                projectId: projectId,
                name: "Name",
                arrivalDate: DateTimeOffset.UtcNow,
                departureDate: DateTimeOffset.UtcNow.AddDays(1.0),
                destinationLocationId: locationId);

            Action<ItineraryStop> tester = (serviceResult) =>
            {
                Assert.AreEqual(1, context.ItineraryStops.Count());
                var stop = context.ItineraryStops.First();
                Assert.IsTrue(Object.ReferenceEquals(serviceResult, stop));

                Assert.AreEqual(addedItineraryStop.ArrivalDate, stop.DateArrive);
                Assert.AreEqual(addedItineraryStop.DepartureDate, stop.DateLeave);
                Assert.AreEqual(addedItineraryStop.DestinationLocationId, stop.DestinationId);
                Assert.AreEqual(addedItineraryStop.ItineraryId, itineraryId);
                Assert.AreEqual(addedItineraryStop.Name, stop.Name);
                Assert.AreEqual(ItineraryStatus.InProgress.Id, stop.ItineraryStatusId);

                DateTimeOffset.Now.Should().BeCloseTo(stop.History.CreatedOn, 20000);
                DateTimeOffset.Now.Should().BeCloseTo(stop.History.RevisedOn, 20000);
                Assert.AreEqual(creatorId, stop.History.CreatedBy);
                Assert.AreEqual(creatorId, stop.History.RevisedBy);

                var updatedItinerary = context.Itineraries.First();
                Assert.AreEqual(itineraryCreatorId, updatedItinerary.History.CreatedBy);
                Assert.AreEqual(creatorId, updatedItinerary.History.RevisedBy);
                Assert.AreEqual(yesterday, updatedItinerary.History.CreatedOn);
                DateTimeOffset.Now.Should().BeCloseTo(stop.History.RevisedOn, 20000);
            };

            Action<EcaItineraryStopValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.IsNotNull(entity);
                Assert.AreEqual(itinerary.StartDate, entity.ItineraryStartDate);
                Assert.AreEqual(itinerary.EndDate, entity.ItineraryEndDate);
                Assert.AreEqual(addedItineraryStop.ArrivalDate, entity.ItineraryStopArrivalDate);
                Assert.AreEqual(addedItineraryStop.DepartureDate, entity.ItineraryStopDepartureDate);
            };

            validator.Setup(x => x.ValidateCreate(It.IsAny<EcaItineraryStopValidationEntity>())).Callback(validationEntityTester);

            context.Revert();
            var result = service.Create(addedItineraryStop);
            tester(result);

            context.Revert();
            result = await service.CreateAsync(addedItineraryStop);
            tester(result);

            validator.Verify(x => x.ValidateCreate(It.IsAny<EcaItineraryStopValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestCreate_ItineraryDoesNotExist()
        {
            var projectId = 1;
            var creatorId = 2;
            var itineraryCreatorId = 100;
            var itineraryId = 3;
            var locationId = 4;
            var creator = new User(creatorId);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var project = new Project
            {
                ProjectId = projectId
            };
            var destinationLocation = new Location
            {
                LocationId = locationId
            };
            Itinerary itinerary = null;

            context.SetupActions.Add(() =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Project = project,
                    ProjectId = projectId
                };
                itinerary.History.CreatedOn = yesterday;
                itinerary.History.RevisedOn = yesterday;
                itinerary.History.CreatedBy = itineraryCreatorId;
                itinerary.History.RevisedBy = itineraryCreatorId;
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
                context.Locations.Add(destinationLocation);
            });

            var addedItineraryStop = new AddedEcaItineraryStop(
                creator: creator,
                itineraryId: itineraryId + 1,
                projectId: projectId,
                name: "Name",
                arrivalDate: DateTimeOffset.UtcNow,
                departureDate: DateTimeOffset.UtcNow.AddDays(1.0),
                destinationLocationId: locationId);
            context.Revert();
            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Itinerary).Name, addedItineraryStop.ItineraryId);
            Action a = () => service.Create(addedItineraryStop);
            Func<Task> f = () => service.CreateAsync(addedItineraryStop);

            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreate_LocationDoesNotExist()
        {
            var projectId = 1;
            var creatorId = 2;
            var itineraryCreatorId = 100;
            var itineraryId = 3;
            var locationId = 4;
            var creator = new User(creatorId);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var project = new Project
            {
                ProjectId = projectId
            };
            var destinationLocation = new Location
            {
                LocationId = locationId
            };
            Itinerary itinerary = null;

            context.SetupActions.Add(() =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Project = project,
                    ProjectId = projectId
                };
                itinerary.History.CreatedOn = yesterday;
                itinerary.History.RevisedOn = yesterday;
                itinerary.History.CreatedBy = itineraryCreatorId;
                itinerary.History.RevisedBy = itineraryCreatorId;
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
                context.Locations.Add(destinationLocation);
            });

            var addedItineraryStop = new AddedEcaItineraryStop(
                creator: creator,
                itineraryId: itineraryId,
                projectId: projectId,
                name: "Name",
                arrivalDate: DateTimeOffset.UtcNow,
                departureDate: DateTimeOffset.UtcNow.AddDays(1.0),
                destinationLocationId: locationId + 1);
            context.Revert();
            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Location).Name, addedItineraryStop.DestinationLocationId);
            Action a = () => service.Create(addedItineraryStop);
            Func<Task> f = () => service.CreateAsync(addedItineraryStop);

            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreate_ItineraryDoesNotBelongToProject()
        {
            var projectId = 1;
            var creatorId = 2;
            var itineraryCreatorId = 100;
            var itineraryId = 3;
            var locationId = 4;
            var creator = new User(creatorId);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var project = new Project
            {
                ProjectId = projectId
            };
            var destinationLocation = new Location
            {
                LocationId = locationId
            };
            Itinerary itinerary = null;

            context.SetupActions.Add(() =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Project = project,
                    ProjectId = projectId
                };
                itinerary.History.CreatedOn = yesterday;
                itinerary.History.RevisedOn = yesterday;
                itinerary.History.CreatedBy = itineraryCreatorId;
                itinerary.History.RevisedBy = itineraryCreatorId;
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
                context.Locations.Add(destinationLocation);
            });

            var addedItineraryStop = new AddedEcaItineraryStop(
                creator: creator,
                itineraryId: itineraryId,
                projectId: projectId + 1,
                name: "Name",
                arrivalDate: DateTimeOffset.UtcNow,
                departureDate: DateTimeOffset.UtcNow.AddDays(1.0),
                destinationLocationId: locationId);
            context.Revert();
            var message = String.Format("The user with id [{0}] attempted to edit an itinerary on a project with id [{1}] but should have been denied access.",
                        creatorId,
                        addedItineraryStop.ProjectId);
            Action a = () => service.Create(addedItineraryStop);
            Func<Task> f = () => service.CreateAsync(addedItineraryStop);

            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_CheckProperties()
        {
            var projectId = 1;
            var creatorId = 2;
            var updatorId = 10;
            var itineraryCreatorId = 100;
            var itineraryId = 3;
            var locationId = 4;
            var itineraryStopId = 5;
            var updator = new User(updatorId);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var project = new Project
            {
                ProjectId = projectId
            };
            var destinationLocation = new Location
            {
                LocationId = locationId
            };
            Itinerary itinerary = null;
            ItineraryStop itineraryStop = null;

            context.SetupActions.Add(() =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Project = project,
                    ProjectId = projectId,
                    StartDate = DateTimeOffset.UtcNow.AddDays(-100.0),
                    EndDate = DateTimeOffset.UtcNow.AddDays(100.0)
                };
                itinerary.History.CreatedOn = yesterday;
                itinerary.History.RevisedOn = yesterday;
                itinerary.History.CreatedBy = itineraryCreatorId;
                itinerary.History.RevisedBy = itineraryCreatorId;

                itineraryStop = new ItineraryStop
                {
                    ItineraryStopId = itineraryStopId,
                    Itinerary = itinerary,
                    ItineraryId = itinerary.ItineraryId,
                };
                itineraryStop.History.CreatedOn = yesterday;
                itineraryStop.History.RevisedOn = yesterday;
                itineraryStop.History.CreatedBy = creatorId;
                itineraryStop.History.RevisedBy = creatorId;

                context.ItineraryStops.Add(itineraryStop);
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
                context.Locations.Add(destinationLocation);
            });

            var updatedItineraryStop = new UpdatedEcaItineraryStop(
                updator: updator,
                itineraryStopId: itineraryStopId,
                projectId: projectId,
                name: "Name",
                arrivalDate: DateTimeOffset.UtcNow,
                departureDate: DateTimeOffset.UtcNow.AddDays(1.0),
                destinationLocationId: locationId);
            Action beforeTester = () =>
            {
                Assert.IsNull(itineraryStop.Name);
                Assert.IsNull(itineraryStop.DestinationId);
                Assert.AreNotEqual(updatedItineraryStop.ArrivalDate, itineraryStop.DateArrive);
                Assert.AreNotEqual(updatedItineraryStop.DepartureDate, itineraryStop.DateLeave);
            };
            Action tester = () =>
            {
                Assert.AreEqual(1, context.ItineraryStops.Count());
                var stop = context.ItineraryStops.First();

                Assert.AreEqual(updatedItineraryStop.ArrivalDate, stop.DateArrive);
                Assert.AreEqual(updatedItineraryStop.DepartureDate, stop.DateLeave);
                Assert.AreEqual(updatedItineraryStop.DestinationLocationId, stop.DestinationId);
                Assert.AreEqual(updatedItineraryStop.Name, stop.Name);

                Assert.AreEqual(yesterday, stop.History.CreatedOn);
                DateTimeOffset.Now.Should().BeCloseTo(stop.History.RevisedOn, 20000);
                Assert.AreEqual(creatorId, stop.History.CreatedBy);
                Assert.AreEqual(updatorId, stop.History.RevisedBy);

                var updatedItinerary = context.Itineraries.First();
                Assert.AreEqual(itineraryCreatorId, updatedItinerary.History.CreatedBy);
                Assert.AreEqual(updatorId, updatedItinerary.History.RevisedBy);
                Assert.AreEqual(yesterday, updatedItinerary.History.CreatedOn);
                DateTimeOffset.Now.Should().BeCloseTo(stop.History.RevisedOn, 20000);
            };

            Action<EcaItineraryStopValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.IsNotNull(entity);
                Assert.AreEqual(itinerary.StartDate, entity.ItineraryStartDate);
                Assert.AreEqual(itinerary.EndDate, entity.ItineraryEndDate);
                Assert.AreEqual(updatedItineraryStop.ArrivalDate, entity.ItineraryStopArrivalDate);
                Assert.AreEqual(updatedItineraryStop.DepartureDate, entity.ItineraryStopDepartureDate);
            };

            validator.Setup(x => x.ValidateUpdate(It.IsAny<EcaItineraryStopValidationEntity>())).Callback(validationEntityTester);

            context.Revert();
            beforeTester();
            service.Update(updatedItineraryStop);
            tester();

            context.Revert();
            beforeTester();
            await service.UpdateAsync(updatedItineraryStop);
            tester();

            validator.Verify(x => x.ValidateUpdate(It.IsAny<EcaItineraryStopValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestUpdate_ItineraryStopDoesNotExist()
        {
            var projectId = 1;
            var creatorId = 2;
            var updatorId = 10;
            var itineraryCreatorId = 100;
            var itineraryId = 3;
            var locationId = 4;
            var itineraryStopId = 5;
            var updator = new User(updatorId);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var project = new Project
            {
                ProjectId = projectId
            };
            var destinationLocation = new Location
            {
                LocationId = locationId
            };
            Itinerary itinerary = null;
            ItineraryStop itineraryStop = null;

            context.SetupActions.Add(() =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Project = project,
                    ProjectId = projectId,
                    StartDate = DateTimeOffset.UtcNow.AddDays(-100.0),
                    EndDate = DateTimeOffset.UtcNow.AddDays(100.0)
                };
                itinerary.History.CreatedOn = yesterday;
                itinerary.History.RevisedOn = yesterday;
                itinerary.History.CreatedBy = itineraryCreatorId;
                itinerary.History.RevisedBy = itineraryCreatorId;

                itineraryStop = new ItineraryStop
                {
                    ItineraryStopId = itineraryStopId,
                    Itinerary = itinerary,
                    ItineraryId = itinerary.ItineraryId,
                };
                itineraryStop.History.CreatedOn = yesterday;
                itineraryStop.History.RevisedOn = yesterday;
                itineraryStop.History.CreatedBy = creatorId;
                itineraryStop.History.RevisedBy = creatorId;

                context.ItineraryStops.Add(itineraryStop);
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
                context.Locations.Add(destinationLocation);
            });

            var updatedItineraryStop = new UpdatedEcaItineraryStop(
                updator: updator,
                itineraryStopId: itineraryStopId + 1,
                projectId: projectId,
                name: "Name",
                arrivalDate: DateTimeOffset.UtcNow,
                departureDate: DateTimeOffset.UtcNow.AddDays(1.0),
                destinationLocationId: locationId);
            context.Revert();
            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(ItineraryStop).Name, updatedItineraryStop.ItineraryStopId);
            Action a = () => service.Update(updatedItineraryStop);
            Func<Task> f = () => service.UpdateAsync(updatedItineraryStop);

            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_DestinationDoesNotExist()
        {
            var projectId = 1;
            var creatorId = 2;
            var updatorId = 10;
            var itineraryCreatorId = 100;
            var itineraryId = 3;
            var locationId = 4;
            var itineraryStopId = 5;
            var updator = new User(updatorId);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var project = new Project
            {
                ProjectId = projectId
            };
            var destinationLocation = new Location
            {
                LocationId = locationId
            };
            Itinerary itinerary = null;
            ItineraryStop itineraryStop = null;

            context.SetupActions.Add(() =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Project = project,
                    ProjectId = projectId,
                    StartDate = DateTimeOffset.UtcNow.AddDays(-100.0),
                    EndDate = DateTimeOffset.UtcNow.AddDays(100.0)
                };
                itinerary.History.CreatedOn = yesterday;
                itinerary.History.RevisedOn = yesterday;
                itinerary.History.CreatedBy = itineraryCreatorId;
                itinerary.History.RevisedBy = itineraryCreatorId;

                itineraryStop = new ItineraryStop
                {
                    ItineraryStopId = itineraryStopId,
                    Itinerary = itinerary,
                    ItineraryId = itinerary.ItineraryId,
                };
                itineraryStop.History.CreatedOn = yesterday;
                itineraryStop.History.RevisedOn = yesterday;
                itineraryStop.History.CreatedBy = creatorId;
                itineraryStop.History.RevisedBy = creatorId;

                context.ItineraryStops.Add(itineraryStop);
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
                context.Locations.Add(destinationLocation);
            });

            var updatedItineraryStop = new UpdatedEcaItineraryStop(
                updator: updator,
                itineraryStopId: itineraryStopId,
                projectId: projectId,
                name: "Name",
                arrivalDate: DateTimeOffset.UtcNow,
                departureDate: DateTimeOffset.UtcNow.AddDays(1.0),
                destinationLocationId: locationId + 1);
            context.Revert();
            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Location).Name, updatedItineraryStop.DestinationLocationId);
            Action a = () => service.Update(updatedItineraryStop);
            Func<Task> f = () => service.UpdateAsync(updatedItineraryStop);

            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_ItineraryStopDoesNotBelongToProject()
        {
            var projectId = 1;
            var creatorId = 2;
            var updatorId = 10;
            var itineraryCreatorId = 100;
            var itineraryId = 3;
            var locationId = 4;
            var itineraryStopId = 5;
            var updator = new User(updatorId);
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var project = new Project
            {
                ProjectId = projectId
            };
            var destinationLocation = new Location
            {
                LocationId = locationId
            };
            Itinerary itinerary = null;
            ItineraryStop itineraryStop = null;

            context.SetupActions.Add(() =>
            {
                itinerary = new Itinerary
                {
                    ItineraryId = itineraryId,
                    Project = project,
                    ProjectId = projectId,
                    StartDate = DateTimeOffset.UtcNow.AddDays(-100.0),
                    EndDate = DateTimeOffset.UtcNow.AddDays(100.0)
                };
                itinerary.History.CreatedOn = yesterday;
                itinerary.History.RevisedOn = yesterday;
                itinerary.History.CreatedBy = itineraryCreatorId;
                itinerary.History.RevisedBy = itineraryCreatorId;

                itineraryStop = new ItineraryStop
                {
                    ItineraryStopId = itineraryStopId,
                    Itinerary = itinerary,
                    ItineraryId = itinerary.ItineraryId,
                };
                itineraryStop.History.CreatedOn = yesterday;
                itineraryStop.History.RevisedOn = yesterday;
                itineraryStop.History.CreatedBy = creatorId;
                itineraryStop.History.RevisedBy = creatorId;

                context.ItineraryStops.Add(itineraryStop);
                context.Projects.Add(project);
                context.Itineraries.Add(itinerary);
                context.Locations.Add(destinationLocation);
            });

            var updatedItineraryStop = new UpdatedEcaItineraryStop(
                updator: updator,
                itineraryStopId: itineraryStopId,
                projectId: projectId + 1,
                name: "Name",
                arrivalDate: DateTimeOffset.UtcNow,
                departureDate: DateTimeOffset.UtcNow.AddDays(1.0),
                destinationLocationId: locationId);
            context.Revert();
            var message = String.Format("The user with id [{0}] attempted to edit an itinerary on a project with id [{1}] but should have been denied access.",
                        updatorId,
                        updatedItineraryStop.ProjectId);
            Action a = () => service.Update(updatedItineraryStop);
            Func<Task> f = () => service.UpdateAsync(updatedItineraryStop);

            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }
        #endregion
    }
}
