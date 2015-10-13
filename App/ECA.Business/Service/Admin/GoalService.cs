using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
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

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The GoalService is capable of performing crud operations against and entity framework context.
    /// </summary>
    public class GoalService : LookupService<GoalDTO>, IGoalService
    {
        /// <summary>
        /// Creates a new GoalService with the context to operate against.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions"> The save actions.</param>
        public GoalService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns a query to get dtos.
        /// </summary>
        /// <returns>The query to get goal dtos.</returns>
        protected override IQueryable<GoalDTO> GetSelectDTOQuery()
        {
            return GoalQueries.CreateGetGoalDTOQuery(this.Context);
        }
        #endregion
    }
}
