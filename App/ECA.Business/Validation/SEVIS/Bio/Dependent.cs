using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// A Dependent instance is used to specify what action will be taken on a sevis registered exchange visitor dependent, such as
    /// adding a new dependenting, deleting a sevis registered dependent, or editing a sevis registered dependent.
    /// </summary>
    public abstract class Dependent : Biography, IUserDefinable
    {
        /// <summary>
        /// Gets or sets the user defined a field.
        /// </summary>
        public string UserDefinedA { get; set; }

        /// <summary>
        /// Gets or sets the user defined b field.
        /// </summary>
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance used when performing
        /// a sevis registered exchange visitor's dependent details.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependent(Add|Delete|Edit|EndStatus|Reprint|Terminate) instance.</returns>
        public abstract object GetSevisExhangeVisitorDependentInstance();
    }
}
