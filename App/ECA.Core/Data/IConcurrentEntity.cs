using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Data
{
    /// <summary>
    /// An IConcurrentEntity is a database entity that maintains concurrency.
    /// </summary>
    public interface IConcurrentEntity : IConcurrent, IIdentifiable
    {
        
    }
}
