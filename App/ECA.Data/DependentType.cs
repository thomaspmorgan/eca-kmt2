using System.Collections.Generic;

namespace ECA.Data
{
    /// <summary>
    /// The dependent type entity is used to detail the dependent's relationship to a participant.
    /// </summary>
    public partial class DependentType
    {
        /// <summary>
        /// Creates a new default instance and initializes the people collection.
        /// </summary>
        public DependentType()
        {
            this.Dependents = new HashSet<PersonDependent>();
        }

        /// <summary>
        /// Gets or sets the person type id.
        /// </summary>
        public int DependentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the person type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sevis dependent type code i.e. child = 02, spouse = 01.
        /// </summary>
        public string SevisDependentTypeCode { get; set; }
        
        /// <summary>
        /// Gets or sets the people of this person type.
        /// </summary>
        public virtual ICollection<PersonDependent> Dependents { get; set; }
    }
}
