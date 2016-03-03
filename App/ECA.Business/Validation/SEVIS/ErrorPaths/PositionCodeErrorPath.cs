using ECA.Business.Validation.SEVIS.ErrorPaths;

namespace ECA.Business.Validation.SEVIS
{
    /// <summary>
    /// An PositionCodeErrorPath is used when a participant's position code has an error and where it might be located.
    /// </summary>
    public class PositionCodeErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public PositionCodeErrorPath()
        {
            SetByStaticLookup(SevisErrorType.PositionCode);
        }
    }
}
