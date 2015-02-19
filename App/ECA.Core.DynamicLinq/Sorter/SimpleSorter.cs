using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Sorter
{
    public class SimpleSorter : ISorter
    {
        public string Direction { get; set; }

        public string Property { get; set; }

        public LinqSorter<T> ToLinqSorter<T>() where T : class
        {
            var direction = SortDirection.ToSortDirection(this.Direction);
            return new LinqSorter<T>(this.Property, direction);
        }
    }
}
