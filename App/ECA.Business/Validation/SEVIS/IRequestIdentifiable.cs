using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    public interface IRequestIdentifiable
    {
        string RequestId { get; set; }
    }
}
