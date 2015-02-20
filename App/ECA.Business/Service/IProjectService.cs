using ECA.Business.Models;
using System;
namespace ECA.Business.Service
{
    public interface IProjectService : IDisposable
    {
        ECA.Data.Project Create(DraftProject project);
    }
}
