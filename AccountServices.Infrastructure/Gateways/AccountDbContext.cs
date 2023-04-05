using AccountServices.Gateways.Entities;
using AccountServices.UseCases.ValueTypes;
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
                    v => v.ToString(),
                    v => (EmailAddress)v!
                );
            modelBuilder.Entity<Account>()
                .HasKey(p => p.EmailAddress);
        }

    }
}
