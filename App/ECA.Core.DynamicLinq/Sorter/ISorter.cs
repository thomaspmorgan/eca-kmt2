using System.Diagnostics.Contracts;

namespace ECA.Core.DynamicLinq.Sorter
{
    /// <summary>
    /// An ISorter is a class that can be converted to sorters used in queries.
    /// </summary>
    [ContractClass(typeof(ISorterContract))]
    public interface ISorter
    {
        /// <summary>
        /// Returns a linq sorter from this ISorter.
        /// </summary>
        /// <typeparam name="T">The type to sort.</typeparam>
        /// <returns>The linq sorter.</returns>
        LinqSorter<T> ToLinqSorter<T>() where T : class;
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISorter))]
    public abstract class ISorterContract : ISorter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public LinqSorter<T> ToLinqSorter<T>() where T : class
        {
            Contract.Ensures(Contract.Result<LinqSorter<T>>() != null, "The ToLinqSorter method must return a non null value.");
            return null;
        }
    }
}
