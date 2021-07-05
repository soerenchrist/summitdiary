using System.Linq;
using Bogus;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Core.Common.Models;

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
        }
    }
}