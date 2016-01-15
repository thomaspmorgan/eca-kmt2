using ECA.Business.Queries.Itineraries;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Validation;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryService is used to perform crud operations for project itineraries.
    /// </summary>
    public class ItineraryService : EcaService, IItineraryService
    {
        private readonly IBusinessValidator<AddedEcaItineraryValidationEntity, UpdatedEcaItineraryValidationEntity> validator;
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private readonly Action<int, Itinerary, int> throwSecurityViolationIfDifferentProject;
        /// <summary>
        /// Creates a new service instance with the given context and save actions.
        /// </summary>
        /// <param name="context">The context to perform crud operations against.</param>
        /// <param name="saveActions">The context save actions.</param>
        public ItineraryService(EcaContext context, IBusinessValidator<AddedEcaItineraryValidationEntity, UpdatedEcaItineraryValidationEntity> validator, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(validator != null, "The validator must not be null.");
            this.validator = validator;
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The [{0}] with id [{1}] does not exist.", type.Name, id));
                }
            };
            throwSecurityViolationIfDifferentProject = (userId, instance, projectId) =>
            {
                if (instance != null && instance.ProjectId != projectId)
                {
                    throw new BusinessSecurityException(
                        String.Format("The user with id [{0}] attempted to edit an itinerary on a project with id [{1}] but should have been denied access.",
                        userId,
                        projectId));
                }
            };
        }

        #region Get

        /// <summary>
        /// Returns the itineraries for the given project by project id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The itineraries of the project.</returns>
        public Task<List<ItineraryDTO>> GetItinerariesByProjectIdAsync(int projectId)
        {
            var itineraries = ItineraryQueries.CreateGetItinerariesByProjectIdQuery(this.Context, projectId).ToListAsync();
            return itineraries;
        }

        /// <summary>
        /// Returns the itineraries for the given project by project id.
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The itineraries of the project.</returns>
        public List<ItineraryDTO> GetItinerariesByProjectId(int projectId)
        {
            var itineraries = ItineraryQueries.CreateGetItinerariesByProjectIdQuery(this.Context, projectId).ToList();
            return itineraries;
        }

        /// <summary>
        /// Returns the itinerary with the given id and project id.
        /// </summary>
        /// <param name="id">The id of the itinerary.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The itinerary.</returns>
        public ItineraryDTO GetItineraryById(int projectId, int id)
        {
            var itinerary = CreateGetItineraryByIdAndProjectId(projectId, id).FirstOrDefault();
            return itinerary;
        }

        /// <summary>
        /// Returns the itinerary with the given id and project id.
        /// </summary>
        /// <param name="id">The id of the itinerary.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The itinerary.</returns>
        public async Task<ItineraryDTO> GetItineraryByIdAsync(int projectId, int id)
        {
            var itinerary = await CreateGetItineraryByIdAndProjectId(projectId, id).FirstOrDefaultAsync();
            return itinerary;
        }

        private IQueryable<ItineraryDTO> CreateGetItineraryByIdAndProjectId(int projectId, int id)
        {
            var query = ItineraryQueries.CreateGetItinerariesQuery(this.Context).Where(x => x.ProjectId == projectId && x.Id == id);
            return query;
        }
        #endregion

        #region Create

        /// <summary>
        /// Creates a new itinerary in the eca datastore.
        /// </summary>
        /// <param name="itinerary">The new itinerary.</param>
        /// <returns>The itinerary that was added to the eca context.</returns>
        public Itinerary Create(AddedEcaItinerary itinerary)
        {
            var project = Context.Projects.Find(itinerary.ProjectId);
            throwIfModelDoesNotExist(itinerary.ProjectId, project, typeof(Project));

            var arrivalLocation = Context.Locations.Find(itinerary.ArrivalLocationId);
            throwIfModelDoesNotExist(itinerary.ArrivalLocationId, arrivalLocation, typeof(Location));

            var departureLocation = Context.Locations.Find(itinerary.DepartureLocationId);
            throwIfModelDoesNotExist(itinerary.DepartureLocationId, departureLocation, typeof(Location));

            var participantIds = CreateGetParticipantsByIdsQuery(itinerary.ParticipantIds, itinerary.ProjectId).Select(x => x.ParticipantId).ToList();
            var participantsByIdThatDoNotExist = GetIdsOfParticipantsThatDoNotExist(itinerary.ParticipantIds, participantIds);

            return DoCreate(
                addedItinerary: itinerary, 
                project: project, 
                arrivalLocation: arrivalLocation, 
                departureLocation: departureLocation);
        }

        /// <summary>
        /// Creates a new itinerary in the eca datastore.
        /// </summary>
        /// <param name="itinerary">The new itinerary.</param>
        /// <returns>The itinerary that was added to the eca context.</returns>
        public async Task<Itinerary> CreateAsync(AddedEcaItinerary itinerary)
        {
            var project = await Context.Projects.FindAsync(itinerary.ProjectId);
            throwIfModelDoesNotExist(itinerary.ProjectId, project, typeof(Project));

            var arrivalLocation = await Context.Locations.FindAsync(itinerary.ArrivalLocationId);
            throwIfModelDoesNotExist(itinerary.ArrivalLocationId, arrivalLocation, typeof(Location));

            var departureLocation = await Context.Locations.FindAsync(itinerary.DepartureLocationId);
            throwIfModelDoesNotExist(itinerary.DepartureLocationId, departureLocation, typeof(Location));

            var participantIds = await CreateGetParticipantsByIdsQuery(itinerary.ParticipantIds, itinerary.ProjectId).Select(x => x.ParticipantId).ToListAsync();
            var participantsByIdThatDoNotExist = GetIdsOfParticipantsThatDoNotExist(itinerary.ParticipantIds, participantIds);

            return DoCreate(
                addedItinerary: itinerary,
                project: project,
                arrivalLocation: arrivalLocation,
                departureLocation: departureLocation);
        }

        private AddedEcaItineraryValidationEntity GetAddedEcaItineraryValidationEntity(AddedEcaItinerary addedItinerary, Project project, Location arrivalLocation, Location departureLocation)
        {
            Contract.Requires(addedItinerary != null, "The added itinerary must not be null.");
            Contract.Requires(project != null, "The project must not be null.");
            Contract.Requires(arrivalLocation != null, "The arrival location must not be null.");
            Contract.Requires(departureLocation != null, "The departure destination location must not be null.");
            return new AddedEcaItineraryValidationEntity(
                addedEcaItinerary: addedItinerary,
                project: project,
                arrivalLocation: arrivalLocation,
                departureLocation: departureLocation
                );
        }

        private Itinerary DoCreate(AddedEcaItinerary addedItinerary, Project project, Location arrivalLocation, Location departureLocation, IEnumerable<int> participantsByIdThatDoNotExist)
        {
            validator.ValidateCreate(GetAddedEcaItineraryValidationEntity(
                addedItinerary: addedItinerary,
                project: project,
                arrivalLocation: arrivalLocation,
                departureLocation: departureLocation));

            var itinerary = new Itinerary();
            addedItinerary.Audit.SetHistory(itinerary);
            itinerary.ArrivalLocationId = arrivalLocation.LocationId;
            itinerary.DepartureLocationId = departureLocation.LocationId;
            itinerary.EndDate = addedItinerary.EndDate;
            itinerary.Name = addedItinerary.Name;
            itinerary.ProjectId = project.ProjectId;
            itinerary.StartDate = addedItinerary.StartDate;
            itinerary.ItineraryStatusId = ItineraryStatus.InProgress.Id;
            this.Context.Itineraries.Add(itinerary);
            return itinerary;

        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the datastore's itinerary with the given itinerary.
        /// </summary>
        /// <param name="itinerary">The updated itinerary.</param>
        public void Update(UpdatedEcaItinerary itinerary)
        {
            var itineraryToUpdate = CreateGetItineraryByIdQuery(itinerary.Id).FirstOrDefault();
            throwIfModelDoesNotExist(itinerary.Id, itineraryToUpdate, typeof(Itinerary));

            var arrivalLocation = Context.Locations.Find(itinerary.ArrivalLocationId);
            throwIfModelDoesNotExist(itinerary.ArrivalLocationId, arrivalLocation, typeof(Location));

            var departureLocation = Context.Locations.Find(itinerary.DepartureLocationId);
            throwIfModelDoesNotExist(itinerary.DepartureLocationId, departureLocation, typeof(Location));

            throwSecurityViolationIfDifferentProject(itinerary.Audit.User.Id, itineraryToUpdate, itinerary.ProjectId);

            DoUpdate(
                updatedItinerary: itinerary,
                itineraryToUpdate: itineraryToUpdate,
                arrivalLocation: arrivalLocation,
                departureLocation: departureLocation);
        }

        /// <summary>
        /// Updates the datastore's itinerary with the given itinerary.
        /// </summary>
        /// <param name="itinerary">The updated itinerary.</param>
        public async Task UpdateAsync(UpdatedEcaItinerary itinerary)
        {
            var itineraryToUpdate = await CreateGetItineraryByIdQuery(itinerary.Id).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(itinerary.Id, itineraryToUpdate, typeof(Itinerary));

            var arrivalLocation = await Context.Locations.FindAsync(itinerary.ArrivalLocationId);
            throwIfModelDoesNotExist(itinerary.ArrivalLocationId, arrivalLocation, typeof(Location));

            var departureLocation = await Context.Locations.FindAsync(itinerary.DepartureLocationId);
            throwIfModelDoesNotExist(itinerary.DepartureLocationId, departureLocation, typeof(Location));

            throwSecurityViolationIfDifferentProject(itinerary.Audit.User.Id, itineraryToUpdate, itinerary.ProjectId);

            DoUpdate(
                updatedItinerary: itinerary,
                itineraryToUpdate: itineraryToUpdate,
                arrivalLocation: arrivalLocation,
                departureLocation: departureLocation);
        }

        private IQueryable<Itinerary> CreateGetItineraryByIdQuery(int itineraryId)
        {
            return Context.Itineraries.Include(x => x.Stops).Where(x => x.ItineraryId == itineraryId);
        }

        private void DoUpdate(UpdatedEcaItinerary updatedItinerary, Itinerary itineraryToUpdate, Location arrivalLocation, Location departureLocation)
        {
            validator.ValidateUpdate(GetUpdatedEcaItineraryValidationEntity(
                updatedItinerary: updatedItinerary,
                itineraryToUpdate: itineraryToUpdate,
                arrivalLocation: arrivalLocation,
                departureLocation: departureLocation,
                stops: itineraryToUpdate.Stops));
            updatedItinerary.Audit.SetHistory(itineraryToUpdate);
            itineraryToUpdate.ArrivalLocationId = arrivalLocation.LocationId;
            itineraryToUpdate.DepartureLocationId = departureLocation.LocationId;
            itineraryToUpdate.EndDate = updatedItinerary.EndDate;
            itineraryToUpdate.Name = updatedItinerary.Name;
            itineraryToUpdate.StartDate = updatedItinerary.StartDate;
        }

        private UpdatedEcaItineraryValidationEntity GetUpdatedEcaItineraryValidationEntity(UpdatedEcaItinerary updatedItinerary, Itinerary itineraryToUpdate, Location arrivalLocation, Location departureLocation, IEnumerable<ItineraryStop> stops)
        {
            Contract.Requires(updatedItinerary != null, "The updated itinerary must not be null.");
            Contract.Requires(itineraryToUpdate != null, "The itinerary to update must not be null.");
            Contract.Requires(arrivalLocation != null, "The arrival location must not be null.");
            Contract.Requires(stops != null, "The itinerary stops must not be null.");
            Contract.Requires(departureLocation != null, "The departure destination location must not be null.");
            return new UpdatedEcaItineraryValidationEntity(
                updatedItinerary: updatedItinerary,
                itineraryToUpdate: itineraryToUpdate,
                arrivalLocation: arrivalLocation,
                departureLocation: departureLocation,
                itineraryStop: stops
                );
        }

        #endregion

        private IEnumerable<int> GetIdsOfParticipantsThatDoNotExist(IEnumerable<int> participantIds, IEnumerable<int> contextParticipantIds)
        {
            return contextParticipantIds.Except(participantIds);
        }

        /// <summary>
        /// Creates a query to get participants from a collection of ids and the project.
        /// </summary>
        /// <param name="participantIds">The ids of the participants.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The participants.</returns>
        private IQueryable<Participant> CreateGetParticipantsByIdsQuery(IEnumerable<int> participantIds, int projectId)
        {
            return Context.Participants.Where(x => participantIds.Distinct().Contains(x.ParticipantId) && x.ProjectId == projectId);
        }

        /// <summary>
        /// Updates the participants on the given Itinerary to the participants with the given ids.  Ensure the participants
        /// are already loaded via the context before calling this method.
        /// </summary>
        /// <param name="participantIds">The participants by id.</param>
        /// <param name="itinerary">The itinerary to update.</param>
        public void SetParticipants(List<int> participantIds, Itinerary itinerary)
        {
            Contract.Requires(participantIds != null, "The participant ids must not be null.");
            Contract.Requires(itinerary != null, "The itinerary group must not be null.");
            //this will be empty on create, update will have to load participants
            var participantsToRemove = itinerary.Participants.Where(x => !participantIds.Contains(x.ParticipantId)).ToList();
            var participantsToAdd = new List<Participant>();
            participantIds.Where(x => !itinerary.Participants.Select(o => o.ParticipantId).ToList().Contains(x)).ToList()
                .Select(x => new Participant { ParticipantId = x }).ToList()
                .ForEach(x => participantsToAdd.Add(x));

            participantsToAdd.ForEach(x =>
            {
                var localEntity = Context.GetLocalEntity<Participant>(y => y.ParticipantId == x.ParticipantId);
                if (localEntity == null)
                {
                    if (Context.GetEntityState(x) == EntityState.Detached)
                    {
                        Context.Participants.Attach(x);
                    }
                    itinerary.Participants.Add(x);
                }
                else
                {
                    itinerary.Participants.Add(localEntity);
                }
            });
            participantsToRemove.ForEach(x =>
            {
                itinerary.Participants.Remove(x);
            });
        }
    }
}
