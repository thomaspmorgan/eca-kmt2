using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq
{
    public class QueryableOperator<T> where T : class
    {
        public QueryableOperator(int start, int limit, ICollection<IFilter> filters = null, ICollection<ISorter> sorters = null)
        {
            if (start < 0)
            {
                throw new ArgumentException("The start value must be at least 0.");
            }
            if (limit < 0)
            {
                throw new ArgumentException("The limit value must be at least 0.");
            }
            if (filters == null)
            {
                filters = new List<IFilter>();
            }
            if (sorters == null)
            {
                sorters = new List<ISorter>();
            }
        }

        public int Start { get; set; }

        public int Limit { get; set; }

    }
}
