using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Common;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Extentions;

namespace Persistence
{
    public class MainDbContext : DbContext, IDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        public MainDbContext(
            DbContextOptions<MainDbContext> options) : base(options)
        {

        }


        public MainDbContext(
        ICurrentUserService currentUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        DbContextOptions<MainDbContext> options
        ) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
            _mediator = mediator;
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
        public DbSet<Owner> Owners { get; set; }


        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<ManagerClaim> ManagerClaims { get; set; }

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await this.SaveChangesAsync(cancellationToken);

            return true;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                entry.Entity.UpdatedAt = _dateTimeService.Now;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedAt = _dateTimeService.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.Deleted = true;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await this.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyGlobalQueryFilter<Entity>(x => !x.Deleted);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }
    }
}
