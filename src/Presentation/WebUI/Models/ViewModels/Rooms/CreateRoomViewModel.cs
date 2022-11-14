using System;

namespace WebUI.Models.ViewModels.Rooms
{
    public class CreateRoomViewModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }

        public string Description { get; set; }

        public string AddressLine { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }

        public string CategoryId { get; set; }
        public string RoomTypeId { get; set; }
        public string[] AmenitieIds { get; set; }
        public string[] RequirementIds { get; set; }
        public string[] MediaUrls { get; set; }

    }
}
