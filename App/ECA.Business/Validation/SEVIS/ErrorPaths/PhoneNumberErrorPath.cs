namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    /// <summary>
    /// An PhoneNumberErrorPath is used when a person'sp hone number has an error and where it might be located.
    /// </summary>
    public class PhoneNumberErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public PhoneNumberErrorPath()
        {
            SetByStaticLookup(SevisErrorType.PhoneNumber);
        }
    }
}
