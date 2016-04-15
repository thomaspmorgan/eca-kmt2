using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Sevis.Model
{
    public partial class SEVISEVBatchType
    {
        /// <summary>
        /// Returns true if a dependent with the given sevis id was deleted in this batch.
        /// </summary>
        /// <param name="dependentSevisId">The dependent's sevis id.</param>
        /// <returns>True, if a dependent with the given sevis id was deleted in this batch, otherwise false.</returns>
        public bool ContainsDeletedParticipantDependent(string dependentSevisId)
        {
            if(this.UpdateEV == null || this.UpdateEV.Count() == 0)
            {
                return false;
            }
            else
            {
                var updatedExchangeVisitors = this.UpdateEV.ToList();

                var exchangeVisitorDependents = updatedExchangeVisitors
                    .Where(x => x.Item.GetType() == typeof(SEVISEVBatchTypeExchangeVisitorDependent))
                    .Select(x => (SEVISEVBatchTypeExchangeVisitorDependent)x.Item)
                    .ToList();

                var deletedDependents = exchangeVisitorDependents
                    .Where(x => x.Item.GetType() == typeof(SEVISEVBatchTypeExchangeVisitorDependentDelete))
                    .Select(x => (SEVISEVBatchTypeExchangeVisitorDependentDelete)x.Item)
                    .ToList();

                var deletedDependent = deletedDependents.Where(x => x.dependentSevisID == dependentSevisId).FirstOrDefault();
                return deletedDependent != null;
            }
        }
    }
}
