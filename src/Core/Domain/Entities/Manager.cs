﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;

namespace Domain.Entities;

public class Manager : Entity
{
    public string Name { get; set; }

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

    public Manager(string name, string email, string phone) : this()
    {
        Name = name.Trim();
        Email = email.Trim();
        Phone = phone.Trim();
    }

    public void AddRoleRange(List<Role> roles)
    {
        foreach (var role in roles)
        {
            Roles.Add(role);
        }
    }

    public void AddRole(Role role)
    {
        Roles.Add(role);
    }


    public void UpdateRoles(List<Role> roles)
    {
        Roles.Clear();
        AddRoleRange(roles);
    }

    public void ClearRoles()
    {
        Roles.Clear();
    }

    public void UpdateClaims(List<string> claims)
    {
        Claims.Clear();
        foreach (var claim in claims)
        {
            Claims.Add(new ManagerClaim(Id, claim));
        }
    }

    public void ClearClaims()
    {
        Claims.Clear();
    }

    public void Recover()
    {
        Deleted = false;
    }
}
