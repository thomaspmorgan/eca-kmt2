using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// An UpdatedDependentBiography is used to update a dependent's biographical information in sevis.
    /// </summary>
    public class UpdatedDependent : Dependent, ISevisIdentifable, IRemarkable, ISevisExchangeVisitorUpdatableComponent
    {
        public string SevisId { get; set; }

        public string Remarks { get; set; }

        public bool IsRelationshipFieldSpecified { get; set; }

        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependentEdit instance for when a sevis registered exchange visitor
        /// must have a dependent updated in sevis.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependentEdit instance for when a sevis registered exchange visitor
        /// must have a dependent updated in sevis.</returns>
        public override object GetSevisExhangeVisitorDependentInstance()
        {
            return new SEVISEVBatchTypeExchangeVisitorDependentEdit
            {

            };
        }

        public object GetSevisEvBatchTypeExchangeVisitorUpdateComponent()
        {
            return GetSevisExhangeVisitorDependentInstance();
        }
    }
}
