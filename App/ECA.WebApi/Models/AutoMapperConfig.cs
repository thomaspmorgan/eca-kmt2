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
            Mapper.CreateMap<Program, ProgramDTO>()
                .MaxDepth(2)
                .ForMember(dest => dest.RevisedOn, opts => opts.MapFrom(p => p.History.RevisedOn));
            Mapper.CreateMap<ProgramDTO, Program>()
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Regions.FirstOrDefault().LocationName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status));
            Mapper.CreateMap<ProjectDTO, Project>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Organization, OrganizationDTO>()
                .MaxDepth(2);
            Mapper.CreateMap<OrganizationDTO, Organization>()
                .IgnoreAllNonExisting();
            Mapper.CreateMap<OrganizationType, OrganizationTypeDTO>().ReverseMap();
            Mapper.CreateMap<Goal, GoalDTO>().ReverseMap();
            Mapper.CreateMap<Location, RegionDTO>();
            Mapper.CreateMap<RegionDTO, Location>()
                .IgnoreAllNonExisting();
            Mapper.CreateMap<History, HistoryDTO>().ReverseMap();
            Mapper.CreateMap<Contact, ContactDTO>().ReverseMap();
            Mapper.CreateMap<Theme, ThemeDTO>().ReverseMap();
            Mapper.AssertConfigurationIsValid();
        }

        public static IMappingExpression<TSource, TDestination>
            IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps = Mapper.GetAllTypeMaps().First(x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
    }
}