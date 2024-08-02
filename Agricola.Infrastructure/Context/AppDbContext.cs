using AgricolaApi.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace AgricolaApi.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Farm> Farms { get; set; }
    public DbSet<Lot> Lots { get; set; }
    public DbSet<Group> Groups { get; set; }
}