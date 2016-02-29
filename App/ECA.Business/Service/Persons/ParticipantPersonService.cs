using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using NLog;
using ECA.Core.Exceptions;
using ECA.Business.Validation;

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantPersonService is capable of performing crud operations on participantPersons in the ECA system.
    /// </summary>
    public class ParticipantPersonService : DbContextService<EcaContext>, IParticipantPersonService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private IBusinessValidator<Object, UpdatedParticipantPersonValidationEntity> participantPersonValidator;

        /// <summary>
        /// Creates a new ParticipantPersonService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">The logger.</param>
        public ParticipantPersonService(EcaContext context, IBusinessValidator<Object, UpdatedParticipantPersonValidationEntity> participantPersonValidator, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(participantPersonValidator != null, "The participant person validator must not be null.");
            this.participantPersonValidator = participantPersonValidator;
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
            throwSecurityViolationIfParticipantDoesNotBelongToProject = (userId, projectId, participant) =>
            {
                if (participant != null && participant.ProjectId != projectId)
                {
                    throw new BusinessSecurityException(
                        String.Format("The user with id [{0}] attempted to delete a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        userId,
                        participant.ParticipantId,
                        projectId));
                }
            };
        }

        #region Get
        /// <summary>
        /// Returns the participantPersons for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersons.</returns>
        public PagedQueryResults<SimpleParticipantPersonDTO> GetParticipantPersonsByProjectId(int projectId, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            var participantPersons = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersons;
        }

        /// <summary>
        /// Returns the participantPersons for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersons.</returns>
        public Task<PagedQueryResults<SimpleParticipantPersonDTO>> GetParticipantPersonsByProjectIdAsync(int projectId, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            var participantPersons = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersons;
        }

        /// <summary>
        /// Returns a participant Person
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPerson</returns>
        public SimpleParticipantPersonDTO GetParticipantPersonById(int projectId, int participantId)
        {
            var participantPerson = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(this.Context, projectId, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantPerson by id [{0}].", participantId);
            return participantPerson;
        }

        /// <summary>
        /// Returns a participantPerson asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPerson</returns>
        public Task<SimpleParticipantPersonDTO> GetParticipantPersonByIdAsync(int projectId, int participantId)
        {
            var participant = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(this.Context, projectId, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantPerson by id [{0}].", participantId);
            return participant;
        }
        #endregion

        #region Update

        private ParticipantPerson DoCreateParticipantPerson(Participant participant, Audit audit)
        {
            var participantPerson = new ParticipantPerson();
            participantPerson.Participant = participant;
            
            var createAudit = new Create(audit.User);
            createAudit.SetHistory(participantPerson);
            this.Context.ParticipantPersons.Add(participantPerson);
            return participantPerson;
        }

        /// <summary>
        /// Updates a participant person with given updated participant information.
        /// </summary>
        /// <param name="updatedPerson">The updated participant person.</param>
        public void CreateOrUpdate(UpdatedParticipantPerson updatedPerson)
        {
            var participant = CreateGetParticipantByIdQuery(updatedPerson.ParticipantId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedPerson.ParticipantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(updatedPerson.Audit.User.Id, updatedPerson.ProjectId, participant);

            //should be loaded as part of the query
            var participantPerson = Context.ParticipantPersons.Find(updatedPerson.ParticipantId);
            if (participantPerson == null)
            {
                participantPerson = DoCreateParticipantPerson(participant, updatedPerson.Audit);
            }

            Organization host = null;
            Organization home = null;
            Address hostAddress = null;
            Address homeAddress = null;
            ParticipantStatus participantStatus = null;
            
            if (updatedPerson.HomeInstitutionId.HasValue)
            {
                home = CreateGetInstitutionByIdQuery(updatedPerson.HomeInstitutionId.Value).FirstOrDefault();
                throwIfModelDoesNotExist(updatedPerson.HomeInstitutionId.Value, home, typeof(Organization));
                                
                if (updatedPerson.HomeInstitutionAddressId.HasValue)
                {
                    homeAddress = CreateGetAddressByIdQuery(updatedPerson.HomeInstitutionAddressId.Value, home.OrganizationId).FirstOrDefault();
                    throwIfModelDoesNotExist(updatedPerson.HomeInstitutionAddressId.Value, homeAddress, typeof(Address));
                }
            }
            if (updatedPerson.HostInstitutionId.HasValue)
            {
                host = CreateGetInstitutionByIdQuery(updatedPerson.HostInstitutionId.Value).FirstOrDefault();
                throwIfModelDoesNotExist(updatedPerson.HostInstitutionId.Value, host, typeof(Organization));

                if (updatedPerson.HostInstitutionAddressId.HasValue)
                {
                    hostAddress = CreateGetAddressByIdQuery(updatedPerson.HostInstitutionAddressId.Value, host.OrganizationId).FirstOrDefault();
                    throwIfModelDoesNotExist(updatedPerson.HostInstitutionAddressId.Value, hostAddress, typeof(Address));
                }
            }
            ParticipantType participantType = Context.ParticipantTypes.Find(updatedPerson.ParticipantTypeId);
            throwIfModelDoesNotExist(updatedPerson.ParticipantTypeId, participantType, typeof(ParticipantType));

            if (updatedPerson.ParticipantStatusId.HasValue)
            {
                participantStatus = Context.ParticipantStatuses.Find(updatedPerson.ParticipantStatusId.Value);
                throwIfModelDoesNotExist(updatedPerson.ParticipantStatusId.Value, participantStatus, typeof(ParticipantStatus));
            }

            DoCreateOrUpdate(
                updatedPerson: updatedPerson,
                participant: participant,
                participantPerson: participantPerson,
                participantType: participantType,
                participantStatus: participantStatus,
                host: host,
                home: home,
                hostAddress: hostAddress,
                homeAddress: homeAddress
                );
        }

        /// <summary>
        /// Updates a participant person with given updated participant information.
        /// </summary>
        /// <param name="updatedPerson">The updated participant person.</param>
        /// <returns>The task.</returns>
        public async Task CreateOrUpdateAsync(UpdatedParticipantPerson updatedPerson)
        {
            var participant = await CreateGetParticipantByIdQuery(updatedPerson.ParticipantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedPerson.ParticipantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(updatedPerson.Audit.User.Id, updatedPerson.ProjectId, participant);

            //should be loaded as part of the query
            var participantPerson = await Context.ParticipantPersons.FindAsync(updatedPerson.ParticipantId);
            if (participantPerson == null)
            {
                participantPerson = DoCreateParticipantPerson(participant, updatedPerson.Audit);
            }

            Organization host = null;
            Organization home = null;
            Address hostAddress = null;
            Address homeAddress = null;
            ParticipantStatus participantStatus = null;

            if (updatedPerson.HomeInstitutionId.HasValue)
            {
                home = await CreateGetInstitutionByIdQuery(updatedPerson.HomeInstitutionId.Value).FirstOrDefaultAsync();
                throwIfModelDoesNotExist(updatedPerson.HomeInstitutionId.Value, home, typeof(Organization));

                if (updatedPerson.HomeInstitutionAddressId.HasValue)
                {
                    homeAddress = await CreateGetAddressByIdQuery(updatedPerson.HomeInstitutionAddressId.Value, home.OrganizationId).FirstOrDefaultAsync();
                    throwIfModelDoesNotExist(updatedPerson.HomeInstitutionAddressId.Value, homeAddress, typeof(Address));
                }
            }
            if (updatedPerson.HostInstitutionId.HasValue)
            {
                host = await CreateGetInstitutionByIdQuery(updatedPerson.HostInstitutionId.Value).FirstOrDefaultAsync();
                throwIfModelDoesNotExist(updatedPerson.HostInstitutionId.Value, host, typeof(Organization));

                if (updatedPerson.HostInstitutionAddressId.HasValue)
                {
                    hostAddress = await CreateGetAddressByIdQuery(updatedPerson.HostInstitutionAddressId.Value, host.OrganizationId).FirstOrDefaultAsync();
                    throwIfModelDoesNotExist(updatedPerson.HostInstitutionAddressId.Value, hostAddress, typeof(Address));
                }
            }
            ParticipantType participantType = await Context.ParticipantTypes.FindAsync(updatedPerson.ParticipantTypeId);
            throwIfModelDoesNotExist(updatedPerson.ParticipantTypeId, participantType, typeof(ParticipantType));

            if (updatedPerson.ParticipantStatusId.HasValue)
            {
                participantStatus = await Context.ParticipantStatuses.FindAsync(updatedPerson.ParticipantStatusId.Value);
                throwIfModelDoesNotExist(updatedPerson.ParticipantStatusId.Value, participantStatus, typeof(ParticipantStatus));
            }

            DoCreateOrUpdate(
                updatedPerson: updatedPerson,
                participant: participant,
                participantPerson: participantPerson,
                participantType: participantType,
                participantStatus: participantStatus,
                host: host,
                home: home,
                hostAddress: hostAddress,
                homeAddress: homeAddress
                );
        }

        private void DoCreateOrUpdate(
            UpdatedParticipantPerson updatedPerson,
            Participant participant,
            ParticipantPerson participantPerson,
            ParticipantType participantType,
            ParticipantStatus participantStatus,
            Organization host,
            Organization home,
            Address hostAddress,
            Address homeAddress)
        {
            participantPersonValidator.ValidateUpdate(GetUpdatedPersonParticipantValidationEntity(participantType));
            updatedPerson.Audit.SetHistory(participant);
            updatedPerson.Audit.SetHistory(participantPerson);

            participantPerson.HomeInstitutionId = updatedPerson.HomeInstitutionId;
            participantPerson.HomeInstitutionAddressId = updatedPerson.HomeInstitutionAddressId;

            participantPerson.HostInstitutionId = updatedPerson.HostInstitutionId;
            participantPerson.HostInstitutionAddressId = updatedPerson.HostInstitutionAddressId;
            participant.ParticipantTypeId = updatedPerson.ParticipantTypeId;
            participant.ParticipantStatusId = updatedPerson.ParticipantStatusId;
        }

        private UpdatedParticipantPersonValidationEntity GetUpdatedPersonParticipantValidationEntity(ParticipantType type)
        {
            return new UpdatedParticipantPersonValidationEntity(type);
        }

        #endregion

        private IQueryable<Participant> CreateGetParticipantByIdQuery(int participantId)
        {
            return Context.Participants.Where(x => x.ParticipantId == participantId).Include(x => x.ParticipantPerson);
        }

        private IQueryable<Organization> CreateGetInstitutionByIdQuery(int organizationId)
        {
            return Context.Organizations.Where(x => x.OrganizationId == organizationId);
        }

        private IQueryable<Address> CreateGetAddressByIdQuery(int addressId, int organizationId)
        {
            return Context.Addresses.Where(x => x.AddressId == addressId && x.OrganizationId == organizationId);
        }

    }
}
