namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    /// <summary>
    /// An StartDateErrorPath is used when a participant's traveling start date has an error and where it might be located.
    /// </summary>
    public class StartDateErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public StartDateErrorPath()
        {
            SetByStaticLookup(SevisErrorType.StartDate);
        }
    }
}
