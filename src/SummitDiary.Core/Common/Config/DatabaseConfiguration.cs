using Microsoft.Extensions.Configuration;

namespace SummitDiary.Core.Common.Config
{
    public class DatabaseConfiguration
    {
        private readonly IConfiguration _configuration;

        public DatabaseConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public bool UseSqlite => _configuration.GetValue("Database:UseSqlite", true);
        public bool UsePostgres => _configuration.GetValue("Database:UsePostgres", false);
        public bool UseMySql => _configuration.GetValue("Database:UseMySql", false);

        public string ConnectionString =>
            _configuration.GetValue("Database:ConnectionString", "Data Source=diary.db");
    }
}