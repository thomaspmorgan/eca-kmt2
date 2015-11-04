using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;


namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// The PositionService service is used to perform crud operations on SEVIS positions.
    /// </summary>
    public class PositionService : LookupService<SimpleSevisLookupDTO>, IPositionService
    {
        /// <summary>
        /// Creates a new instance with the context to query.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to query.</param>
        public PositionService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Gets a set of Sevis Position dtos
        /// </summary>
        /// <returns>SimpleSevisLookupDTO</returns>
        protected override IQueryable<SimpleSevisLookupDTO> GetSelectDTOQuery()
        {
            return this.Context.Positions.Select(x => new SimpleSevisLookupDTO
            {
                Id = x.PositionId,
                Code = x.PositionCode,
                Description = x.Description
            });
        }
    }
}
