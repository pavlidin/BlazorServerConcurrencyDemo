using BlazorDbContextScopeDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorDbContextScopeDemo.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
}
