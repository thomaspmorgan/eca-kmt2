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
            int ownerOrganizationId,
            int programStatusId,
            string focus,
            string website,
            List<int> goalIds,
            List<int> pointOfContactIds,
            List<int> themeIds)
            : base(
                updatedBy: createdBy,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                programStatusId: programStatusId,
                focus: focus,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds
                )
        {
            this.NewHistory = new CreatedHistory(createdBy);
        }

        public CreatedHistory NewHistory { get; private set; }

    }
}
