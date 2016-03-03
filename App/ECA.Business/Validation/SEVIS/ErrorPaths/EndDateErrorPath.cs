namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    /// <summary>
    /// An EndDateErrorPath is used to denote a participant traveling end date error and where it might be located.
    /// </summary>
    public class EndDateErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public EndDateErrorPath()
        {
            SetByStaticLookup(SevisErrorType.EndDate);
        }
    }
}
