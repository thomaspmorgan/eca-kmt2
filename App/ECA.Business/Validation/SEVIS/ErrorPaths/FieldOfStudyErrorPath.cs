using ECA.Business.Validation.Sevis.ErrorPaths;

namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    /// <summary>
    /// An FieldOfStudyErrorPath is used to denote a field of study and where it might be located.
    /// </summary>
    public class FieldOfStudyErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public FieldOfStudyErrorPath()
        {
            SetByStaticLookup(SevisErrorType.FieldOfStudy);
        }
    }
}
