namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    /// <summary>
    /// An PermanentResidenceCountryErrorPath is used when a person's permanent residence country has an error and where it might be located.
    /// </summary>
    public class PermanentResidenceCountryErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public PermanentResidenceCountryErrorPath()
        {
            SetByStaticLookup(SevisErrorType.PermanentResidenceCountry);
        }
    }
}
