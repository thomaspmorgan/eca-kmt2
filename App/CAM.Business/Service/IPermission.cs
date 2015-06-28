namespace CAM.Business.Service
{
    /// <summary>
    /// An IPermission is a object that contains principal permission details.
    /// </summary>
    public interface IPermission
    {
        /// <summary>
        /// Gets or sets the principal id.
        /// </summary>
        int PrincipalId { get; set; }

        /// <summary>
        /// Gets or sets the permission id.
        /// </summary>
        int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        int ResourceId { get; set; }

        /// <summary>
        /// Gets or sets is allowed.
        /// </summary>
        bool IsAllowed { get; set; }

        /// <summary>
        /// Gets or sets the foreign resource id.
        /// </summary>
        int ForeignResourceId { get; set; }

        /// <summary>
        /// Gets or sets the resource type id.
        /// </summary>
        int ResourceTypeId { get; set; }
    }
}
