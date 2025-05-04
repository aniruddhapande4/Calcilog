using CalciLog.Models;
using Microsoft.EntityFrameworkCore;

namespace CalciLog.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Consumer> Consumers { get; set; }
    public DbSet<CalculationRecord> CalculationRecords { get; set; }
}
