using System;
using System.Collections.Generic;

namespace WebUI.Areas.Admin.ViewModels.Roles
{
    public class DetailRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> Claims { get; set; }
        public int UserCount { get; set; }
    }
}

