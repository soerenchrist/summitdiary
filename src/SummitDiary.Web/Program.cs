using MonkeyCache.FileStore;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Infrastructure.Data;

namespace SummitDiary.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Barrel.ApplicationId = "summitdiary";
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<IApplicationDbContext>();
                context.EnsureCreated();

                await DatabaseSeed.PopulateData(context);
#if DEBUG
                TestDataGenerator.GenerateTestData(context);
#endif
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured while initializing the database");
            }
            
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}