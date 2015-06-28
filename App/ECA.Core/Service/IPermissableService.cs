using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Service
{
    public interface IPermissableService
    {
        void OnAdded(IList<IPermissable> addedEntities);

        Task OnAddedAsync(IList<IPermissable> addedEntities);

        void OnUpdated(IList<IPermissable> updatedEntities);

        Task OnUpdatedAsync(IList<IPermissable> updatedEntities);
    }
}
