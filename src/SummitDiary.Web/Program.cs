using MonkeyCache.FileStore;
using SummitDiary.Core.Common.Config;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Web;
using SummitDiary.Web.Attributes;

Barrel.ApplicationId = "summitdiary";
var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(NotFoundExceptionFilterAttribute));
});
builder.Services.AddRazorPages();
            
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
var databaseConfiguration = new DatabaseConfiguration(builder.Configuration);

builder.Services.AddCoreDependencies();
builder.Services.AddInfrastructureDependencies(databaseConfiguration);
builder.Services.AddSpaStaticFiles(config =>
{
    config.RootPath = "ClientApp/dist";
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseRouting();
app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "default",
        "{controller}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";
    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
    }
});

// Populate database
using var scope = app.Services.CreateScope();
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

app.Run();


/*
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
*/