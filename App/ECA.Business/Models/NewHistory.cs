using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Models
{
    public class NewHistory
    {
        public NewHistory(int creatorUserId)
        {
            this.CreatedAndRevisedOn = DateTimeOffset.UtcNow;
            this.CreatorUserId = creatorUserId;
        }

        public int CreatorUserId { get; private set; }

        public DateTimeOffset CreatedAndRevisedOn { get; private set; }

        public History AsHistory()
        {
            return new History
            {
                CreatedBy = CreatorUserId,
                CreatedOn = CreatedAndRevisedOn,
                RevisedBy = CreatorUserId,
                RevisedOn = CreatedAndRevisedOn
            };
        }
    }
}
