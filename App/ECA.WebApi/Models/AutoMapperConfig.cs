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
            Mapper.CreateMap<Data.Program, Program>().MaxDepth(2);
            Mapper.CreateMap<Data.Project, Project>().MaxDepth(1);
            Mapper.CreateMap<Data.Theme, Theme>();
            Mapper.CreateMap<Data.Organization, Organization>()
                .MaxDepth(2)
                .ForMember(o => o.OwnerPrograms, opts => opts.Ignore());
            Mapper.CreateMap<Data.OrganizationType, OrganizationType>(); 
        }
    }
}