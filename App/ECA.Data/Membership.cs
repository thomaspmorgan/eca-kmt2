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
    public class Membership
    {
        [Key]
        public int MembershipId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
