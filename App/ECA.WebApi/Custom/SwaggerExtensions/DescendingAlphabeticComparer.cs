using System.Collections.Generic;

namespace Swashbuckle.Dummy.SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public class DescendingAlphabeticComparer : IComparer<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Compare(string x, string y)
        {
            return y.CompareTo(x);
        }
    }
}
