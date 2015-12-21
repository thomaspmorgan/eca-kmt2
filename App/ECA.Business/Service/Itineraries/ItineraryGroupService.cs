using ECA.Business.Queries.Itineraries;
using System.Data.Entity;
using System.Linq;
using ECA.Business.Queries.Models.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System;
using ECA.Core.Exceptions;
using ECA.Business.Validation;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An ItineraryGroupService is a service capable of performing crud operations on itinerary groups with an EcaContext.
    /// </summary>
    public class ItineraryGroupService : EcaService, IItineraryGroupService
    {
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private readonly Action<int, Itinerary, int> throwSecurityViolationIfDifferentProject;
        private readonly IBusinessValidator<AddedEcaItineraryGroupValidationEntity, object> validator;

        /// <summary>
        /// Creates a new ItineraryGroupService with the given context and save actions.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public ItineraryGroupService(EcaContext context, IBusinessValidator<AddedEcaItineraryGroupValidationEntity, object> validator, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
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
        /// Returns the itinerary groups for the given itinerary by id.
        /// </summary>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="projectId">The project id.  Used for security purposes.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, sorted itinerary groups.</returns>
        public async Task<PagedQueryResults<ItineraryGroupDTO>> GetItineraryGroupsByItineraryIdAsync(int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator)
        {
            var results = await ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(this.Context, projectId, itineraryId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            return results;
        }

        /// <summary>
        /// Returns the itinerary groups for the given itinerary by id.
        /// </summary>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <param name="projectId">The project id.  Used for security purposes.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, sorted itinerary groups.</returns>
        public PagedQueryResults<ItineraryGroupDTO> GetItineraryGroupsByItineraryId(int projectId, int itineraryId, QueryableOperator<ItineraryGroupDTO> queryOperator)
        {
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(this.Context, projectId, itineraryId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            return results;
        }

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The list of itinerary groups and participant persons per group.</returns>
        public async Task<List<ItineraryGroupParticipantsDTO>> GetItineraryGroupPersonsByItineraryIdAsync(int projectId, int itineraryId)
        {
            var results = await ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(this.Context, projectId, itineraryId).ToListAsync();
            return results;
        }

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryId">The itinerary id.</param>
        /// <returns>The list of itinerary groups and participant persons per group.</returns>
        public List<ItineraryGroupParticipantsDTO> GetItineraryGroupPersonsByItineraryId(int projectId, int itineraryId)
        {
            var results = ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(this.Context, projectId, itineraryId).ToList();
            return results;
        }

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryGroupId">The itinerary group id.</param>
        /// <returns>The itinerary group and its participant persons.</returns>
        public async Task<ItineraryGroupParticipantsDTO> GetItineraryGroupByIdAsync(int projectId, int itineraryGroupId)
        {
            var results = await CreateGetItineraryGroupByIdQuery(projectId, itineraryGroupId).FirstOrDefaultAsync();
            return results;
        }

        /// <summary>
        /// Returns the itinerary groups and participant persons given the project id and itinerary id.
        /// </summary>
        /// <param name="projectId">The id of the project.  Used for security purposes.</param>
        /// <param name="itineraryGroupId">The itinerary group id.</param>
        /// <returns>The itinerary group and its participant persons.</returns>
        public ItineraryGroupParticipantsDTO GetItineraryGroupById(int projectId, int itineraryGroupId)
        {
            var results = CreateGetItineraryGroupByIdQuery(projectId, itineraryGroupId).FirstOrDefault();
            return results;
        }

        private IQueryable<ItineraryGroupParticipantsDTO> CreateGetItineraryGroupByIdQuery(int projectId, int itineraryGroupId)
        {
            return ItineraryGroupQueries.CreateGetItineraryGroupParticipantsQuery(this.Context).Where(x => x.ProjectId == projectId && x.ItineraryGroupId == itineraryGroupId);
        }

        #endregion

        #region Create
        /// <summary>
        /// Creates a new itinerary group.
        /// </summary>
        /// <param name="addedGroup">The itinerary group.</param>
        /// <returns>The added eca.data itinerary group.</returns>
        public ItineraryGroup Create(AddedEcaItineraryGroup addedGroup)
        {
            var itinerary = Context.Itineraries.Find(addedGroup.ItineraryId);
            throwIfModelDoesNotExist(addedGroup.ItineraryId, itinerary, typeof(Itinerary));
            throwSecurityViolationIfDifferentProject(addedGroup.Audit.User.Id, itinerary, addedGroup.ProjectId);

            var participants = CreateGetParticipantsQuery(addedGroup.ParticipantIds).ToList();
            var existingItineraryGroups = ItineraryGroupQueries.CreateGetEqualItineraryGroupsQuery(this.Context, addedGroup.ItineraryId, addedGroup.ProjectId, addedGroup.ParticipantIds).ToList();
            return DoCreate(itinerary, addedGroup, participants, existingItineraryGroups);
        }

        /// <summary>
        /// Creates a new itinerary group.
        /// </summary>
        /// <param name="addedGroup">The itinerary group.</param>
        /// <returns>The added eca.data itinerary group.</returns>
        public async Task<ItineraryGroup> CreateAsync(AddedEcaItineraryGroup addedGroup)
        {
            var itinerary = await Context.Itineraries.FindAsync(addedGroup.ItineraryId);
            throwIfModelDoesNotExist(addedGroup.ItineraryId, itinerary, typeof(Itinerary));
            throwSecurityViolationIfDifferentProject(addedGroup.Audit.User.Id, itinerary, addedGroup.ProjectId);

            var participants = await CreateGetParticipantsQuery(addedGroup.ParticipantIds).ToListAsync();
            var existingItineraryGroups = await ItineraryGroupQueries.CreateGetEqualItineraryGroupsQuery(this.Context, addedGroup.ItineraryId, addedGroup.ProjectId, addedGroup.ParticipantIds).ToListAsync();
            return DoCreate(itinerary, addedGroup, participants, existingItineraryGroups);
        }

        private IQueryable<Participant> CreateGetParticipantsQuery(IEnumerable<int> participantIds)
        {
            return Context.Participants.Include(x => x.ParticipantType).Where(x => participantIds.Contains(x.ParticipantId));
        }

        private ItineraryGroup DoCreate(Itinerary itinerary, AddedEcaItineraryGroup addedGroup, IEnumerable<Participant> participants, List<ItineraryGroupDTO> existingItineraryGroups)
        {
            Contract.Requires(itinerary != null, "The itinerary must not be null.");
            Contract.Requires(addedGroup != null, "The added group must not be null.");
            var validationEntity = GetAddedEcaItineraryGroupValidationEntity(itinerary: itinerary, addedGroup: addedGroup, participants: participants, existingItineraryGroups: existingItineraryGroups);
            validator.ValidateCreate(validationEntity);
            var itineraryGroup = new ItineraryGroup();
            itineraryGroup.Name = addedGroup.Name;
            itineraryGroup.ItineraryId = addedGroup.ItineraryId;
            SetParticipants(addedGroup.ParticipantIds.ToList(), itineraryGroup);
            addedGroup.Audit.SetHistory(itineraryGroup);
            Context.ItineraryGroups.Add(itineraryGroup);
            return itineraryGroup;
        }

        private AddedEcaItineraryGroupValidationEntity GetAddedEcaItineraryGroupValidationEntity(
            Itinerary itinerary, 
            AddedEcaItineraryGroup addedGroup, 
            IEnumerable<Participant> participants,
            IEnumerable<ItineraryGroupDTO> existingItineraryGroups)
        {
            Contract.Requires(itinerary != null, "The itinerary must not be null.");
            Contract.Requires(addedGroup != null, "The added group must not be null.");
            Contract.Requires(participants != null, "The participants must not be null.");
            return new AddedEcaItineraryGroupValidationEntity(participants: participants, existingItineraryGroups: existingItineraryGroups);
        }
        #endregion

        /// <summary>
        /// Updates the participants on the given Itinerary Group to the participants with the given ids.  Ensure the participants
        /// are already loaded via the context before calling this method.
        /// </summary>
        /// <param name="objectiveIds">The participants by id.</param>
        /// <param name="objectivable">The itinerary group to update.</param>
        public void SetParticipants(List<int> participantIds, ItineraryGroup itineraryGroup)
        {
            Contract.Requires(participantIds != null, "The participant ids must not be null.");
            Contract.Requires(itineraryGroup != null, "The itinerary group must not be null.");
            //this will be empty on create, update will have to load participants
            var participantsToRemove = itineraryGroup.Participants.Where(x => !participantIds.Contains(x.ParticipantId)).ToList();
            var participantsToAdd = new List<Participant>();
            participantIds.Where(x => !itineraryGroup.Participants.Select(o => o.ParticipantId).ToList().Contains(x)).ToList()
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
                    itineraryGroup.Participants.Add(x);
                }
                else
                {
                    itineraryGroup.Participants.Add(localEntity);
                }
            });
            participantsToRemove.ForEach(x =>
            {
                itineraryGroup.Participants.Remove(x);
            });
        }
    }
}
