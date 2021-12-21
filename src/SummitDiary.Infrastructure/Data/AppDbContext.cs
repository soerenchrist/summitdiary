using Ardalis.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Models.ActivityAggregate;
using SummitDiary.Core.Models.SummitAggregate;

namespace SummitDiary.Infrastructure.Data;
public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Summit> Summits { get; set; } = default!;
    public DbSet<Region> Regions { get; set; } = default!;
    public DbSet<Country> Countries { get; set; } = default!;
    public DbSet<Activity> Activities { get; set; } = default!;
    public DbSet<Attachment> Attachments { get; set; } = default!;
    public DbSet<OsmData> OsmData { get; set; } = default!;
    public DbSet<WishlistItem> WishlistItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }

    public bool EnsureCreated()
    {
        return Database.EnsureCreated();
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}
