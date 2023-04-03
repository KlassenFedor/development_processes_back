using dev_processes_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = dev_processes_backend.Models.File;

namespace dev_processes_backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<InterviewState> InterviewStates { get; set; }
        public DbSet<Practice> Practices { get; set; }
        public DbSet<PracticeDiaryTemplate> PracticeDiaryTemplates { get; set; }
        public DbSet<PracticeOrder> PracticeOrders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<VacancyPriority> VacancyPriorities { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<File>(e =>
            {
                e.HasOne<Company>()
                    .WithOne(c => c.Logo)
                    .HasForeignKey<Company>(c => c.LogoId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Practice>()
                    .WithOne(c => c.PracticeDiary)
                    .HasForeignKey<Practice>(p => p.PracticeDiaryId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Practice>()
                    .WithOne(c => c.CharacterizationFile)
                    .HasForeignKey<Practice>(p => p.CharacterizationFileId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Interview>().HasQueryFilter(i => !i.Vacancy.IsDeleted);
            
            builder.Entity<Role>().ToTable("Roles");
            
            builder.Entity<User>().ToTable("Users");
            
            builder.Entity<UserRole>(e =>
            {
                e.ToTable("UserRoles");
                e.HasOne(x => x.Role)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.User)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Vacancy>(e =>
            {
                e.HasQueryFilter(v => !v.IsDeleted);
                e.HasOne(v => v.Company)
                    .WithMany(c => c.Vacancies)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            builder.Entity<VacancyPriority>().HasQueryFilter(vp => !vp.Vacancy.IsDeleted);
        }
        
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new())
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected virtual void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    // Soft delete entity
                    case EntityState.Deleted when entry.Entity is ISoftDeletableEntity:
                        entry.State = EntityState.Unchanged;
                        if ((bool)entry.Property(nameof(ISoftDeletableEntity.IsDeleted)).CurrentValue)
                        {
                            break;
                        }
                        entry.Property(nameof(ISoftDeletableEntity.IsDeleted)).CurrentValue = true;
                        break;
                }
            }
        }
    }
}
