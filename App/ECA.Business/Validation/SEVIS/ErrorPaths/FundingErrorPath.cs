namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    /// <summary>
    /// An FundingErrorPath is used when a participant has a sevis funding error and where it might be located.
    /// </summary>
    public class FundingErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public FundingErrorPath()
        {
            SetByStaticLookup(SevisErrorType.Funding);
        }
    }
}
