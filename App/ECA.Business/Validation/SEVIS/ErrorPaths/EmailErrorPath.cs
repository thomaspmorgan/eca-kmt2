namespace ECA.Business.Validation.SEVIS.ErrorPaths
{

    /// <summary>
    /// An EmailErrorPath is used to denote an email error and where it might be located.
    /// </summary>
    public class EmailErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public EmailErrorPath()
        {
            SetByStaticLookup(SevisErrorType.Email);
        }
    }
}
