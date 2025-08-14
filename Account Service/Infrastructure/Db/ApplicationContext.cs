using Account_Service.Features.Accounts;
using Account_Service.Features.Transactions;
using Account_Service.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Account_Service.Infrastructure.Db
{
    /// <inheritdoc />
    public sealed class ApplicationContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly string ConnectionString;

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Account> Accounts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <inheritdoc />
        public ApplicationContext(string connectionString)
        {
            ConnectionString = connectionString;

            Database.Migrate();
        }

        /// <inheritdoc />
        public ApplicationContext(IOptions<DbSettings> options)
        {
            var dbSettings = options.Value;
            ConnectionString = dbSettings.ConnectionString;

            Database.Migrate();
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Account>().Property(a => a.Id).HasColumnName("id").IsRequired();
            modelBuilder.Entity<Account>().Property(a => a.OwnerId).HasColumnName("ownerId").IsRequired();
            modelBuilder.Entity<Account>().HasOne(a => a.Owner).WithMany().HasForeignKey(a => a.OwnerId).IsRequired();
            modelBuilder.Entity<Account>().Property(a => a.Type).HasColumnName("type").IsRequired();
            modelBuilder.Entity<Account>().Property(a => a.Currency).HasColumnName("currency").IsRequired();
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasColumnName("balance").IsRequired().HasPrecision(28, 8);
            modelBuilder.Entity<Account>().Property(a => a.InterestRate).HasColumnName("interestRate").HasPrecision(28, 8);
            modelBuilder.Entity<Account>().Property(a => a.OpenDate).HasColumnName("openDate").IsRequired();
            modelBuilder.Entity<Account>().Property(a => a.CloseDate).HasColumnName("closeDate");
            modelBuilder.Entity<Account>().HasMany(a => a.Transactions).WithOne(t => t.Account).HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Transaction>().Property(t => t.Id).HasColumnName("id").IsRequired();
            modelBuilder.Entity<Transaction>().Property(t => t.AccountId).HasColumnName("accountId").IsRequired();
            modelBuilder.Entity<Transaction>().Property(t => t.CounterpartyAccountId).HasColumnName("counterpartyAccountId");
            modelBuilder.Entity<Transaction>().Property(t => t.Sum).HasColumnName("sum").IsRequired().HasPrecision(28, 8);
            modelBuilder.Entity<Transaction>().Property(t => t.Currency).HasColumnName("currency").IsRequired();
            modelBuilder.Entity<Transaction>().Property(t => t.Type).HasColumnName("type").IsRequired();
            modelBuilder.Entity<Transaction>().Property(t => t.Description).HasColumnName("description").IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Transaction>().Property(t => t.DateTime).HasColumnName("date");
            modelBuilder.Entity<Transaction>().HasOne(t => t.CounterpartyAccount).WithMany().HasForeignKey(t => t.CounterpartyAccountId);

            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("id").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
            
            modelBuilder.Entity<Account>().HasIndex(a => a.OwnerId).HasMethod("hash");
            modelBuilder.Entity<Transaction>().HasIndex(t => new { t.AccountId, t.DateTime });
            modelBuilder.Entity<Transaction>().HasIndex(t => t.DateTime).HasMethod("gist");

            // ReSharper disable once StringLiteralTypo
            modelBuilder.Entity<Account>().Property(a => a.RowVersion).HasColumnName("xmin").IsRowVersion();
            // ReSharper disable once StringLiteralTypo
            modelBuilder.Entity<Transaction>().Property(t => t.RowVersion).HasColumnName("xmin").IsRowVersion();
            // ReSharper disable once StringLiteralTypo
            modelBuilder.Entity<User>().Property(u => u.RowVersion).HasColumnName("xmin").IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }
}