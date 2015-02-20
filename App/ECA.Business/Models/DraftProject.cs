using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models
{
    public class DraftProject
    {
        public DraftProject(string name, string description, int creatorUserId)
        {
            Contract.Requires(name != null, "The name must not be null.");
            this.Name = name;
            this.StatusId = ProjectStatus.Draft.Id;
            this.Description = description;
            this.History = new NewHistory(creatorUserId);
        }

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
