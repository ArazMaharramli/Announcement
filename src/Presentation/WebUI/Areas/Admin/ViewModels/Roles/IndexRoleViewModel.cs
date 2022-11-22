using System;
using System.Collections.Generic;
using Application.Common.Models;

namespace WebUI.Areas.Admin.ViewModels.Roles
{
    public class IndexRoleViewModel
    {
        public IndexRoleViewModel()
        {
            Roles = new List<RoleDto>();
        }

        public List<RoleDto> Roles { get; set; }
    }
}

