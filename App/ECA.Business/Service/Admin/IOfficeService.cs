using ECA.Business.Queries.Models.Admin;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
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
    }
}
