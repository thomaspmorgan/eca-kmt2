using ECA.Business.Validation.Sevis.ErrorPaths;

namespace ECA.Business.Validation.Sevis.ErrorPaths
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
