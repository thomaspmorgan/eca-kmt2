using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Security
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            this.ResourcePermissions = new List<ResourcePermissionViewModel>();
        }

        public Guid UserId { get; set; }

        public int CamPrincipalId { get; set; }

        public string UserName { get; set; }

        public IEnumerable<ResourcePermissionViewModel> ResourcePermissions { get; set; }
    }
}