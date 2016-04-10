using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Data
{
    /// <summary>
    /// The SevisAccount is used to hold a principal's sevis account credentials.
    /// </summary>
    [Table("CAM.SevisAccount")]
    public class SevisAccount
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        public int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the org id.
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Principal.
        /// </summary>
        public virtual Principal Principal { get; set; }
    }
}
