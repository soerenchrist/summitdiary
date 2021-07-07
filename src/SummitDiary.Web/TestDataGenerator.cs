using System.Linq;
using Bogus;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;
using SummitDiary.Core.Endpoints.Diary.Dto;

namespace SummitDiary.Web
{
    public static class TestDataGenerator
    {
        public static void GenerateTestData(IApplicationDbContext context)
        {
            if (!context.Summits.Any())
            {
                var testSummits = new Faker<Summit>()
                    .RuleFor(x => x.Name, x => x.Person.FirstName)
                    .RuleFor(x => x.CountryId, x => x.Random.Number(1, 4))
                    .RuleFor(x => x.RegionId, _ => 1)
                    .RuleFor(x => x.Height, x => x.Random.Number(2500, 4500))
                    .RuleFor(x => x.Latitude, x => x.Random.Number(-90, 90))
                    .RuleFor(x => x.Longitude, x => x.Random.Number(-180, 180))
                    .StrictMode(false);

                for (int i = 0; i < 500; i++)
                {
                    var summit = testSummits.Generate();
                    context.Summits.Add(summit);
                }

                context.SaveChangesAsync();
            }

            if (!context.Activities.Any())
            {
                var diaryEntries = new Faker<Activity>()
                    .RuleFor(x => x.Title, x => x.Commerce.ProductName())
                    .RuleFor(x => x.Distance, x => x.Random.Number(0, 10000))
                    .RuleFor(x => x.ElevationUp, x => x.Random.Number(0, 1000))
                    .RuleFor(x => x.ElevationDown, x => x.Random.Number(0, 1000))
                    .RuleFor(x => x.Duration, x => x.Random.Number(0, 10000))
                    .RuleFor(x => x.Rating, x => x.Random.Number(1, 5))
                    .RuleFor(x => x.HikeDate, x => x.Date.Past())
                    .RuleFor(x => x.Notes, x => x.Commerce.ProductDescription())
                    .RuleFor(x => x.Summits, x => new []{context.Summits.Find(x.Random.Long(1,500))});

                for (int i = 0; i < 200; i++)
                {
                    var entry = diaryEntries.Generate();
                    context.Activities.Add(entry);
                }

                context.SaveChangesAsync();
            }
        }
    }
}