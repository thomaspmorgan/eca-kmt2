using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Service
{
    public class Enums
    {
        public enum ResourceType {
            Application = 1,
            Office = 2,
            Program = 3,
            Project = 4,
            Participant = 5,
            MoneyFlow = 6,
            Artifact = 7
        }

        public enum AccountStatus
        {
            Active = 1,
            Suspended = 2,
            Revoked = 3,
            Expired = 4
        }
    }
}
