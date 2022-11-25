using System;
using Domain.Common;

namespace Domain.Entities
{
    public class ManagerClaim : Entity
    {

        public string ManagerId { get; set; }
        public Manager Manager { get; set; }

        public string ClaimName { get; set; }

        public ManagerClaim()
        {

        }

        public ManagerClaim(string managerId, string claimName) : this()
        {
            ManagerId = managerId;
            ClaimName = claimName;
        }

    }
}
