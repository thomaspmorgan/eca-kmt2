namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    public class DependentErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public DependentErrorPath()
        {
            SetByStaticLookup(SevisErrorType.Dependent);
        }
    }
}
