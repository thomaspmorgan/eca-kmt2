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
    public class ModifiedParticipantDependent : ISevisExchangeVisitorUpdatableComponent, IUserDefinable
    {
        /// <summary>
        /// Gets or sets the dependent that has been modified.
        /// </summary>
        public Dependent Dependent { get; set; }

        /// <summary>
        /// Gets or sets the user defined a value.
        /// </summary>
        public string UserDefinedA { get; set; }

        /// <summary>
        /// Gets or sets the user defined b value.
        /// </summary>
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Returns a sevis registered exchnage visitor's dependent modification, such as a new dependent that must be added.
        /// </summary>
        /// <returns>A sevis registered exchnage visitor's dependent modification, such as a new dependent that must be added.</returns>
        public SEVISEVBatchTypeExchangeVisitorDependent GetSEVISEVBatchTypeExchangeVisitorDependent()
        {
            //should return SEVISEVBatchTypeExchangeVisitorDependent with the correct Item property set to either
            //SEVISEVBatchTypeExchangeVisitorDependentAdd, SEVISEVBatchTypeExchangeVisitorDependentDelete, etc....
            return new SEVISEVBatchTypeExchangeVisitorDependent
            {
                Item = Dependent.GetSevisExhangeVisitorDependentInstance(),
                UserDefinedA = this.UserDefinedA,
                UserDefinedB = this.UserDefinedB
            };
        }

        /// <summary>
        /// Returns the SEVISEVBatchTypeExchangeVisitorDependent instance. 
        /// </summary>
        /// <returns>Returns the SEVISEVBatchTypeExchangeVisitorDependent instance.</returns>
        public object GetSevisEvBatchTypeExchangeVisitorUpdateComponent()
        {
            return GetSEVISEVBatchTypeExchangeVisitorDependent();
        }
    }
}
