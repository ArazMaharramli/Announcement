using System;
using System.Collections.Generic;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerCreated
{
    public class ManagerCreatedIntegrationEvent : IntegrationEvent, INotification
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        /// <summary>
        /// Create user when manager created but Identity service does not have this user
        /// </summary>
        public bool CreateUser { get; set; }

        public List<string> RoleNames { get; set; }

        /// <summary>
        /// Create new Event Instance
        /// </summary>
        /// <param name="id">Manager Id</param>
        /// <param name="name">Name</param>
        /// <param name="email">Email</param>
        /// <param name="phone">Phone number</param>
        /// <param name="createUser">Create user when manager created but Identity service does not have this user</param>
        /// <param name="roleIds">Id's of roles which have to be assigned to user</param>
        /// 
        public ManagerCreatedIntegrationEvent(string id, string name, string email, string phone, bool createUser, List<string> roleNames)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            CreateUser = createUser;
            RoleNames = roleNames;
        }
    }
}

