using ECA.Business.Queries.Persons;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Data;
using NLog;
using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The MembershipService is capable of handling crud operations on a Membership entity
    /// </summary>
    public class MembershipService : DbContextService<EcaContext>, ECA.Business.Service.Persons.IMembershipService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<Person, int> throwIfPersonEntityNotFound;
        private readonly Action<Membership, int> throwIfMembershipNotFound;

        /// <summary>
        /// Creates a new instance and initializes the context.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public MembershipService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfMembershipNotFound = (membership, id) =>
            {
                if (membership == null)
                {
                    throw new ModelNotFoundException(String.Format("The membership with id [{0}] was not found.", id));
                }
            };

            throwIfPersonEntityNotFound = (person, id) =>
                {
                    if(person == null)
                    {
                        throw new ModelNotFoundException(String.Format("The person with id[{0}] was not found.", id));
                    }
                };
        }

        #region Get

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of membership dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        public PagedQueryResults<MembershipDTO> Get(QueryableOperator<MembershipDTO> queryOperator)
        {
            var results = GetMembershipDTOQuery(queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded membership with query operator = [{0}].", queryOperator);
            return results;
        }

        /// <summary>
        /// Returns a paged, filtered, and sorted instance of membership dtos.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted dtos.</returns>
        public async Task<PagedQueryResults<MembershipDTO>> GetAsync(QueryableOperator<MembershipDTO> queryOperator)
        {
            var results = await GetMembershipDTOQuery(queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            logger.Trace("Loaded memberships with query operator = [{0}].", queryOperator);
            return results;
        }
        
        private IQueryable<MembershipDTO> GetMembershipDTOQuery(QueryableOperator<MembershipDTO> queryOperator)
        {
            var query = GetSelectDTOQuery();
            query = query.Apply(queryOperator);
            return query;
        }

        protected IQueryable<MembershipDTO> GetSelectDTOQuery()
        {
            return Context.Memberships.Select(x => new MembershipDTO
            {
                Id = x.MembershipId,
                Name = x.Name
            });
        }

        /// <summary>
        /// Retrieves the membership dto with the given id.
        /// </summary>
        /// <param name="id">The id of the membership.</param>
        /// <returns>The membership dto.</returns>
        public MembershipDTO GetById(int id)
        {
            var dto = MembershipQueries.CreateGetMembershipDTOByIdQuery(this.Context, id).FirstOrDefault();
            logger.Info("Retrieved the membership dto with the given id [{0}].", id);
            return dto;
        }

        /// <summary>
        /// Retrieves the membership dto with the given id.
        /// </summary>
        /// <param name="id">The id of the membership.</param>
        /// <returns>The membership dto.</returns>
        public async Task<MembershipDTO> GetByIdAsync(int id)
        {
            var dto = await MembershipQueries.CreateGetMembershipDTOByIdQuery(this.Context, id).FirstOrDefaultAsync();
            logger.Info("Retrieved the membership dto with the given id [{0}].", id);
            return dto;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a new membership in the ECA system.
        /// </summary>
        /// <param name="personMembership">The membership.</param>
        /// <returns>The created membership entity.</returns>
        public Membership Create(NewPersonMembership personMembership)
        {
            var person = this.Context.People.Find(personMembership.PersonId);
            return DoCreate(personMembership, person);
        }

        /// <summary>
        /// Creates a new membership in the ECA system.
        /// </summary>
        /// <param name="personMembership">The membership.</param>
        /// <returns>The created membership entity.</returns>
        public async Task<Membership> CreateAsync(NewPersonMembership personMembership) 
        {
            var person = await this.Context.People.FindAsync(personMembership.PersonId);
            return DoCreate(personMembership, person);
        }

        private Membership DoCreate(NewPersonMembership personMembership, Person person) 
        {
            throwIfPersonEntityNotFound(person, personMembership.PersonId);
            return personMembership.AddPersonMembership(person);
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates the ECA system's membership data with the given updated membership.
        /// </summary>
        /// <param name="updatedMembership">The updated membership.</param>
        public void Update(UpdatedPersonMembership updatedMembership)
        {
            var membership = Context.Memberships.Find(updatedMembership.Id);
            DoUpdate(updatedMembership, membership);
        }

        /// <summary>
        /// Updates the ECA system's membership data with the given updated membership.
        /// </summary>
        /// <param name="updatedMembership">The updated membership.</param>
        public async Task UpdateAsync(UpdatedPersonMembership updatedMembership)
        {
            var membership = await Context.Memberships.FindAsync(updatedMembership.Id);
            DoUpdate(updatedMembership, membership);
        }

        private void DoUpdate(UpdatedPersonMembership updatedPersonMembership, Membership modelToUpdate)
        {
            Contract.Requires(updatedPersonMembership != null, "The updatedPersonMembership must not be null.");
            throwIfMembershipNotFound(modelToUpdate, updatedPersonMembership.Id);
            modelToUpdate.MembershipId = updatedPersonMembership.Id;
            modelToUpdate.Name = updatedPersonMembership.Name;
            updatedPersonMembership.Update.SetHistory(modelToUpdate);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes the membership entry with the given id.
        /// </summary>
        /// <param name="membershipId">The id of the membership to delete.</param>
        public void Delete(int membershipId)
        {
            var membership = Context.Memberships.Find(membershipId);
            DoDelete(membership);
        }

        /// <summary>
        /// Deletes the membership entry with the given id.
        /// </summary>
        /// <param name="membershipId">The id of the membership to delete.</param>
        public async Task DeleteAsync(int membershipId)
        {
            var membership = await Context.Memberships.FindAsync(membershipId);
            DoDelete(membership);
        }

        private void DoDelete(Membership membershipToDelete)
        {
            if (membershipToDelete != null)
            {
                Context.Memberships.Remove(membershipToDelete);
            }
        }
        #endregion
    }
}
