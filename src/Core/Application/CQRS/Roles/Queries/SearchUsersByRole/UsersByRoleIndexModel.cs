using System;
namespace Application.CQRS.Roles.Queries.SearchUsersByRole
{
	public class UsersByRoleIndexModel
	{
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

