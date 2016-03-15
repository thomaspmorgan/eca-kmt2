namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    /// <summary>
    /// An FullNameErrorPath is used to denote a name error and where it might be located.
    /// </summary>
    public class FullNameErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public FullNameErrorPath()
        {
            SetByStaticLookup(SevisErrorType.FullName);
        }
    }
}
