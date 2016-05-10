using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis
{
    public interface IChangeComparable<TChangeComparable, TChangeDetail>
        where TChangeDetail : ChangeDetail
    {
        TChangeDetail GetChangeDetail(TChangeComparable otherChangeComparable);
    }
}
