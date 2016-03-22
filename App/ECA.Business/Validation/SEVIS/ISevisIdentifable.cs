namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// An ISevisIdentifiable model is capable of retrieving its sevis id.
    /// </summary>
    public interface ISevisIdentifable
    {
        /// <summary>
        /// Gets the sevis id.
        /// </summary>
        string SevisId { get; }
    }
}
