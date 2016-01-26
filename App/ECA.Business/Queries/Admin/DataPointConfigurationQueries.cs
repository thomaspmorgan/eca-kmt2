using ECA.Business.Queries.Models.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Admin
{
    public static class DataPointConfigurationQueries
    {
        public static IQueryable<DataPointConfigurationDTO> CreateGetDataPointConfigurations(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var dataPointConfigurations = (from cp in context.DataPointCategoryProperties
                                           join c in context.DataPointConfigurations on cp.DataPointCategoryPropertyId equals c.DataPointCategoryPropertyId
                                           select new DataPointConfigurationDTO
                                           {
                                               DataPointConfigurationId = c.DataPointConfigurationId,
                                               OfficeId = c.OfficeId,
                                               ProgramId = c.ProgramId,
                                               ProjectId = c.ProjectId,
                                               CategoryPropertyId = cp.DataPointCategoryPropertyId,
                                               CategoryId = cp.DataPointCategoryId,
                                               CategoryName = cp.DataPointCategory.DataPointCategoryName,
                                               PropertyId = cp.DataPointPropertyId,
                                               PropertyName = cp.DataPointProperty.DataPointPropertyName,
                                               IsRequired = true
                                           });
            return dataPointConfigurations;
        }
    }
}
