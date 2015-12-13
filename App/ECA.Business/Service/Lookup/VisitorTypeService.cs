using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// An VisitorTypeService is a service handling a lookup on a vistor type.
    /// </summary>
    public class VisitorTypeService : LookupService<SimpleLookupDTO>, IVisitorTypeService
    {
        /// <summary>
        /// Creates a new VisitorTypeService.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public VisitorTypeService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns a query to get dtos.
        /// </summary>
        /// <returns>The query to get visitor type dtos.</returns>
        protected override IQueryable<SimpleLookupDTO> GetSelectDTOQuery()
        {
            return this.Context.VisitorTypes.Select(x => new SimpleLookupDTO
            {
                Id = x.VisitorTypeId,
                Value = x.VisitorTypeName
            });
        }
        #endregion
    }
}
