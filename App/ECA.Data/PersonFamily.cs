using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class PersonFamily : IHistorical
    {
        //public PersonFamily()
        //{
        //    Dependents = new HashSet<Person>();
        //}

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the dependent id.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int RelatedPersonId { get; set; }

        /// <summary>
        /// the SEVIS ID (assigned by SEVIS)
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// create/update time and user
        /// </summary>
        public History History { get; set; }
        

        //public virtual ICollection<Person> Dependents { get; set; }
    }
}
