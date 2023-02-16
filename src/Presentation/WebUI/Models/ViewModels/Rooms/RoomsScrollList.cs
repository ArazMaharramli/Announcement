using WebUI.Areas.Admin.ViewModels;

namespace WebUI.Models.ViewModels.Rooms;

public class RoomsScrollList : InfinityScrollModel
{
    public string CategoryId { get; set; }
    public string Query { get; set; }
    public double? Lng { get; set; }
    public double? Lat { get; set; }
}
