using ECA.Business.Validation.SEVIS.ErrorPaths;

namespace ECA.Business.Validation.SEVIS
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
