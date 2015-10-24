using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Business model for a new participant organization
    /// </summary>
    public class ParticipantOrganization : NewOrganization
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="projectId">The project id</param>
        /// <param name="name">The organization name</param>
        /// <param name="description">The organization description</param>
        /// <param name="organizationRoles">The organization roles</param>
        /// <param name="website">The organization website</param>
        /// <param name="pointsOfContact">The points of contact</param>
        public ParticipantOrganization(User user, int projectId, int participantTypeId, string name, string description, int organizationType, List<int> organizationRoles,
                                       string website, List<int> pointsOfContact) :
                                           base(user, name, description, organizationType, organizationRoles, website, pointsOfContact)
        {
            this.ProjectId = projectId;
            this.ParticipantTypeId = participantTypeId;
        }

        /// <summary>
        /// Gets or sets the project id
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets or sets the participant type id
        /// </summary>
        public int ParticipantTypeId { get; private set; }
    }
}
