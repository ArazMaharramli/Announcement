using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Common;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Extentions;

namespace Persistence
{
    public class MainDbContext : DbContext, IDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public MainDbContext(
            DbContextOptions<MainDbContext> options) : base(options)
        {

        }


        public MainDbContext(
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService,
        DbContextOptions<MainDbContext> options) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Amenitie> Amenities { get; set; }
        public DbSet<AmenitieTranslation> AmenitieTranslations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<RequirementTranslation> RequirementTranslations { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedAt = _dateTimeService.Now;
                        entry.Entity.UpdatedAt = _dateTimeService.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.UpdatedAt = _dateTimeService.Now;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.UpdatedAt = _dateTimeService.Now;
                        entry.Entity.Deleted = true;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyGlobalQueryFilter<AuditableEntity>(x => !x.Deleted);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }
    }
}
