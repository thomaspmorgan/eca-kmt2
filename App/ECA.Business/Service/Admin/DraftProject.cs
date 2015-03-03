using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
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
        /// <param name="createdBy">The user who created the draft project.</param>
        public DraftProject(User createdBy, string name, string description, int programId)
        {
            // remember to fix the unit test for this constructor too
            throw new NotImplementedException();
            //this.Name = name;
            //this.ProgramId = programId;
            //this.StatusId = ProjectStatus.Draft.Id;
            //this.Description = description;
            //this.History = new CreatedHistory(createdBy);
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
        //public CreatedHistory History { get; private set; }        
    }
}
