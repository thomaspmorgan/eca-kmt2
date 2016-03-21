namespace ECA.Business.Validation.Sevis
{
    /// <summary>
    /// An IFormPrintable model is capable of retrieving it's print form value.
    /// </summary>
    public interface IFormPrintable
    {
        /// <summary>
        /// Gets the print form flab.
        /// </summary>
        bool PrintForm { get; }
    }
}
