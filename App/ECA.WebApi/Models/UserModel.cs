using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models
{
        public class UserModel
        {
            public string GroupName { get; set; }
            public string MissionCodes { get; set; }
            public string JobTitle { get; set; }
            public string AlternateEmail { get; set; }
        }
}