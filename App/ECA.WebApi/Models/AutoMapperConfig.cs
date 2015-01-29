using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data=ECA.Data;
using AutoMapper;

namespace ECA.WebApi.Models
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Data.Program, ProgramDTO>().MaxDepth(2);
            Mapper.CreateMap<Data.Project, ProjectDTO>().MaxDepth(1);
            Mapper.CreateMap<Data.Theme, ThemeDTO>();
            Mapper.CreateMap<Data.Organization, OrganizationDTO>()
                .MaxDepth(2)
                .ForMember(o => o.OwnerPrograms, opts => opts.Ignore());
            Mapper.CreateMap<Data.OrganizationType, OrganizationTypeDTO>(); 
        }
    }
}