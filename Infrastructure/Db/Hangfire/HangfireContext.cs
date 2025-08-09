using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Account_Service.Infrastructure.Db.Hangfire
{
    /// <inheritdoc />
    public sealed class HangfireContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly string ConnectionString;

        /// <inheritdoc />
        public HangfireContext(string connectionString)
        {
            ConnectionString = connectionString;

            Database.Migrate();
        }

        /// <inheritdoc />
        public HangfireContext(IOptions<HangfireDbSettings> options)
        {
            var hangfireDbSettings = options.Value;
            ConnectionString = hangfireDbSettings.ConnectionString;

            Database.Migrate();
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}