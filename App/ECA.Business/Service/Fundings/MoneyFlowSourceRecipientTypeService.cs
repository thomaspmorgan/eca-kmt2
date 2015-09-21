using ECA.Business.Queries.Models.Admin;
using System.Data.Entity;
using ECA.Business.Service.Lookup;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// The MoneyFlowSourceRecipientTypeService is used to perform crud and business operations on money flow source recipient types.
    /// </summary>
    public class MoneyFlowSourceRecipientTypeService : LookupService<MoneyFlowSourceRecipientTypeDTO>, IMoneyFlowSourceRecipientTypeService
    {

        /// <summary>
        /// Creates a new ProjectTypeService with the context and logger.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public MoneyFlowSourceRecipientTypeService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns a query to retrieve money flow source recipient type dtos.
        /// </summary>
        /// <returns>A query to get money flow source recipient type dtos.</returns>
        protected override IQueryable<MoneyFlowSourceRecipientTypeDTO> GetSelectDTOQuery()
        {
            var query = this.Context.MoneyFlowSourceRecipientTypes.Select(x => x);
            return CreateSelectMoneyFlowSourceRecipientDTOQuery(query);
        }

        /// <summary>
        /// Returns the list of MoneyFlowSourceRecipientTypes that are allowable recipient types of the given 
        /// money flow source recipient type by id.
        /// </summary>
        /// <returns>The list of MoneyFlowSourceRecipientTypes that are valid recipient types for the MoneyFlowSourceRecipientType with the given id.</returns>
        public List<MoneyFlowSourceRecipientTypeDTO> GetRecipientMoneyFlowTypes(int moneyFlowSourceRecipientTypeId)
        {
            return CreateGetRecipientMoneyFlowSourceRecipientTypeDTOsQuery(moneyFlowSourceRecipientTypeId).ToList();
        }

        /// <summary>
        /// Returns the list of MoneyFlowSourceRecipientTypes that are allowable recipient types of the given 
        /// money flow source recipient type by id.
        /// </summary>
        /// <returns>The list of MoneyFlowSourceRecipientTypes that are valid recipient types for the MoneyFlowSourceRecipientType with the given id.</returns>
        public Task<List<MoneyFlowSourceRecipientTypeDTO>> GetRecipientMoneyFlowTypesAsync(int moneyFlowSourceRecipientTypeId)
        {
            return CreateGetRecipientMoneyFlowSourceRecipientTypeDTOsQuery(moneyFlowSourceRecipientTypeId).ToListAsync();
        }

        /// <summary>
        /// Returns the list of MoneyFlowSourceRecipientTypes that are allowable source types of the given 
        /// money flow source recipient type by id.
        /// </summary>
        /// <returns>The list of MoneyFlowSourceRecipientTypes that are valid recipient types for the MoneyFlowSourceRecipientType with the given id.</returns>
        public List<MoneyFlowSourceRecipientTypeDTO> GetSourceMoneyFlowTypes(int moneyFlowSourceRecipientTypeId)
        {
            return CreateGetSourceMoneyFlowSourceRecipientTypeDTOsQuery(moneyFlowSourceRecipientTypeId).ToList();
        }

        /// <summary>
        /// Returns the list of MoneyFlowSourceRecipientTypes that are allowable source types of the given 
        /// money flow source recipient type by id.
        /// </summary>
        /// <returns>The list of MoneyFlowSourceRecipientTypes that are valid recipient types for the MoneyFlowSourceRecipientType with the given id.</returns>
        public Task<List<MoneyFlowSourceRecipientTypeDTO>> GetSourceMoneyFlowTypesAsync(int moneyFlowSourceRecipientTypeId)
        {
            return CreateGetSourceMoneyFlowSourceRecipientTypeDTOsQuery(moneyFlowSourceRecipientTypeId).ToListAsync();
        }

        private IQueryable<MoneyFlowSourceRecipientTypeDTO> CreateGetRecipientMoneyFlowSourceRecipientTypeDTOsQuery(int moneyFlowSourceRecipientTypeId)
        {
            var query = CreateGetRecipientMoneyFlowSourceRecipientTypeSettingsQuery(moneyFlowSourceRecipientTypeId).Select(x => x.PeerMoneyFlowSourceRecipientType);
            return CreateSelectMoneyFlowSourceRecipientDTOQuery(query);
        }

        private IQueryable<MoneyFlowSourceRecipientTypeDTO> CreateGetSourceMoneyFlowSourceRecipientTypeDTOsQuery(int moneyFlowSourceRecipientTypeId)
        {
            var query = CreateGetSourceMoneyFlowSourceRecipientTypeSettingsQuery(moneyFlowSourceRecipientTypeId).Select(x => x.PeerMoneyFlowSourceRecipientType);
            return CreateSelectMoneyFlowSourceRecipientDTOQuery(query);
        }

        private IQueryable<MoneyFlowSourceRecipientTypeDTO> CreateSelectMoneyFlowSourceRecipientDTOQuery(IQueryable<MoneyFlowSourceRecipientType> query)
        {
            return query.Select(x => new MoneyFlowSourceRecipientTypeDTO
            {
                Id = x.MoneyFlowSourceRecipientTypeId,
                Name = x.TypeName
            });
        }

        private IQueryable<MoneyFlowSourceRecipientTypeSetting> CreateGetSourceMoneyFlowSourceRecipientTypeSettingsQuery(int sourceRecipientTypeId)
        {
            return CreateGetMoneyFlowSourceReceipientTypeSettingsQuery(sourceRecipientTypeId).Where(x => x.IsSource);
        }

        private IQueryable<MoneyFlowSourceRecipientTypeSetting> CreateGetRecipientMoneyFlowSourceRecipientTypeSettingsQuery(int sourceRecipientTypeId)
        {
            return CreateGetMoneyFlowSourceReceipientTypeSettingsQuery(sourceRecipientTypeId).Where(x => x.IsReceipient);
        }

        private IQueryable<MoneyFlowSourceRecipientTypeSetting> CreateGetMoneyFlowSourceReceipientTypeSettingsQuery(int moneyFlowSourceRecipientTypeId)
        {
            return this.Context.MoneyFlowSourceRecipientTypeSettings.Where(x => x.MoneyFlowSourceRecipientTypeId == moneyFlowSourceRecipientTypeId);
        }

        #endregion
    }
}
