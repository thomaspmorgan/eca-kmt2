using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A list of professional organizations the participant belongs to 
    /// Example: Chamber of Commerce, Society of Physicists 
    /// </summary>
    public class Membership: IHistorical
    {
        private const int MAX_NAME_LENGTH = 255;

        [Key]
        public int MembershipId { get; set; }

        [Required]
        [MaxLength(MAX_NAME_LENGTH)]
        public string Name { get; set; }

        [Required]
        public int PersonId { get; set; }
        
        public Person Person { get; set; }

        public History History { get; set; }
    }
}
