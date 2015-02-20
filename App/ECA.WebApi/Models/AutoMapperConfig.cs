﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECA.Data;
using AutoMapper;
using System.Text;

namespace ECA.WebApi.Models
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Program, ProgramDTO>()
                .ForMember(dest => dest.RevisedOn, opts => opts.MapFrom(p => p.History.RevisedOn))
                .MaxDepth(2);
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
            Mapper.CreateMap<MoneyFlow, MoneyFlowDTO>()
                .ForMember(dest => dest.MoneyFlowStatus, opt => opt.MapFrom(p => p.MoneyFlowStatus.MoneyFlowStatusName))
                .ForMember(dest => dest.MoneyFlowType, opt => opt.MapFrom(p => p.MoneyFlowType.MoneyFlowTypeName))
                .ForMember(dest => dest.SourceType, opt => opt.MapFrom(p => p.SourceType.TypeName))
                .ForMember(dest => dest.RecipientType, opt => opt.MapFrom(p => p.RecipientType.TypeName))
                .ForMember(dest => dest.RecipientName, opt => opt.ResolveUsing<RecipientNameResolver>())
                .ForMember(dest => dest.SourceName, opt => opt.ResolveUsing<SourceNameResolver>());
            Mapper.CreateMap<Participant, ParticipantDTO>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(p => p.PersonId != null ? FullName(p.Person.Names) : p.Organization.Name))
                .ForMember(dest => dest.Gender, opts => opts.MapFrom(p => p.PersonId != null ? p.Person.Gender.ToString() : "N/A"))
                .ForMember(dest => dest.Status, opts => opts.MapFrom(p => p.PersonId != null ? "Active" : p.Organization.Status));
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

        public class RecipientNameResolver : ValueResolver<MoneyFlow, string>
        {
            protected override string ResolveCore(MoneyFlow source)
            {
                string name = "";

                if (source.RecipientProgramId != null)
                    name = source.RecipientProgram.Name;
                else if (source.RecipientProjectId != null)
                    name = source.RecipientProject.Name;
                else if (source.RecipientTransportationId != null)
                    name = source.RecipientTransportation.Carrier.Name;
                else if (source.RecipientItineraryStopId != null)
                    name = source.RecipientItineraryStopId.ToString(); // Need a name for the ItineraryStop
                else if (source.RecipientParticipantId != null)
                {
                    if (source.RecipientParticipant.OrganizationId != null)
                        name = source.RecipientParticipant.Organization.Name;
                    else if (source.RecipientParticipant.PersonId != null)
                        name = FullName(source.RecipientParticipant.Person.Names);
                }
                else if (source.RecipientTransportationId != null)
                    name = source.RecipientTransportation.Carrier.Name;
                else if (source.RecipientAccommodationId != null)
                    name = source.RecipientAccommodation.Host.Name;

                return name;
            }
        }

        public class SourceNameResolver : ValueResolver<MoneyFlow, string>
        {
            protected override string ResolveCore(MoneyFlow source)
            {
                string name = "";

                if (source.SourceProgramId != null)
                    name = source.RecipientProgram.Name;
                else if (source.SourceProjectId != null)
                    name = source.RecipientProject.Name;
                else if (source.SourceItineraryStopId != null)
                    name = source.SourceItineraryStopId.ToString(); // Need a name for the ItineraryStop
                else if (source.SourceParticipantId != null)
                {
                    if (source.SourceParticipant.OrganizationId != null)
                        name = source.SourceParticipant.Organization.Name;
                    else if (source.SourceParticipant.PersonId != null)
                        name = FullName(source.SourceParticipant.Person.Names);
                }
                return name;
            }
        }

        private static string FullName(ICollection<NamePart> Names)
        {
            StringBuilder fullName = new StringBuilder();
            NamePart namePart = Names.FirstOrDefault(p => p.NameType == NameType.GivenName);
            if (namePart != null)
                fullName.Append(namePart.Value);
            namePart = Names.FirstOrDefault(p => p.NameType == NameType.Alias);
            if (namePart != null)
                fullName.Append(" '" + namePart.Value + "'");
            namePart = Names.FirstOrDefault(p => p.NameType == NameType.MiddleName);
            if (namePart != null)
                fullName.Append(" " + namePart.Value);
            namePart = Names.FirstOrDefault(p => p.NameType == NameType.FamilyName);
            if (namePart != null)
                fullName.Append(" " + namePart.Value);
            namePart = Names.FirstOrDefault(p => p.NameType == NameType.Patronym);
            if (namePart != null)
                fullName.Append(" " + namePart.Value);
            return fullName.ToString();
        }

    }
}