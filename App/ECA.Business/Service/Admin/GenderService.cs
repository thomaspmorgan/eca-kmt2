using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Implementation of gender service
    /// </summary>
    public class GenderService : LookupService<SimpleLookupDTO>, IGenderService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name = "saveActions" > The save actions.</param>
        public GenderService(EcaContext context, List<ISaveAction> saveActions = null) 
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Implementation of lookup service method 
        /// </summary>
        /// <returns>List of genders</returns>
        protected override IQueryable<SimpleLookupDTO> GetSelectDTOQuery()
        {
            var query = this.Context.Genders.Select(x => new SimpleLookupDTO
            {
                Id = x.GenderId,
                Value = x.GenderName
            });
            return query;
        }
    }
}
