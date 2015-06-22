using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Data
{
    public interface IResource
    {
        int GetId();

        int GetResourceTypeId();

        int GetParentId();

        int GetParentResourceTypeId();
    }
}
