using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models.Admin
{
    /// <summary>
    /// A DraftProject is a new project in ECA.
    /// </summary>
    public class DraftProject
    {
        /// <summary>
        /// Creates a new DraftProject with the project's name, description and creator.
        /// </summary>
        /// <param name="name">The name of the project.</param>
        /// <param name="programId">The program id.</param>
        /// <param name="description">The description of the project.</param>
        /// <param name="creatorUserId">The creator user id.</param>
        public DraftProject(string name, string description, int programId, int creatorUserId)
        {
            this.Name = name;
            this.ProgramId = programId;
            this.StatusId = ProjectStatus.Draft.Id;
            this.Description = description;
            this.History = new NewHistory(creatorUserId);
        }

        /// <summary>
        /// Gets the program id.
        /// </summary>
        public int ProgramId { get; private set; }

        /// <summary>
        /// Gets the name of the new project.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description of the project.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the status id of the draft project.
        /// </summary>
        public int StatusId { get; private set; }

        /// <summary>
        /// Gets the history of the draft project.
        /// </summary>
        public NewHistory History { get; private set; }        
    }
}
