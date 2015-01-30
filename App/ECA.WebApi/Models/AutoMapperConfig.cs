using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECA.Data;
using AutoMapper;

namespace ECA.WebApi.Models
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Program, ProgramDTO>().MaxDepth(2).ReverseMap();
            Mapper.CreateMap<Project, ProjectDTO>().MaxDepth(2).ReverseMap(); 
            Mapper.CreateMap<Theme, ThemeDTO>();
            Mapper.CreateMap<Organization, OrganizationDTO>().ReverseMap()
                .MaxDepth(2)
                .ForMember(o => o.OwnerPrograms, opts => opts.Ignore());
            Mapper.CreateMap<OrganizationType, OrganizationTypeDTO>();
            Mapper.CreateMap<Goal, GoalDTO>();
        }
    }
}