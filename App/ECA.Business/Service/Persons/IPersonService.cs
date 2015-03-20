using ECA.Business.Queries.Models.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public interface IPersonService
    {
        PiiDTO GetPiiById(int personId);

        Task<PiiDTO> GetPiiByIdAsync(int personId);
    }
}
