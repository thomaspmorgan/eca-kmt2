using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// A ModifiedParticipantDependent is used to represent a change that must be made to a sevis registered exchange visitor.  More specifically,
    /// this class will be used to return a SEVISEVBatchTypeExchangeVisitorDependent which has the added, modified, etc dependent payload.
    /// </summary>
    public class ModifiedParticipantDependent
    {
        public ModifiedParticipantDependent(Dependent dependent)
        {
            this.Dependent = dependent;
        }

        /// <summary>
        /// Gets or sets the dependent that has been modified.
        /// </summary>
        public Dependent Dependent { get; private set; }

        /// <summary>
        /// Returns a sevis registered exchnage visitor's dependent modification, such as a new dependent that must be added.
        /// </summary>
        /// <returns>A sevis registered exchnage visitor's dependent modification, such as a new dependent that must be added.</returns>
        public SEVISEVBatchTypeExchangeVisitorDependent GetSEVISEVBatchTypeExchangeVisitorDependent()
        {
            //should return SEVISEVBatchTypeExchangeVisitorDependent with the correct Item property set to either
            //SEVISEVBatchTypeExchangeVisitorDependentAdd, SEVISEVBatchTypeExchangeVisitorDependentDelete, etc....
            var instance = new SEVISEVBatchTypeExchangeVisitorDependent
            {
                Item = Dependent.GetSevisExhangeVisitorDependentInstance(),
            };
            var key = new ParticipantSevisKey(this.Dependent);
            key.SetUserDefinedFields(instance);

            return instance;
        }       
    }
}
