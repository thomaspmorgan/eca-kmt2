using ECA.Business.Models.Programs;
using ECA.Business.Validation;
using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Programs
{
    public class ProgramServiceValidationEntity
    {
        public ProgramServiceValidationEntity(List<int> regionLocationTypeIds, Focus focus, Organization owner, int? parentProgramId, Program parentProgram)
        {
            this.RegionLocationTypeIds = regionLocationTypeIds;
            this.Focus = focus;
            this.OwnerOrganization = owner;
            this.ParentProgramId = parentProgramId;
            this.ParentProgram = parentProgram;
        }

        public List<int> RegionLocationTypeIds { get; private set; }

        public Focus Focus { get; set; }

        public Organization OwnerOrganization { get; private set; }

        public Program ParentProgram { get; private set; }

        public int? ParentProgramId { get; private set; }
    }

    public class ProgramServiceValidator : BusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity>
    {   

        private IEnumerable<BusinessValidationResult> Validate(ProgramServiceValidationEntity validationEntity)
        {
            if (validationEntity.RegionLocationTypeIds.Count > 1)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.RegionIds, "The given locations are not all regions.");
            }
            if (validationEntity.RegionLocationTypeIds.Count == 1 && validationEntity.RegionLocationTypeIds.First() != LocationType.Region.Id)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.RegionIds, "The given location is not a region.");
            }
            if (validationEntity.Focus == null)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.FocusId, "The focus does not exist.");
            }
            if (validationEntity.OwnerOrganization == null)
            {
                yield return new BusinessValidationResult<EcaProgram>(x => x.OwnerOrganizationId, "The organization does not exist.");
            }
            if (validationEntity.ParentProgramId.HasValue)
            {
                if (validationEntity.ParentProgram == null)
                {
                    yield return new BusinessValidationResult<EcaProgram>(x => x.ParentProgramId, "The parent program does not exist.");
                }
            }
        }

        public override IEnumerable<BusinessValidationResult> DoValidateCreate(ProgramServiceValidationEntity validationEntity)
        {
            return Validate(validationEntity);
        }

        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(ProgramServiceValidationEntity validationEntity)
        {
            return Validate(validationEntity);
        }
    }
}
