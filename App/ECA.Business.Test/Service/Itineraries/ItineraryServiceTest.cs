using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ECA.Business.Service.Itineraries;
using ECA.Data;
using System.Collections.Generic;
using ECA.Business.Queries.Itineraries;
using Moq;
using ECA.Business.Validation;
using ECA.Business.Service;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryServiceTest
    {
        private TestEcaContext context;
        private ItineraryService service;
        private Mock<IBusinessValidator<AddedEcaItineraryValidationEntity, UpdatedEcaItineraryValidationEntity>> validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new Mock<IBusinessValidator<AddedEcaItineraryValidationEntity, UpdatedEcaItineraryValidationEntity>>();
            context = new TestEcaContext();
            service = new ItineraryService(context, validator.Object);
        }
        #region Get

        [TestMethod]
        public async Task TestGetItineraryById()
        {
            var cityLocationType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName = LocationType.City.Value
            };
            var countryLocationType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var arrival = new Location
            {
                LocationId = 1,
                LocationName = "city 1",
                LocationType = cityLocationType,
                LocationTypeId = cityLocationType.LocationTypeId
            };
            var departureDestination = new Location
            {
                LocationId = 2,
                LocationName = "country 1",
                LocationType = countryLocationType,
                LocationTypeId = countryLocationType.LocationTypeId
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                Arrival = arrival,
                ArrivalLocationId = arrival.LocationId,
                Departure = departureDestination,
                DepartureLocationId = departureDestination.LocationId,
                EndDate = DateTimeOffset.Now.AddDays(1.0),
                ItineraryId = 1,
                Name = "name",
                ProjectId = project.ProjectId,
                Project = project,
                StartDate = DateTimeOffset.Now.AddDays(-10.0),
            };
            itinerary.History.RevisedOn = DateTimeOffset.Now.AddDays(-2.0);
            context.LocationTypes.Add(cityLocationType);
            context.LocationTypes.Add(countryLocationType);
            context.Locations.Add(arrival);
            context.Locations.Add(departureDestination);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            Action<ItineraryDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);

                Assert.AreEqual(arrival.LocationId, result.ArrivalLocation.Id);
                Assert.AreEqual(departureDestination.LocationId, result.DepartureLocation.Id);
                Assert.AreEqual(project.ProjectId, result.ProjectId);
                Assert.AreEqual(itinerary.History.RevisedOn, result.LastRevisedOn);
                Assert.AreEqual(itinerary.EndDate, result.EndDate);
                Assert.AreEqual(itinerary.ItineraryId, result.Id);
                Assert.AreEqual(itinerary.Name, result.Name);
                Assert.AreEqual(itinerary.StartDate, result.StartDate);
                Assert.AreEqual(0, result.ParticipantsCount);
                Assert.AreEqual(0, result.GroupsCount);
            };

            var serviceResults = service.GetItineraryById(project.ProjectId, itinerary.ItineraryId);
            var serviceResultsAsync = await service.GetItineraryByIdAsync(project.ProjectId, itinerary.ItineraryId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryById_ItineraryByIdDoesNotExist()
        {
            var cityLocationType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName = LocationType.City.Value
            };
            var countryLocationType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var arrival = new Location
            {
                LocationId = 1,
                LocationName = "city 1",
                LocationType = cityLocationType,
                LocationTypeId = cityLocationType.LocationTypeId
            };
            var departureDestination = new Location
            {
                LocationId = 2,
                LocationName = "country 1",
                LocationType = countryLocationType,
                LocationTypeId = countryLocationType.LocationTypeId
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                Arrival = arrival,
                ArrivalLocationId = arrival.LocationId,
                Departure = departureDestination,
                DepartureLocationId = departureDestination.LocationId,
                EndDate = DateTimeOffset.Now.AddDays(1.0),
                ItineraryId = 1,
                Name = "name",
                ProjectId = project.ProjectId,
                Project = project,
                StartDate = DateTimeOffset.Now.AddDays(-10.0),
            };
            itinerary.History.RevisedOn = DateTimeOffset.Now.AddDays(-2.0);
            context.LocationTypes.Add(cityLocationType);
            context.LocationTypes.Add(countryLocationType);
            context.Locations.Add(arrival);
            context.Locations.Add(departureDestination);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            Action<ItineraryDTO> tester = (result) =>
            {
                Assert.IsNull(result);
            };

            var serviceResults = service.GetItineraryById(project.ProjectId, itinerary.ItineraryId + 1);
            var serviceResultsAsync = await service.GetItineraryByIdAsync(project.ProjectId, itinerary.ItineraryId + 1);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryById_ItineraryByProjectIdDoesNotExist()
        {
            var cityLocationType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName = LocationType.City.Value
            };
            var countryLocationType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var arrival = new Location
            {
                LocationId = 1,
                LocationName = "city 1",
                LocationType = cityLocationType,
                LocationTypeId = cityLocationType.LocationTypeId
            };
            var departureDestination = new Location
            {
                LocationId = 2,
                LocationName = "country 1",
                LocationType = countryLocationType,
                LocationTypeId = countryLocationType.LocationTypeId
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                Arrival = arrival,
                ArrivalLocationId = arrival.LocationId,
                Departure = departureDestination,
                DepartureLocationId = departureDestination.LocationId,
                EndDate = DateTimeOffset.Now.AddDays(1.0),
                ItineraryId = 1,
                Name = "name",
                ProjectId = project.ProjectId,
                Project = project,
                StartDate = DateTimeOffset.Now.AddDays(-10.0),
            };
            itinerary.History.RevisedOn = DateTimeOffset.Now.AddDays(-2.0);
            context.LocationTypes.Add(cityLocationType);
            context.LocationTypes.Add(countryLocationType);
            context.Locations.Add(arrival);
            context.Locations.Add(departureDestination);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            Action<ItineraryDTO> tester = (result) =>
            {
                Assert.IsNull(result);
            };

            var serviceResults = service.GetItineraryById(project.ProjectId + 1, itinerary.ItineraryId);
            var serviceResultsAsync = await service.GetItineraryByIdAsync(project.ProjectId + 1, itinerary.ItineraryId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItinerariesByProjectId()
        {
            var cityLocationType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName = LocationType.City.Value
            };
            var countryLocationType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var arrival = new Location
            {
                LocationId = 1,
                LocationName = "city 1",
                LocationType = cityLocationType,
                LocationTypeId = cityLocationType.LocationTypeId
            };
            var departureDestination = new Location
            {
                LocationId = 2,
                LocationName = "country 1",
                LocationType = countryLocationType,
                LocationTypeId = countryLocationType.LocationTypeId
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                Arrival = arrival,
                ArrivalLocationId = arrival.LocationId,
                Departure = departureDestination,
                DepartureLocationId = departureDestination.LocationId,
                EndDate = DateTimeOffset.Now.AddDays(1.0),
                ItineraryId = 1,
                Name = "name",
                ProjectId = project.ProjectId,
                Project = project,
                StartDate = DateTimeOffset.Now.AddDays(-10.0),
            };
            itinerary.History.RevisedOn = DateTimeOffset.Now.AddDays(-2.0);
            context.LocationTypes.Add(cityLocationType);
            context.LocationTypes.Add(countryLocationType);
            context.Locations.Add(arrival);
            context.Locations.Add(departureDestination);
            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);

            Action<List<ItineraryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count());

                var firstResult = results.First();
                Assert.AreEqual(arrival.LocationId, firstResult.ArrivalLocation.Id);
                Assert.AreEqual(departureDestination.LocationId, firstResult.DepartureLocation.Id);
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
                Assert.AreEqual(itinerary.History.RevisedOn, firstResult.LastRevisedOn);
                Assert.AreEqual(itinerary.EndDate, firstResult.EndDate);
                Assert.AreEqual(itinerary.ItineraryId, firstResult.Id);
                Assert.AreEqual(itinerary.Name, firstResult.Name);
                Assert.AreEqual(itinerary.StartDate, firstResult.StartDate);
                Assert.AreEqual(0, firstResult.ParticipantsCount);
                Assert.AreEqual(0, firstResult.GroupsCount);
            };

            var serviceResults = service.GetItinerariesByProjectId(project.ProjectId);
            var serviceResultsAsync = await service.GetItinerariesByProjectIdAsync(project.ProjectId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreate_CheckProperties()
        {
            var creatorId = 1;
            Location arrivalLocation = new Location
            {
                LocationId = 1
            };
            Location departureLocation = new Location
            {
                LocationId = 2
            };
            Project project = new Project
            {
                ProjectId = 10
            };
            AddedEcaItinerary model = new AddedEcaItinerary(
                creator: new User(creatorId),
                projectId: project.ProjectId,
                arrivalLocationId: arrivalLocation.LocationId,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: departureLocation.LocationId,
                name: "Name",
                startDate: DateTimeOffset.Now.AddDays(-1.0)
                );

            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Locations.Add(arrivalLocation);
                context.Locations.Add(departureLocation);
            });
            Action tester = () =>
            {
                Assert.AreEqual(1, context.Itineraries.Count());
                var addedItinerary = context.Itineraries.First();
                
                Assert.AreEqual(creatorId, addedItinerary.History.CreatedBy);
                Assert.AreEqual(creatorId, addedItinerary.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedItinerary.History.RevisedOn, 20000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedItinerary.History.CreatedOn, 20000);

                Assert.AreEqual(project.ProjectId, addedItinerary.ProjectId);
                Assert.AreEqual(model.ArrivalLocationId, addedItinerary.ArrivalLocationId);
                Assert.AreEqual(model.DepartureLocationId, addedItinerary.DepartureLocationId);
                Assert.AreEqual(model.StartDate, addedItinerary.StartDate);
                Assert.AreEqual(model.EndDate, addedItinerary.EndDate);
                Assert.AreEqual(model.Name, addedItinerary.Name);
                Assert.AreEqual(ItineraryStatus.InProgress.Id, addedItinerary.ItineraryStatusId);
            };

            Action<AddedEcaItineraryValidationEntity> validationEntityTester = (validationEntity) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(model, validationEntity.AddedItinerary));
                Assert.IsTrue(Object.ReferenceEquals(project, validationEntity.Project));
                Assert.IsTrue(Object.ReferenceEquals(arrivalLocation, validationEntity.ArrivalLocation));
                Assert.IsTrue(Object.ReferenceEquals(departureLocation, validationEntity.DepartureLocation));
            };
            validator.Setup(x => x.ValidateCreate(It.IsAny<AddedEcaItineraryValidationEntity>())).Callback(validationEntityTester);

            context.Revert();
            service.Create(model);
            tester();

            context.Revert();
            await service.CreateAsync(model);
            tester();

            validator.Verify(x => x.ValidateCreate(It.IsAny<AddedEcaItineraryValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestCreate_ProjectDoesNotExist()
        {
            Location arrivalLocation = new Location
            {
                LocationId = 1
            };
            Location departureLocation = new Location
            {
                LocationId = 2
            };
            context.Locations.Add(arrivalLocation);
            context.Locations.Add(departureLocation);
            AddedEcaItinerary model = new AddedEcaItinerary(
               creator: new User(1),
                arrivalLocationId: 1,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: departureLocation.LocationId,
                name: "Name",
                startDate: DateTimeOffset.Now.AddDays(-1.0),
                projectId: -1
                );

            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Project).Name, model.ProjectId);
            Func<Task> f = () =>
            {
                return service.CreateAsync(model);
            };
            Action a = () => service.Create(model);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreate_ArrivalLocationDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            Location departureLocation = new Location
            {
                LocationId = 2
            };
            context.Projects.Add(project);
            context.Locations.Add(departureLocation);
            AddedEcaItinerary model = new AddedEcaItinerary(
                creator: new User(1),
                arrivalLocationId: 1,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: departureLocation.LocationId,
                name: "Name",
                startDate: DateTimeOffset.Now.AddDays(-1.0),
                projectId: project.ProjectId
                );

            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Location).Name, model.ArrivalLocationId);
            Func<Task> f = () =>
            {
                return service.CreateAsync(model);
            };
            Action a = () => service.Create(model);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreate_DepartureLocationDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            Location arrivalLocation = new Location
            {
                LocationId = 1
            };
            context.Projects.Add(project);
            context.Locations.Add(arrivalLocation);
            AddedEcaItinerary model = new AddedEcaItinerary(
                creator: new User(1),
                arrivalLocationId: arrivalLocation.LocationId,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: 2,
                name: "Name",
                startDate: DateTimeOffset.Now.AddDays(-1.0),
                projectId: project.ProjectId
                );

            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Location).Name, model.DepartureLocationId);
            Func<Task> f = () =>
            {
                return service.CreateAsync(model);
            };
            Action a = () => service.Create(model);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_CheckProperties()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var creatorId = 1;
            var updatorId = 2;
            var itineraryId = 1;
            var projectId = 5;
            Itinerary itineraryToUpdate = null;
            Location arrivalLocation = new Location
            {
                LocationId = 1
            };
            Location departureLocation = new Location
            {
                LocationId = 2
            };
            UpdatedEcaItinerary model = new UpdatedEcaItinerary(
                id: itineraryId,
                updator: new User(updatorId),
                arrivalLocationId: arrivalLocation.LocationId,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: departureLocation.LocationId,
                name: "Name",
                projectId: projectId,
                startDate: DateTimeOffset.Now.AddDays(-1.0)

                );

            context.SetupActions.Add(() =>
            {
                itineraryToUpdate = new Itinerary
                {
                    ItineraryId = itineraryId,
                    ProjectId = projectId
                };
                itineraryToUpdate.History.CreatedBy = creatorId;
                itineraryToUpdate.History.CreatedOn = yesterday;
                itineraryToUpdate.History.RevisedBy = creatorId;
                itineraryToUpdate.History.RevisedOn = yesterday;
                context.Itineraries.Add(itineraryToUpdate);
                context.Locations.Add(arrivalLocation);
                context.Locations.Add(departureLocation);
            });
            Action tester = () =>
            {
                Assert.AreEqual(creatorId, itineraryToUpdate.History.CreatedBy);
                Assert.AreEqual(yesterday, itineraryToUpdate.History.CreatedOn);
                Assert.AreEqual(updatorId, itineraryToUpdate.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(itineraryToUpdate.History.RevisedOn, 20000);

                Assert.AreEqual(model.ArrivalLocationId, itineraryToUpdate.ArrivalLocationId);
                Assert.AreEqual(model.DepartureLocationId, itineraryToUpdate.DepartureLocationId);
                Assert.AreEqual(model.StartDate, itineraryToUpdate.StartDate);
                Assert.AreEqual(model.EndDate, itineraryToUpdate.EndDate);
                Assert.AreEqual(model.Name, itineraryToUpdate.Name);
            };

            Action<UpdatedEcaItineraryValidationEntity> validationEntityTester = (validationEntity) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(itineraryToUpdate, validationEntity.ItineraryToUpdate));
                Assert.IsTrue(Object.ReferenceEquals(model, validationEntity.UpdatedItinerary));
                Assert.IsTrue(Object.ReferenceEquals(arrivalLocation, validationEntity.ArrivalLocation));
                Assert.IsTrue(Object.ReferenceEquals(departureLocation, validationEntity.DepartureLocation));
            };
            validator.Setup(x => x.ValidateUpdate(It.IsAny<UpdatedEcaItineraryValidationEntity>())).Callback(validationEntityTester);


            context.Revert();
            service.Update(model);
            tester();

            context.Revert();
            await service.UpdateAsync(model);
            tester();

            validator.Verify(x => x.ValidateUpdate(It.IsAny<UpdatedEcaItineraryValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestUpdate_DoesNotBelongToGivenProject()
        {
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var creatorId = 1;
            var updatorId = 2;
            var itineraryId = 1;
            var projectId = 5;
            Itinerary itineraryToUpdate = null;
            Location arrivalLocation = new Location
            {
                LocationId = 1
            };
            Location departureLocation = new Location
            {
                LocationId = 2
            };
            UpdatedEcaItinerary model = new UpdatedEcaItinerary(
                id: itineraryId,
                updator: new User(updatorId),
                arrivalLocationId: arrivalLocation.LocationId,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: departureLocation.LocationId,
                name: "Name",
                projectId: projectId + 1,
                startDate: DateTimeOffset.Now.AddDays(-1.0)

                );

            context.SetupActions.Add(() =>
            {
                itineraryToUpdate = new Itinerary
                {
                    ItineraryId = itineraryId,
                    ProjectId = projectId
                };
                itineraryToUpdate.History.CreatedBy = creatorId;
                itineraryToUpdate.History.CreatedOn = yesterday;
                itineraryToUpdate.History.RevisedBy = creatorId;
                itineraryToUpdate.History.RevisedOn = yesterday;
                context.Itineraries.Add(itineraryToUpdate);
                context.Locations.Add(arrivalLocation);
                context.Locations.Add(departureLocation);
            });
            context.Revert();
            var message = String.Format("The user with id [{0}] attempted to edit an itinerary on a project with id [{1}] but should have been denied access.",
                        updatorId,
                        model.ProjectId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(model);
            };
            Action a = () => service.Update(model);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_ItineraryDoesNotExist()
        {
            var itineraryId = 1;
            var projectId = 5;        
            Location arrivalLocation = new Location
            {
                LocationId = 1
            };
            Location departureLocation = new Location
            {
                LocationId = 2
            };
            context.Locations.Add(arrivalLocation);
            context.Locations.Add(departureLocation);
            UpdatedEcaItinerary model = new UpdatedEcaItinerary(
                id: itineraryId,
                updator: new User(1),
                arrivalLocationId: arrivalLocation.LocationId,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: departureLocation.LocationId,
                name: "Name",
                projectId: projectId,
                startDate: DateTimeOffset.Now.AddDays(-1.0)
                );
            
            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Itinerary).Name, itineraryId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(model);
            };
            Action a = () => service.Update(model);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_ArrivalLocationDoesNotExist()
        {
            var itineraryId = 1;
            var projectId = 5;
            var itinerary = new Itinerary
            {
                ItineraryId = itineraryId
            };
            Location departureLocation = new Location
            {
                LocationId = 2
            };
            context.Itineraries.Add(itinerary);
            context.Locations.Add(departureLocation);
            UpdatedEcaItinerary model = new UpdatedEcaItinerary(
                id: itineraryId,
                updator: new User(1),
                arrivalLocationId: 1,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: departureLocation.LocationId,
                name: "Name",
                projectId: projectId,
                startDate: DateTimeOffset.Now.AddDays(-1.0)

                );

            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Location).Name, model.ArrivalLocationId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(model);
            };
            Action a = () => service.Update(model);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_DepartureLocationDoesNotExist()
        {
            var itineraryId = 1;
            var projectId = 5;
            var itinerary = new Itinerary
            {
                ItineraryId = itineraryId
            };
            Location arrivalLocation = new Location
            {
                LocationId = 1
            };
            context.Itineraries.Add(itinerary);
            context.Locations.Add(arrivalLocation);
            UpdatedEcaItinerary model = new UpdatedEcaItinerary(
                id: itineraryId,
                updator: new User(1),
                arrivalLocationId: arrivalLocation.LocationId,
                endDate: DateTimeOffset.Now.AddDays(1.0),
                departureLocationId: 2,
                name: "Name",
                projectId: projectId,
                startDate: DateTimeOffset.Now.AddDays(-1.0)

                );

            var message = String.Format("The [{0}] with id [{1}] does not exist.", typeof(Location).Name, model.DepartureLocationId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(model);
            };
            Action a = () => service.Update(model);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion
    }
}
