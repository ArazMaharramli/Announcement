﻿using System.Collections.Generic;
using Application.CQRS.Rooms.Queries.GetActiveRooms;

namespace WebUI.Models.ViewModels.Rooms
{
    public class RoomsIndexViewModel
    {
        public List<RoomBriefVM> Rooms { get; set; }
    }
}
