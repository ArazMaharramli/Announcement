using System.Collections.Generic;
using System.Linq;
using Domain.Common;

namespace Domain.Entities;

public class Manager : Entity
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public string Email { get; set; }
    public string Phone { get; set; }

    public ICollection<Role> Roles { get; set; }

    public ICollection<ManagerClaim> Claims { get; set; }

    public bool HasClaim(string claimName)
    {
        return (this.Claims.Where(x => !x.Deleted).Count() == 0 &&
                        this.Roles.Any(y =>
                            y.Claims.Any(z =>
                                z.ClaimName == claimName)
                            )
                        ) ||
                            this.Claims.Any(y => !y.Deleted && y.ClaimName == claimName)
                        ;
    }

    public Manager()
    {
        Claims = new HashSet<ManagerClaim>();
        Roles = new HashSet<Role>();
    }
}
