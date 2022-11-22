using System;
using Domain.Common;

namespace Domain.Entities
{
    public class RoleClaim : Entity
    {
        public string RoleId { get; set; }
        public Role Role { get; set; }

        public string ClaimName { get; set; }

        public RoleClaim()
        {

        }

        public RoleClaim(string name) : this()
        {
            ClaimName = name.Trim();
        }
    }
}
