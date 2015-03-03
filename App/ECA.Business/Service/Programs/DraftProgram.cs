using ECA.Business.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models.Programs
{
    public class DraftProgram : EcaProgram
    {
        public DraftProgram(
            User createdBy,
            string name, 
            string description, 
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            int ownerOrganizationId)
            : base(createdBy, name, description, startDate, endDate, ownerOrganizationId, ProgramStatus.Draft.Id)
        {
            this.NewHistory = new CreatedHistory(createdBy);
        }

        public CreatedHistory NewHistory { get; private set; }
        
    }
}
