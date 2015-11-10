using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    [Table("UserView")]
    public class UserAccount
    {
        /// <summary>
        /// The name of a role that should be granted all permissions for permissable entities by resource type.
        /// </summary>
        public const string KMT_SUPER_USER_ROLE_NAME = "KMT Super User";

        [Key]
        public int PrincipalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
    }
}
