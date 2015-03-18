using AutoMapper;
using ECA.Business.Service;
using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
    public class RowVersionStringResolver : ValueResolver<IConcurrent, string>
    {
        protected override string ResolveCore(IConcurrent source)
        {
            Contract.Assert(source != null, "The source must not be null.");
            if (source.RowVersion == null)
            {
                return null;
            }
            else
            {
                return Convert.ToBase64String(source.RowVersion);
            }
        }
    }
}