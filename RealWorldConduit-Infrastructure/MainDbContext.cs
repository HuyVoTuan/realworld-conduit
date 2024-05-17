using Microsoft.EntityFrameworkCore;

using RealWorldConduit_Domain.Commons;
using RealWorldConduit_Domain.Entities;

namespace RealWorldConduit_Infrastructure
{
    public class MainDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto").HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AutomateAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AutomateAuditInfo()
        {
            var entries = ChangeTracker.Entries()
                                       .Where(e => e.Entity is IAuditEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                IAuditEntity entity = (IAuditEntity)entry.Entity;

                // Update [ UpdatedDate ] every time perform SaveChangesAsync on an entity
                entity.UpdatedDate = DateTime.UtcNow;

                // If an entity is at newly [ Added State ], update CreatedDate
                // Otherwise can not modify CreatedDate
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                }
                else
                {
                    Entry(entity).Property(e => e.CreatedDate).IsModified = false;
                }
            }
        }
    }
}
