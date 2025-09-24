using VideoStreaming.Entities;
using VideoStreaming.Entities.Identity;
using VideoStreaming.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoStreaming.Common.Enums;

namespace VideoStreaming.Data;

public class VideoStreamingDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public virtual DbSet<Meet> Meets { get; set; }
    public virtual DbSet<MeetMember> MeetMembers { get; set; }

    public VideoStreamingDbContext(DbContextOptions<VideoStreamingDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(u => u.Claims)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.Logins)
                .WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.Tokens)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasIndex(u => u.Email).IsUnique();
        });
    }

    public override int SaveChanges()
    {
        PopulateEntityFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        PopulateEntityFields();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public async Task SeedData(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        Role[] roles =
        {
            new() { Name = UserRoleConstants.Administrator },
            new() { Name = UserRoleConstants.Streamer },
            new() { Name = UserRoleConstants.Viewer },
        };

        bool adminRoleExists = await roleManager.RoleExistsAsync(UserRoleConstants.Administrator);

        if (!adminRoleExists)
        {
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private void PopulateEntityFields()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                var entity = (IEntity)entityEntry.Entity;
                if (entity.CreatedAt == default)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                ((IEntity)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}
