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
            Mapper.CreateMap<Program, ProgramDTO>().ReverseMap().MaxDepth(1);
            Mapper.CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Regions.FirstOrDefault().LocationName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status));
            Mapper.CreateMap<ProjectDTO, Project>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Regions, opt => opt.Ignore())
                .ForMember(dest => dest.Themes, opt => opt.Ignore());
            Mapper.CreateMap<Organization, OrganizationDTO>().ReverseMap()
                .MaxDepth(2)
                .ForMember(o => o.OwnerPrograms, opts => opts.Ignore());
            Mapper.CreateMap<OrganizationType, OrganizationTypeDTO>();
            Mapper.CreateMap<Goal, GoalDTO>().ReverseMap();
            Mapper.CreateMap<Location, RegionDTO>().ReverseMap();
            Mapper.CreateMap<History, HistoryDTO>().ReverseMap();
            Mapper.CreateMap<Contact, ContactDTO>();
        }
    }
}