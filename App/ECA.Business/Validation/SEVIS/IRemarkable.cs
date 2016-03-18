namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// An IRemarkable object is capable of retrieving its remarks.
    /// </summary>
    public interface IRemarkable
    {
        /// <summary>
        /// Gets the remarks.
        /// </summary>
        string Remarks { get; }
    }
}
