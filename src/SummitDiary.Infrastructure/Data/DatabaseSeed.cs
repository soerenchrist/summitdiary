using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Services;

namespace SummitDiary.Infrastructure.Data
{
    public static class DatabaseSeed
    {
        private static readonly List<Country> Countries = new()
        {
            new Country
            {
                Name = "Deutschland"
            },
            new Country
            {
                Name = "Österreich"
            },
            new Country
            {
                Name = "Schweiz"
            },
            new Country
            {
                Name = "Frankreich"
            },
            new Country
            {
                Name = "Italien"
            }
        };

        private static readonly List<Region> Regions = new()
        {
            new Region
            {
                Name = "Alpen"
            }
        };

        public static async Task PopulateData(IApplicationDbContext context)
        {
            if (!context.Countries.Any())
            {
                context.Countries.AddRange(Countries);
                await context.SaveChangesAsync();
            }

            if (!context.Regions.Any())
            {
                context.Regions.AddRange(Regions);
                await context.SaveChangesAsync();
            }

            if (!context.Summits.Any())
            {
                var scraper = new SummitScraper();
                var summits = await scraper.FetchSummits();
                await context.Summits.AddRangeAsync(summits);
                await context.SaveChangesAsync();
            }
        }
    }
}