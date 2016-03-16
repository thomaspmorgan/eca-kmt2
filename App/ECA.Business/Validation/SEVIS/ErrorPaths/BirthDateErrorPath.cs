namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    /// <summary>
    /// An BirthDateErrorPath is used to denote a birth date error and where it might be located.
    /// </summary>
    public class BirthDateErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public BirthDateErrorPath()
        {
            SetByStaticLookup(SevisErrorType.BirthDate);
        }
    }
}
