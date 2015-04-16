using CAM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebApi.Test.Security
{
    public class TestCamUser : CAM.Business.Service.User
    {
        public bool IsValid { get; set; }

        public override bool AuthenticateUserWithGuid(Guid userGuid, CamModel cam)
        {
            this.AdGuid = userGuid;
            return IsValid;
        }
    }
}
