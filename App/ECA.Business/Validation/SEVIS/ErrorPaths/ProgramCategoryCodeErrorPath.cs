using ECA.Business.Validation.SEVIS.ErrorPaths;

namespace ECA.Business.Validation.SEVIS
{
    /// <summary>
    /// An ProgramCategoryCodeErrorPath is used when a participant's program category code has an error and where it might be located.
    /// </summary>
    public class ProgramCategoryCodeErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public ProgramCategoryCodeErrorPath()
        {
            SetByStaticLookup(SevisErrorType.ProgramCategoryCode);
        }
    }
}
