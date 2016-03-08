using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// The person type entity is used to detail whether the person is a participant or a dependent of another participant.
    /// </summary>
    public partial class PersonType
    {
        /// <summary>
        /// Creates a new default instance and initializes the people collection.
        /// </summary>
        public PersonType()
        {
            this.People = new HashSet<Person>();
        }

        /// <summary>
        /// Gets or sets the person type id.
        /// </summary>
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the person type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sevis dependent type code i.e. child = 02, spouse = 01.
        /// </summary>
        public string SevisDependentTypeCode { get; set; }

        /// <summary>
        /// Gets or sets whether this person type is a dependent type.
        /// </summary>
        public bool IsDependentPersonType { get; set; }

        /// <summary>
        /// Gets or sets the people of this person type.
        /// </summary>
        public virtual ICollection<Person> People { get; set; }
    }
}
