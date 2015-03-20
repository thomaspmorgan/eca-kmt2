using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Office;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An IOfficeService is capable of performing crud operations on an office.
    /// </summary>
    [ContractClass(typeof(OfficeServiceContract))]
    public interface IOfficeService
    {
        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The office with the given id or null.</returns>
        OfficeDTO GetOfficeById(int officeId);

        /// <summary>
        /// Returns the office with the given id.
        /// </summary>
        /// <param name="officeId">The id of the office.</param>
        /// <returns>The office with the given id or null.</returns>
        Task<OfficeDTO> GetOfficeByIdAsync(int officeId);

        /// <summary>
        /// Returns the programs for the office with the given id.
        /// </summary>
        /// <param name="officeId">The id the office.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office programs.</returns>
        PagedQueryResults<OrganizationProgramDTO> GetPrograms(int officeId, QueryableOperator<OrganizationProgramDTO> queryOperator);

        /// <summary>
        /// Returns the programs for the office with the given id.
        /// </summary>
        /// <param name="officeId">The id the office.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office programs.</returns>
        Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsAsync(int officeId, QueryableOperator<OrganizationProgramDTO> queryOperator);

        /// <summary>
        /// Returns the first level child offices/branches/divisions of the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <returns>The child offices, branches, and divisions.</returns>
        List<SimpleOfficeDTO> GetChildOffices(int officeId);

        /// <summary>
        /// Returns the first level child offices/branches/divisions of the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <returns>The child offices, branches, and divisions.</returns>
        Task<List<SimpleOfficeDTO>> GetChildOfficesAsync(int officeId);

        /// <summary>
        /// Returns a list of offices in hierarchical order
        /// </summary>
        /// <returns></returns>
        List<SimpleOfficeDTO> GetOffices();

    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IOfficeService))]
    public abstract class OfficeServiceContract : IOfficeService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public OfficeDTO GetOfficeById(int officeId)
        {
            return null;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public Task<OfficeDTO> GetOfficeByIdAsync(int officeId)
        {
            return Task.FromResult<OfficeDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<OrganizationProgramDTO> GetPrograms(int officeId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<OrganizationProgramDTO>> GetProgramsAsync(int officeId, QueryableOperator<OrganizationProgramDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<OrganizationProgramDTO>>(null);
        }

        /// Get list of Offices (Simple hierarchial list)
        /// </summary>
        /// <returns></returns>
        public List<SimpleOfficeDTO> GetOffices()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public List<SimpleOfficeDTO> GetChildOffices(int officeId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public Task<List<SimpleOfficeDTO>> GetChildOfficesAsync(int officeId)
        {
            return Task.FromResult<List<SimpleOfficeDTO>>(null);
        }
    }
}
