using ECA.Business.Validation;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Queries.Itineraries;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryStopService is used to perform crud operations on itinerary stops using an EcaContext instance.
    /// </summary>
    public class ItineraryStopService : EcaService, IItineraryStopService
    {
        private readonly IBusinessValidator<EcaItineraryStopValidationEntity, EcaItineraryStopValidationEntity> itineraryStopValidator;
        private readonly IBusinessValidator<ItineraryStopParticipantsValidationEntity, ItineraryStopParticipantsValidationEntity> itineraryStopParticipantsValidator;
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private readonly Action<int, Itinerary, int> throwSecurityViolationIfDifferentProject;
        private readonly Action<int, ItineraryStopParticipants, Itinerary> throwSecurityViolationIfDifferentItinerary;

        /// <summary>
        /// Creates a new service instance with the given context and save actions.
        /// </summary>
        /// <param name="context">The context to perform crud operations against.</param>
        /// <param name="saveActions">The context save actions.</param>
        /// <param name="itineraryStopValidator">The business validator.</param>
        public ItineraryStopService(
            EcaContext context, 
            IBusinessValidator<EcaItineraryStopValidationEntity, EcaItineraryStopValidationEntity> itineraryStopValidator,
            IBusinessValidator<ItineraryStopParticipantsValidationEntity, ItineraryStopParticipantsValidationEntity> itineraryStopParticipantsValidator,
            List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(itineraryStopValidator != null, "The itinerary stop validator must not be null.");
            Contract.Requires(itineraryStopParticipantsValidator != null, "The itinerary stop participant validator must not be null.");
            this.itineraryStopValidator = itineraryStopValidator;
            this.itineraryStopParticipantsValidator = itineraryStopParticipantsValidator;
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
            throwSecurityViolationIfDifferentItinerary = (userId, itineraryStopParticipants, itinerary) =>
            {
                if (itinerary != null && itinerary.ItineraryId != itineraryStopParticipants.ItineraryId)
                {
                    throw new BusinessSecurityException(
                        String.Format("The user with id [{0}] attempted to edit an itinerary stop on a project with id [{1}] and itinerary with id [{2}] but should have been denied access.",
                        userId,
                        itineraryStopParticipants.ProjectId,
                        itineraryStopParticipants.ItineraryId
                        ));
                }
            };
        }

        
        #region Get
        /// <summary>
        /// Returns a list of itinerary stops for the itinerary with the given id and project by id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>Returns the itinerary stops for the itinerary and project by ids.</returns>
        public List<ItineraryStopDTO> GetItineraryStopsByItineraryId(int projectId, int itineraryId)
        {
            return CreateGetItineraryStopsByItineraryIdAndProjectIdQuery(projectId, itineraryId).ToList();
        }

        /// <summary>
        /// Returns a list of itinerary stops for the itinerary with the given id and project by id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>Returns the itinerary stops for the itinerary and project by ids.</returns>
        public Task<List<ItineraryStopDTO>> GetItineraryStopsByItineraryIdAsync(int projectId, int itineraryId)
        {
            return CreateGetItineraryStopsByItineraryIdAndProjectIdQuery(projectId, itineraryId).ToListAsync();
        }

        private IQueryable<ItineraryStopDTO> CreateGetItineraryStopsByItineraryIdAndProjectIdQuery(int projectId, int itineraryId)
        {
            return ItineraryStopQueries.CreateGetItineraryStopsByItineraryIdQuery(this.Context, itineraryId).Where(x => x.ProjectId == projectId).OrderBy(x => x.ArrivalDate);
        }

        /// <summary>
        /// Returns the itinerary stopo with the given id.
        /// </summary>
        /// <param name="itineraryStopId">The itinerary stop id.</param>
        /// <returns>The itinerary stop dto.</returns>
        public ItineraryStopDTO GetItineraryStopById(int itineraryStopId)
        {
            return CreateGetItineraryStopDTOByIdQuery(itineraryStopId).FirstOrDefault();
        }

        /// <summary>
        /// Returns the itinerary stopo with the given id.
        /// </summary>
        /// <param name="itineraryStopId">The itinerary stop id.</param>
        /// <returns>The itinerary stop dto.</returns>
        public Task<ItineraryStopDTO> GetItineraryStopByIdAsync(int itineraryStopId)
        {
            return CreateGetItineraryStopDTOByIdQuery(itineraryStopId).FirstOrDefaultAsync();
        }

        private IQueryable<ItineraryStopDTO> CreateGetItineraryStopDTOByIdQuery(int itineraryStopId)
        {
            return ItineraryStopQueries.CreateGetItineraryStopsQuery(this.Context).Where(x => x.ItineraryStopId == itineraryStopId);
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a new itinerary stop.
        /// </summary>
        /// <param name="addedStop">The new itinerary stop.</param>
        /// <returns>The Eca.Data ItineraryStop instance.</returns>
        public ItineraryStop Create(AddedEcaItineraryStop addedStop)
        {
            var itinerary = Context.Itineraries.Find(addedStop.ItineraryId);
            throwIfModelDoesNotExist(addedStop.ItineraryId, itinerary, typeof(Itinerary));
            throwSecurityViolationIfDifferentProject(addedStop.Audit.User.Id, itinerary, addedStop.ProjectId);

            var destination = Context.Locations.Find(addedStop.DestinationLocationId);
            throwIfModelDoesNotExist(addedStop.DestinationLocationId, destination, typeof(Location));

            return DoCreate(itinerary, addedStop);
        }

        /// <summary>
        /// Creates a new itinerary stop.
        /// </summary>
        /// <param name="addedStop">The new itinerary stop.</param>
        /// <returns>The Eca.Data ItineraryStop instance.</returns>
        public async Task<ItineraryStop> CreateAsync(AddedEcaItineraryStop addedStop)
        {
            var itinerary = await Context.Itineraries.FindAsync(addedStop.ItineraryId);
            throwIfModelDoesNotExist(addedStop.ItineraryId, itinerary, typeof(Itinerary));
            throwSecurityViolationIfDifferentProject(addedStop.Audit.User.Id, itinerary, addedStop.ProjectId);

            var destination = await Context.Locations.FindAsync(addedStop.DestinationLocationId);
            throwIfModelDoesNotExist(addedStop.DestinationLocationId, destination, typeof(Location));

            return DoCreate(itinerary, addedStop);
        }

        private ItineraryStop DoCreate(Itinerary itinerary, AddedEcaItineraryStop addedStop)
        {
            var validationEntity = GetEcaItineraryStopValidationEntity(itinerary: itinerary, ecaitineraryStop: addedStop);
            itineraryStopValidator.ValidateCreate(validationEntity);

            var update = new Update(addedStop.Audit.User);
            update.SetHistory(itinerary);

            var itineraryStop = new ItineraryStop();
            itineraryStop.DateArrive = addedStop.ArrivalDate;
            itineraryStop.DateLeave = addedStop.DepartureDate;
            itineraryStop.DestinationId = addedStop.DestinationLocationId;
            itineraryStop.ItineraryId = addedStop.ItineraryId;
            itineraryStop.ItineraryStatusId = ItineraryStatus.InProgress.Id;
            itineraryStop.Name = addedStop.Name;
            itineraryStop.TimezoneId = addedStop.TimezoneId;
            addedStop.Audit.SetHistory(itineraryStop);

            Context.ItineraryStops.Add(itineraryStop);
            return itineraryStop;
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the system's itinerary stop with the given updated stop.
        /// </summary>
        /// <param name="updatedStop">The updated itinerary stop.</param>
        public void Update(UpdatedEcaItineraryStop updatedStop)
        {
            var itineraryStopToUpdate = CreateGetItineraryStopByIdQuery(updatedStop.ItineraryStopId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedStop.ItineraryStopId, itineraryStopToUpdate, typeof(ItineraryStop));

            var itinerary = itineraryStopToUpdate.Itinerary;
            throwSecurityViolationIfDifferentProject(updatedStop.Audit.User.Id, itinerary, updatedStop.ProjectId);

            var destination = Context.Locations.Find(updatedStop.DestinationLocationId);
            throwIfModelDoesNotExist(updatedStop.DestinationLocationId, destination, typeof(Location));

            DoUpdate(itinerary, updatedStop, itineraryStopToUpdate);
        }

        /// <summary>
        /// Updates the system's itinerary stop with the given updated stop.
        /// </summary>
        /// <param name="updatedStop">The updated itinerary stop.</param>
        public async Task UpdateAsync(UpdatedEcaItineraryStop updatedStop)
        {
            var itineraryStopToUpdate = await CreateGetItineraryStopByIdQuery(updatedStop.ItineraryStopId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedStop.ItineraryStopId, itineraryStopToUpdate, typeof(ItineraryStop));

            var itinerary = itineraryStopToUpdate.Itinerary;
            throwSecurityViolationIfDifferentProject(updatedStop.Audit.User.Id, itinerary, updatedStop.ProjectId);

            var destination = await Context.Locations.FindAsync(updatedStop.DestinationLocationId);
            throwIfModelDoesNotExist(updatedStop.DestinationLocationId, destination, typeof(Location));

            DoUpdate(itinerary, updatedStop, itineraryStopToUpdate);
        }

        private void DoUpdate(Itinerary itinerary, UpdatedEcaItineraryStop updatedStop, ItineraryStop itineraryStop)
        {
            var validationEntity = GetEcaItineraryStopValidationEntity(itinerary: itinerary, ecaitineraryStop: updatedStop);
            itineraryStopValidator.ValidateUpdate(validationEntity);

            itineraryStop.DateArrive = updatedStop.ArrivalDate;
            itineraryStop.DateLeave = updatedStop.DepartureDate;
            itineraryStop.DestinationId = updatedStop.DestinationLocationId;
            itineraryStop.Name = updatedStop.Name;
            itineraryStop.TimezoneId = updatedStop.TimezoneId;

            Contract.Assert(updatedStop.Audit.GetType() == typeof(Update), "The audit type must be an update.  The itinerary create date should not change.");
            updatedStop.Audit.SetHistory(itineraryStop);
            updatedStop.Audit.SetHistory(itinerary);
        }

        private EcaItineraryStopValidationEntity GetEcaItineraryStopValidationEntity(Itinerary itinerary, EcaItineraryStop ecaitineraryStop)
        {
            return new EcaItineraryStopValidationEntity(
                itineraryEndDate: itinerary.EndDate,
                itineraryStartDate: itinerary.StartDate,
                itineraryStopArrivalDate: ecaitineraryStop.ArrivalDate,
                itineraryStopDepartureDate: ecaitineraryStop.DepartureDate,
                timezoneId: ecaitineraryStop.TimezoneId
                );
        }
        #endregion

        private IQueryable<ItineraryStop> CreateGetItineraryStopByIdQuery(int itineraryStopId)
        {
            //need to include itinerary for update and itinerary.participants for setparticipants methods
            return Context.ItineraryStops
                .Include(x => x.Itinerary)
                .Include(x => x.Itinerary.Participants)
                .Where(x => x.ItineraryStopId == itineraryStopId);
        }

        #region Set Participants

        /// <summary>
        /// Sets the participants on the itinerary stop.
        /// </summary>
        /// <param name="itineraryStopParticipants">The business entity containing the participants by id that should be set on the itinerary.</param>
        public void SetParticipants(ItineraryStopParticipants itineraryStopParticipants)
        {
            var itineraryStop = CreateGetItineraryStopByIdQuery(itineraryStopParticipants.ItineraryStopId).FirstOrDefault();
            throwIfModelDoesNotExist(itineraryStopParticipants.ItineraryStopId, itineraryStop, typeof(ItineraryStop));

            var itinerary = itineraryStop.Itinerary;
            throwSecurityViolationIfDifferentItinerary(itineraryStopParticipants.Audit.User.Id, itineraryStopParticipants, itinerary);
            throwSecurityViolationIfDifferentProject(itineraryStopParticipants.Audit.User.Id, itinerary, itineraryStopParticipants.ProjectId);
            var itineraryParticipants = itinerary.Participants;
            DoSetParticipants(
                itinerary: itinerary, 
                itineraryStop: itineraryStop, 
                itineraryParticipants: itineraryParticipants, 
                itineraryStopParticipants: itineraryStopParticipants);
        }

        /// <summary>
        /// Sets the participants on the itinerary top.
        /// </summary>
        /// <param name="itineraryStopParticipants">The business entity containing the participants by id that should be set on the itinerary.</param>
        /// <returns>The task.</returns>
        public async Task SetParticipantsAsync(ItineraryStopParticipants itineraryStopParticipants)
        {
            var itineraryStop = await CreateGetItineraryStopByIdQuery(itineraryStopParticipants.ItineraryStopId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(itineraryStopParticipants.ItineraryStopId, itineraryStop, typeof(ItineraryStop));

            var itinerary = itineraryStop.Itinerary;
            throwSecurityViolationIfDifferentItinerary(itineraryStopParticipants.Audit.User.Id, itineraryStopParticipants, itinerary);
            throwSecurityViolationIfDifferentProject(itineraryStopParticipants.Audit.User.Id, itinerary, itineraryStopParticipants.ProjectId);
            var itineraryParticipants = itinerary.Participants;
            DoSetParticipants(
                itinerary: itinerary,
                itineraryStop: itineraryStop,
                itineraryParticipants: itineraryParticipants,
                itineraryStopParticipants: itineraryStopParticipants);
        }
        

        private void DoSetParticipants(
            Itinerary itinerary, 
            ItineraryStop itineraryStop,
            IEnumerable<Participant> itineraryParticipants, 
            ItineraryStopParticipants itineraryStopParticipants)
        {

            var notAllowedParticipantsById = itineraryStopParticipants.ParticipantIds.Except(itineraryParticipants.Select(x => x.ParticipantId).Distinct());

            var validationEntity = new ItineraryStopParticipantsValidationEntity(notAllowedParticipantsById);
            itineraryStopParticipantsValidator.ValidateUpdate(validationEntity);

            Contract.Assert(itineraryStopParticipants.Audit.GetType() == typeof(Update), "The audit type must be an update.  The itinerary create date should not change.");
            itineraryStopParticipants.Audit.SetHistory(itineraryStop);
            itineraryStopParticipants.Audit.SetHistory(itinerary);
            SetParticipants(itineraryStopParticipants.ParticipantIds.ToList(), itineraryStop, x => x.Participants);
        }

       
        #endregion

    }
}
