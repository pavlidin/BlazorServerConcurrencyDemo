using ConcurrencyApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyApp1.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
}
