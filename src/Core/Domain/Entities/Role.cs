using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Events.Roles;

namespace Domain.Entities
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public ICollection<RoleClaim> Claims { get; set; }
        public ICollection<Manager> Managers { get; set; }

        public Role()
        {
            Claims = new HashSet<RoleClaim>();
        }

        public Role(string name, List<string> claims) : this()
        {
            Name = name.Trim();
            Claims = claims.Select(x => new RoleClaim(x.Trim())).ToList();

            AddDomainEvent(new RoleCreatedDomainEvent(this));
        }

        public void UpdateClaims(List<string> claims)
        {
            Claims.Clear();
            Claims = claims.Select(x => new RoleClaim(x)).ToList();
        }

        public void Update(string name, List<string> claims)
        {
            Name = name.Trim();
            UpdateClaims(claims);

            AddDomainEvent(new RoleUpdatedDomainEvent(this));
        }
    }
}
