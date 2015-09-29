using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An UpdatedPersonMembership is used by a business layer client to update a membership.
    /// </summary>
    public class UpdatedPersonMembership
    {
        /// <summary>
        /// Creates a new UpdatedPersonMembership and initialized it with the given values.
        /// </summary>
        /// <param name="updator">The updator.</param>
        /// <param name="id">The id of the membership.</param>
        /// <param name="name">The value.</param>
        public UpdatedPersonMembership(User updator, int id, string name)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Update = new Update(updator);
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Update Update { get; private set; }

        /// <summary>
        /// Gets the Id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the Name value.
        /// </summary>
        public string Name { get; private set; }
    }
}
