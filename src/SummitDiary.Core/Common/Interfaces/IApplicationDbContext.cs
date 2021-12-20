using Microsoft.EntityFrameworkCore;
using SummitDiary.Core.Common.Models;

namespace SummitDiary.Core.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Summit> Summits { get; set; }
        DbSet<Region> Regions { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<Attachment> Attachments { get; set; }
        DbSet<OsmData> OsmData { get; set; }
        DbSet<WishlistItem> WishlistItems { get;set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
        bool EnsureCreated();
    }
}