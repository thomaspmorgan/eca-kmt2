using CAM.Business.Model;

namespace ECA.Business.Service
{
    /// <summary>
    /// An IAuditable business entity is a entity that track audit details when being created or updated.
    /// </summary>
    public interface IAuditable
    {
        /// <summary>
        /// Gets the audit entity.
        /// </summary>
        Audit Audit { get; }
    }
}