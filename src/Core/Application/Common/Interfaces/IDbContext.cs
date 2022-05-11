using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Common.Interfaces
{
    public interface IDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<Amenitie> Amenities { get; set; }
        DbSet<AmenitieTranslation> AmenitieTranslations { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        DbSet<Media> Medias { get; set; }
        DbSet<Requirement> Requirements { get; set; }
        DbSet<RequirementTranslation> RequirementTranslations { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Setting> Settings { get; set; }
        DbSet<RoomType> RoomTypes { get; set; }
        DbSet<RoomTypeTranslation> RoomTypeTranslations { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
