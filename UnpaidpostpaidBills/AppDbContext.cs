using Microsoft.EntityFrameworkCore;
using UnpaidpostpaidBills;

public class AppDbContext : DbContext
{
    public DbSet<FacturationEncaissement> Bill { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurations supplémentaires si nécessaires

        // Configuration de la classe Bill sans clé primaire explicite
        modelBuilder.Entity<FacturationEncaissement>().HasNoKey().ToView("FACTURATION_ENCAISSEMENT");
    }
}
