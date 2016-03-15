namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    /// <summary>
    /// An CitizenshipErrorPath is used to denote a country of citizenship error and where it might be located.
    /// </summary>
    public class CitizenshipErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public CitizenshipErrorPath()
        {
            SetByStaticLookup(SevisErrorType.Citizenship);
        }
    }
}
