﻿using ECA.Business.Service;
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
            int? parentProgramId,
            int focusId,
            string website,
            List<int> goalIds,
            List<int> pointOfContactIds,
            List<int> themeIds,
            List<int> regionIds)
            : base(
                updatedBy: createdBy,
                id: 0,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                programStatusId: ProgramStatus.Draft.Id,
                focusId: focusId,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds
                )
        {
            this.Audit = new Create(createdBy);
        }


    }
}
