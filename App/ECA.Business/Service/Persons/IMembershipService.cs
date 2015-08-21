using ECA.Business.Queries.Models.Persons;
using ECA.Core.Service;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An ImembershipService is used to create or update membership presences for ECA objects.
    /// </summary>
    [ContractClass(typeof(MembershipServiceContract))]
    public interface IMembershipService : ISaveable
    {
        /// <summary>
        /// Creates a new membership in the ECA system.
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <returns>The created membership entity.</returns>
        Membership Create(NewPersonMembership membership);

        /// <summary>
        /// Creates a new membership in the ECA system.
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <returns>The created membership entity.</returns>
        Task<Membership> CreateAsync(NewPersonMembership membership);

        /// <summary>
        /// Updates the ECA system's membership data with the given updated membership.
        /// </summary>
        /// <param name="updatedmembership">The updated membership.</param>
        void Update(UpdatedPersonMembership updatedmembership);

        /// <summary>
        /// Updates the ECA system's membership data with the given updated membership.
        /// </summary>
        /// <param name="updatedmembership">The updated membership.</param>
        Task UpdateAsync(UpdatedPersonMembership updatedmembership);

        /// <summary>
        /// Returns paged, filtered, and sorted Memberships in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The memberships in the system.</returns>
        PagedQueryResults<MembershipDTO> Get(QueryableOperator<MembershipDTO> queryOperator);

        /// <summary>
        /// Returns paged, filtered, and sorted memberships in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The memberships in the system.</returns>
        Task<PagedQueryResults<MembershipDTO>> GetAsync(QueryableOperator<MembershipDTO> queryOperator);

        /// <summary>
        /// Retrieves the membership dto with the given id.
        /// </summary>
        /// <param name="id">The id of the membership.</param>
        /// <returns>The membership dto.</returns>
        MembershipDTO GetById(int id);

        /// <summary>
        /// Retrieves the membership dto with the given id.
        /// </summary>
        /// <param name="id">The id of the membership.</param>
        /// <returns>The membership dto.</returns>
        Task<MembershipDTO> GetByIdAsync(int id);

        /// <summary>
        /// Deletes the membership entry with the given id.
        /// </summary>
        /// <param name="membershipId">The id of the membership to delete.</param>
        void Delete(int membershipId);

        /// <summary>
        /// Deletes the membership entry with the given id.
        /// </summary>
        /// <param name="membershipId">The id of the membership to delete.</param>
        Task DeleteAsync(int membershipId);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IMembershipService))]
    public abstract class MembershipServiceContract : IMembershipService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="membership"></param>
        /// <returns></returns>
        public Membership Create(NewPersonMembership membership) {
            Contract.Requires(membership != null, "The membership entity must not be null.");
            Contract.Ensures(Contract.Result<Membership>() != null, "The membership entity returned must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membership"></param>
        /// <returns></returns>
        public Task<Membership> CreateAsync(NewPersonMembership membership)
        {
            Contract.Requires(membership != null, "The membership entity must not be null.");
            Contract.Ensures(Contract.Result<Task<Membership>>() != null, "The membership entity returned must not be null.");
            return Task.FromResult<Membership>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedmembership"></param>
        public void Update(UpdatedPersonMembership updatedmembership)
        {
            Contract.Requires(updatedmembership != null, "The updated membership must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedmembership"></param>
        /// <returns></returns>
        public Task UpdateAsync(UpdatedPersonMembership updatedmembership)
        {
            Contract.Requires(updatedmembership != null, "The updated membership must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// Returns paged, filtered, and sorted Memberships in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The memberships in the system.</returns>
        public PagedQueryResults<MembershipDTO> Get(QueryableOperator<MembershipDTO> queryOperator)
        {
            return null;
        }

        /// <summary>
        /// Returns paged, filtered, and sorted memberships in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The memberships in the system.</returns>
        public Task<PagedQueryResults<MembershipDTO>> GetAsync(QueryableOperator<MembershipDTO> queryOperator)
        {
            return Task.FromResult<PagedQueryResults<MembershipDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MembershipDTO GetById(int id)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MembershipDTO> GetByIdAsync(int id)
        {
            return Task.FromResult<MembershipDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipId"></param>
        public void Delete(int membershipId)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membershipId"></param>
        /// <returns></returns>
        public Task DeleteAsync(int membershipId)
        {
            return Task.FromResult<object>(null);
        }
    }
}
