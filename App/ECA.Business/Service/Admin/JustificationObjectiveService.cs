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
    public class JustificationObjectiveService : LookupService<JustificationObjectiveDTO>, 
        ECA.Business.Service.Admin.IJustificationObjectiveService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public JustificationObjectiveService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        protected override IQueryable<JustificationObjectiveDTO> GetSelectDTOQuery()
        {
            return JustificationObjectiveQueries.CreateGetJustificationObjectiveDTOQuery(this.Context);
        }

        /// <summary>
        /// Returns the objective with the given id or null if not found.
        /// </summary>
        /// <param name="id">The id of the objective.</param>
        /// <returns>The objective with the given id, or null if not found.</returns>
        public JustificationObjectiveDTO GetJustificationObjectiveById(int id)
        {
            var objective = JustificationObjectiveQueries.CreateGetJustificationObjectiveByIdQuery(this.Context, id).FirstOrDefault();
            this.logger.Trace("Retrieved objective by id {id}.", id);
            return objective;
        }

        public async Task<JustificationObjectiveDTO> GetJustificationObjectiveByIdAsync(int id)
        {
            var objective = await JustificationObjectiveQueries.CreateGetJustificationObjectiveByIdQuery(this.Context, id).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved objective by id {id}.", id);
            return objective;
        }
        #endregion
        
    }
}
