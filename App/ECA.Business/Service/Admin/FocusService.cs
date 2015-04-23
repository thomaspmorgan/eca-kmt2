using ECA.Business.Queries.Admin;
using System.Data.Entity;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Service.Lookup;
using System.Diagnostics;
using NLog;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The FocusService is capable of performing crud operations on ECA Foci.
    /// </summary>
    public class FocusService : LookupService<FocusDTO>, ECA.Business.Service.Admin.IFocusService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new FocusSerivce with the given context.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public FocusService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns a query to dtos.
        /// </summary>
        /// <returns>A query to return focus dtos.</returns>
        protected override IQueryable<FocusDTO> GetSelectDTOQuery()
        {
            return FocusQueries.CreateGetFocusDTOQuery(this.Context);
        }

        /// <summary>
        /// Returns the focus with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        public FocusDTO GetFocusById(int id)
        {
            var focus = FocusQueries.CreateGetFocusByIdQuery(this.Context, id).FirstOrDefault();
            this.logger.Trace("Retrieved focus by id {0}.", id);            
            return focus;
        }

        /// <summary>
        /// Returns the focus with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        public async Task<FocusDTO> GetFocusByIdAsync(int id)
        {
            var focus = await FocusQueries.CreateGetFocusByIdQuery(this.Context, id).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved focus by id {0}.", id);
            return focus;
        }
        #endregion


    }
}
