using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Business class for updated office
    /// </summary>
    public class UpdatedOffice
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="officeId">The office id</param>
        /// <param name="name">The name</param>
        /// <param name="officeSymbol">The office symbol</param>
        /// <param name="description">The description</param>
        /// <param name="parentOfficeId">The parent office id</param>
        /// <param name="pointsOfContactIds">The point of contact ids</param>
        public UpdatedOffice(User user, int officeId, string name, string officeSymbol, string description, int? parentOfficeId, IEnumerable<int> pointsOfContactIds)
        {
            this.Audit = new Update(user);
            this.OfficeId = officeId;
            this.Name = name;
            this.OfficeSymbol = officeSymbol;
            this.Description = description;
            this.ParentOfficeId = parentOfficeId;
            this.PointsOfContactIds = pointsOfContactIds ?? new List<int>();
        }

        /// <summary>
        /// Gets or sets the audit
        /// </summary>
        public Audit Audit { get; private set; }
        /// <summary>
        /// Gets or sets the office id
        /// </summary>
        public int OfficeId { get; private set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets or sets the office symbol
        /// </summary>
        public string OfficeSymbol { get; private set; }
        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Gets or sets the parent office id
        /// </summary>
        public int? ParentOfficeId { get; private set; }
        /// <summary>
        /// Gets or sets the points of contact ids
        /// </summary>
        public IEnumerable<int> PointsOfContactIds { get; private set; }
    }
}
