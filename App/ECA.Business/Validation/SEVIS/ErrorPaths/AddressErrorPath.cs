namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    /// <summary>
    /// An AddressErrorPath is used to denote an address error and where it might be located.
    /// </summary>
    public class AddressErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public AddressErrorPath()
        {
            SetByStaticLookup(SevisErrorType.Address);
        }
    }
}
