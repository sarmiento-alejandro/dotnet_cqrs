using Microsoft.EntityFrameworkCore;
using CQRS_Implementation.Domain.Entities;

namespace CQRS_Implementation.Infrastructure.Data.MariaDb;

public class MariaDbContext(DbContextOptions<MariaDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MariaDbContext).Assembly);
    }
}