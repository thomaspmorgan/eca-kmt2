using ECA.Core.DynamicLinq;
using System.Data.Entity;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ECA.WebApi.Controllers
{
    public class SampleController : ApiController
    {

        public async Task<PagedQueryResults<SimpleProgramDTO>> GetProgramsAsync(int start, int limit, string filter = null, string sort = null)
        {
            //[{property: 'Id', value: 3, comparison: 'equal'}]

            //http://localhost:5555/api/Sample?start=0&limit=10
            //http://localhost:5555/api/Sample?start=0&limit=10&filter=%5b%7bproperty%3a+'description'%2c+value%3a+'allows+foreign+secondary'%2c+comparison%3a+'like'%7d%5d
            using (var context = new EcaContext())
            {
                //var query = from p in context.Programs
                //            select new SimpleProgramDTO
                //            {
                //                Description = p.Description,
                //                Id = p.ProgramId,
                //                StartDate = p.StartDate
                //            };
                var query = context.Programs.Select(p => new SimpleProgramDTO
                {
                    Description = p.Description,
                    Id = p.ProgramId,
                    StartDate = p.StartDate
                });

                var queryableOperator = new QueryableOperator<SimpleProgramDTO>(
                    start, 
                    limit, 
                    new ExpressionSorter<SimpleProgramDTO>(x => x.Id, SortDirection.Ascending),
                    ParseFilters(filter).ToList<IFilter>(), 
                    ParserSorters(sort).ToList<ISorter>());
                query = query.Apply(queryableOperator);
                return new PagedQueryResults<SimpleProgramDTO>
                {
                    Results = await query.Skip(queryableOperator.Start).Take(queryableOperator.Limit).ToListAsync(),
                    Total = await query.CountAsync()

                };
            }
        }

        private IList<SimpleFilter> ParseFilters(string filter)
        {
            if (filter == null)
            {
                return new List<SimpleFilter>();
            }
            else
            {
                return JsonConvert.DeserializeObject<List<SimpleFilter>>(filter);
            }
            
        }

        private List<SimpleSorter> ParserSorters(string sort)
        {
            if (sort == null)
            {
                return new List<SimpleSorter>();
            }
            else
            {
                return JsonConvert.DeserializeObject<List<SimpleSorter>>(sort);
            }
            
        }
    }

    public class SimpleProgramDTO
    {
        public string Description { get; set; }

        public int Id { get; set; }

        public DateTimeOffset StartDate { get; set; }
    }

    public class PagedQueryResults<T>
    {
        public PagedQueryResults()
        {
            this.Results = new List<T>();
        }

        public int Total { get; set; }

        public IList<T> Results { get; set; }
    }
}
