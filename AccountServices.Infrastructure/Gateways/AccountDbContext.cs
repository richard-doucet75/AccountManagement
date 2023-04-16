using AccountServices.Gateways.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountServices.Infrastructure.Gateways
{
    public sealed class AccountDbContext : DbContext
    { 
        public AccountDbContext(
            DbContextOptions<AccountDbContext> options) 
            : base(options) 
        {
        }
        public DbSet<Account> Accounts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(p => p.EmailAddress)
                .HasConversion(
                    ea => ea.ToString(),
                    ea => ea
                );

            modelBuilder.Entity<Account>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");
        }

    }
}
