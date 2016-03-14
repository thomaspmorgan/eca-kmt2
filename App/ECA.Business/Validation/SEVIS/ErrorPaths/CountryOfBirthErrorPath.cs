namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    /// <summary>
    /// An CountryOfBirthErrorPath is used to denote a country of birth error and where it might be located.
    /// </summary>
    public class CountryOfBirthErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public CountryOfBirthErrorPath()
        {
            SetByStaticLookup(SevisErrorType.CountryOfBirth);
        }

    }
}
