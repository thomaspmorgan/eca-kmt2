﻿using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Implementation of gender service
    /// </summary>
    public class GenderService : LookupService<SimpleLookupDTO>, IGenderService
    {
        private static readonly string COMPONENT_NAME = typeof(GenderService).FullName;
        private readonly ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="logger">The logger to use</param>
        public GenderService(EcaContext context, ILogger logger) 
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        /// <summary>
        /// Implementation of lookup service method 
        /// </summary>
        /// <returns>List of genders</returns>
        protected override IQueryable<SimpleLookupDTO> GetSelectDTOQuery()
        {
            var query = this.Context.Genders.Select(x => new SimpleLookupDTO
            {
                Id = x.GenderId,
                Value = x.GenderName
            });
            return query;
        }
    }
}