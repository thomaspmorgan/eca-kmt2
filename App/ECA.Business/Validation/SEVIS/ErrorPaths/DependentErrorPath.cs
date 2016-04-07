namespace ECA.Business.Validation.Sevis.ErrorPaths
{
    public class DependentErrorPath : ErrorPath
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        /// <param name="personDependentId">The id of the person dependent.</param>
        public DependentErrorPath(int personDependentId)
        {
            SetByStaticLookup(SevisErrorType.Dependent);
            this.PersonDependentId = personDependentId;
        }

        /// <summary>
        /// Gets the Person Dependent Id.
        /// </summary>
        public int PersonDependentId { get; private set; }
    }
}
