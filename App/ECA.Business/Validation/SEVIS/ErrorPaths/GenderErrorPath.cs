namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    /// <summary>
    /// An GenderErrorPath is used when a person's gender has an error and where it might be located.
    /// </summary>
    public class GenderErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public GenderErrorPath()
        {
            SetByStaticLookup(SevisErrorType.Gender);
        }
    }
}
