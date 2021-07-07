using System.Threading;
using System.Threading.Tasks;
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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
        bool EnsureCreated();
    }
}