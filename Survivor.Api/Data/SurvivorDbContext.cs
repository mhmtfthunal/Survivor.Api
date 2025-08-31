using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Survivor.Api.Entities;

namespace Survivor.Api.Data;

public class SurvivorDbContext : DbContext
{
    public SurvivorDbContext(DbContextOptions<SurvivorDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Competitor> Competitors => Set<Competitor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1 - N ilişki
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Competitors)
            .WithOne(c => c.Category!)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Soft delete global filter
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Competitor>().HasQueryFilter(x => !x.IsDeleted);

        // Seed (ekrandaki örneğe uygun)
        var t = new DateTime(2024, 1, 1, 10, 0, 0);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Ünlüler", CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Category { Id = 2, Name = "Gönüllüler", CreatedDate = t, ModifiedDate = t, IsDeleted = false }
        );

        modelBuilder.Entity<Competitor>().HasData(
            new Competitor { Id = 1, FirstName = "Acun", LastName = "Ilıcalı", CategoryId = 1, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 2, FirstName = "Aleyna", LastName = "Avcı", CategoryId = 1, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 3, FirstName = "Hadise", LastName = "Açıkgöz", CategoryId = 1, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 4, FirstName = "Sertan", LastName = "Bozkuş", CategoryId = 1, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 5, FirstName = "Özge", LastName = "Açık", CategoryId = 1, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 6, FirstName = "Kıvanç", LastName = "Tatlıtuğ", CategoryId = 1, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 7, FirstName = "Ahmet", LastName = "Yılmaz", CategoryId = 2, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 8, FirstName = "Elif", LastName = "Demirtaş", CategoryId = 2, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 9, FirstName = "Cem", LastName = "Öztürk", CategoryId = 2, CreatedDate = t, ModifiedDate = t, IsDeleted = false },
            new Competitor { Id = 10, FirstName = "Ayşe", LastName = "Karaca", CategoryId = 2, CreatedDate = t, ModifiedDate = t, IsDeleted = false }
        );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.ModifiedDate = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
