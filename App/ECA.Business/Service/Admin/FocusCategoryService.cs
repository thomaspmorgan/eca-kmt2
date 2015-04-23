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
    public class FocusCategoryService : LookupService<FocusCategoryDTO>, ECA.Business.Service.Admin.IFocusCategoryService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Creates a new FocusCategorySerivce with the given context.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">The logger.</param>
        public FocusCategoryService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns a query to dtos.
        /// </summary>
        /// <returns>A query to return focusCategory dtos.</returns>
        protected override IQueryable<FocusCategoryDTO> GetSelectDTOQuery()
        {
            return FocusCategoryQueries.CreateGetFocusCategoryDTOQuery(this.Context);
        }

        /// <summary>
        /// Returns the focusCategory with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focusCategory.</param>
        /// <returns>The focus with the given id, or null if not found.</returns>
        public FocusCategoryDTO GetFocusCategoryById(int id)
        {
            var focusCategory = FocusCategoryQueries.CreateGetFocusCategoryByIdQuery(this.Context, id).FirstOrDefault();
            this.logger.Trace("Retrieved focus category by id {id}.", id);            
            return focusCategory;
        }

        /// <summary>
        /// Returns the focusCategory with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the focus.</param>
        /// <returns>The focusCategory with the given id, or null if not found.</returns>
        public async Task<FocusCategoryDTO> GetFocusCategoryByIdAsync(int id)
        {
            var focusCategory = await FocusCategoryQueries.CreateGetFocusCategoryByIdQuery(this.Context, id).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved focus category by id {id}.", id);
            return focusCategory;
        }
        #endregion


    }
}
