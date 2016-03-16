using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Finance
{
    /// <summary>
    /// OrganizationFunding is used to represent one or two funding sources and the amount of funding to an exchange visitor.
    /// </summary>
    public class OrganizationFunding
    {
        /// <summary>
        /// Creates a new instance with the given funding details.
        /// </summary>
        /// <param name="org1">The first organization providing funding.  This value is a code.</param>
        /// <param name="otherName1">The name of the first organization if the first organization is 'OTHER'.</param>
        /// <param name="amount1">The first organization funding amount in whole dollars.</param>
        /// <param name="org2">The second organization providing funding.  This value is a code.</param>
        /// <param name="otherName2">The name of the second organization if the second organization is 'OTHER'.</param>
        /// <param name="amount2">The second organization's funding amount in whole dollars.</param>
        public OrganizationFunding(
            string org1,
            string otherName1,
            string amount1,
            string org2,
            string otherName2,
            string amount2)
        {
            this.Org1 = org1;
            this.OtherName1 = otherName1;
            this.Amount1 = amount1;
            this.Org2 = org2;
            this.OtherName2 = otherName2;
            this.Amount2 = amount2;
        }

        /// <summary>
        /// Gets the Org1 name.
        /// </summary>
        public string Org1 { get; private set; }

        /// <summary>
        /// Gets the Org1 other name.
        /// </summary>
        public string OtherName1 { get; private set; }

        /// <summary>
        /// Gets the org 1 funding amount.
        /// </summary>
        public string Amount1 { get; private set; }

        /// <summary>
        /// Gets the Org1 name.
        /// </summary>
        public string Org2 { get; private set; }

        /// <summary>
        /// Gets the Org 2 other name.
        /// </summary>
        public string OtherName2 { get; private set; }

        /// <summary>
        /// Gets the org 2 funding amount.
        /// </summary>
        public string Amount2 { get; private set; }
    }
}
