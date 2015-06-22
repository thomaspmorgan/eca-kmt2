using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Data
{
    public enum PermissableType
    {
        Application,
        Project,
        Program,
        Office
    }

    public interface IPermissable
    {
        int GetId();
        PermissableType GetPermissableType();
        int? GetParentId();
        PermissableType GetParentPermissableType();
    }
}
