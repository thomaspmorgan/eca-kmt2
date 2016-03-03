namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    /// <summary>
    /// An CityOfBirthErrorPath is used to denote a city of birth error and where it might be located.
    /// </summary>
    public class CityOfBirthErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public CityOfBirthErrorPath()
        {
            SetByStaticLookup(SevisErrorType.CityOfBirth);
        }
    }
}
