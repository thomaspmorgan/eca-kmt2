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

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryStopService is used to perform crud operations on itinerary stops using an EcaContext instance.
    /// </summary>
    public class ItineraryStopService : EcaService
    {
        private readonly IBusinessValidator<EcaItineraryStopValidationEntity, EcaItineraryStopValidationEntity> validator;
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private readonly Action<int, Itinerary, int> throwSecurityViolationIfDifferentProject;

        /// <summary>
        /// Creates a new service instance with the given context and save actions.
        /// </summary>
        /// <param name="context">The context to perform crud operations against.</param>
        /// <param name="saveActions">The context save actions.</param>
        /// <param name="validator">The business validator.</param>
        public ItineraryStopService(EcaContext context, IBusinessValidator<EcaItineraryStopValidationEntity, EcaItineraryStopValidationEntity> validator, List<ISaveAction> saveActions = null)
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
            validator.ValidateCreate(validationEntity);

            var update = new Update(addedStop.Audit.User);
            update.SetHistory(itinerary);

            var itineraryStop = new ItineraryStop();
            itineraryStop.DateArrive = addedStop.ArrivalDate;
            itineraryStop.DateLeave = addedStop.DepartureDate;
            itineraryStop.DestinationId = addedStop.DestinationLocationId;
            itineraryStop.ItineraryId = addedStop.ItineraryId;
            itineraryStop.ItineraryStatusId = ItineraryStatus.InProgress.Id;
            itineraryStop.Name = addedStop.Name;
            addedStop.Audit.SetHistory(itineraryStop);

            Context.ItineraryStops.Add(itineraryStop);
            return itineraryStop;
        }

        #endregion

        #region Update
        private IQueryable<ItineraryStop> CreateGetItineraryStopByIdQuery(int itineraryStopId)
        {
            return Context.ItineraryStops
                .Include(x => x.Itinerary)
                .Where(x => x.ItineraryStopId == itineraryStopId);
        }

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
            validator.ValidateUpdate(validationEntity);

            itineraryStop.DateArrive = updatedStop.ArrivalDate;
            itineraryStop.DateLeave = updatedStop.DepartureDate;
            itineraryStop.DestinationId = updatedStop.DestinationLocationId;
            itineraryStop.Name = updatedStop.Name;

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
                itineraryStopDepartureDate: ecaitineraryStop.DepartureDate
                );
        }
        #endregion
    }
}
