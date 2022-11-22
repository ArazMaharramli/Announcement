using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.Common.Models
{
    public static class SystemClaims
    {
        public static class Roles
        {
            public const string Show = "Role.Show";
            public const string Create = "Role.Create";
            public const string Edit = "Role.Edit";
            public const string Delete = "Role.Delete";
        }

        public static class Managers
        {
            public const string Show = "Manager.Show";
            public const string Create = "Manager.Create";
            public const string Edit = "Manager.Edit";
            public const string Delete = "Manager.Delete";

            [Display(Name = "Can Create Tickets")]
            public const string CanCreateTicket = "Managers.CanCreateTicket";
            [Display(Name = "Can Handle Tickets")]
            public const string CanBeAssignedToTickets = "Managers.CanHandleTickets";
        }

        public static class Notifications
        {
            public const string OwnerCreated = "Notification.OwnerCreated";
            public const string RoomCreated = "Nofitication.RoomCreated";
            public const string RoomUpdated = "Nofitication.RoomUpdated";
            public const string RoomExpired = "Nofitication.RoomExpired";
        }

        public static class ContactUsInquiries
        {
            public const string Show = "ContactUsInquiry.Show";
            public const string Create = "ContactUsInquiry.Create";
            public const string Edit = "ContactUsInquiry.Edit";
            public const string Delete = "ContactUsInquiry.Delete";
        }

        public static List<SystemClaimsVM> GetSystemClaims()
        {
            var nestedTypes = typeof(SystemClaims)
                .GetNestedTypes()
                .ToList();

            return nestedTypes.Select(x => new SystemClaimsVM
            {
                GroupName = x.Name,
                Claims = x.GetFields().Select(y => new ClaimVM
                {
                    Key = y.GetCustomAttributes(typeof(DisplayAttribute), true)?.Select(z => ((DisplayAttribute)z).Name).FirstOrDefault() ?? y.Name,
                    Value = y.GetValue(null).ToString()
                }).ToList()
            }).ToList();

        }
    }

    public class SystemClaimsVM
    {
        public string GroupName { get; set; }
        public List<ClaimVM> Claims { get; set; }
    }

    public class ClaimVM
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
