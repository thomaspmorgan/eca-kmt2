using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.Logging;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Person service
    /// </summary>
    public class PersonService : DbContextService<EcaContext>, IPersonService
    {
        private static readonly string COMPONENT_NAME = typeof(PersonService).FullName;
        private readonly ILogger logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="logger">The logger to use</param>
        public PersonService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        /// <summary>
        /// Returns personally identifiable information for a user 
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public PiiDTO GetPiiById(int personId)
        {
            var stopwatch = Stopwatch.StartNew();
            var pii = PersonQueries.CreateGetPiiByIdQuery(this.Context, personId).SingleOrDefault();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "personId", personId } });
            return pii;
        }

        /// <summary>
        /// Returns personally identifiable information for a user asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Personally identifiable information for person</returns>
        public Task<PiiDTO> GetPiiByIdAsync(int personId)
        {
            var stopwatch = Stopwatch.StartNew();
            var pii = PersonQueries.CreateGetPiiByIdQuery(this.Context, personId).SingleOrDefaultAsync();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "personId", personId } });
            return pii;
        }

        /// <summary>
        /// Returns contact info related to a person
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Contact info related to person</returns>
        public ContactInfoDTO GetContactInfoById(int personId)
        {
            var stopwatch = Stopwatch.StartNew();
            var contactInfo = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, personId).SingleOrDefault();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "personId", personId } });
            return contactInfo;
        }

        /// <summary>
        /// Returns contact info related to a person asyncronously
        /// </summary>
        /// <param name="personId">The person id to lookup</param>
        /// <returns>Contact info related to person</returns>
        public Task<ContactInfoDTO> GetContactInfoByIdAsync(int personId)
        {
            var stopwatch = Stopwatch.StartNew();
            var contactInfo = PersonQueries.CreateGetContactInfoByIdQuery(this.Context, personId).SingleOrDefaultAsync();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "personId", personId } });
            return contactInfo;
        }
    }
}
