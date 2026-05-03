using BlazorDbContextScopeDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorDbContextScopeDemo.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        dbContext.Users.AddRange(
            new AppUser { Id = 1, Name = "Ada Lovelace", Email = "ada@example.com" },
            new AppUser { Id = 2, Name = "Grace Hopper", Email = "grace@example.com" },
            new AppUser { Id = 3, Name = "Linus Torvalds", Email = "linus@example.com" },
            new AppUser { Id = 4, Name = "Margaret Hamilton", Email = "margaret@example.com" },
            new AppUser { Id = 5, Name = "Donald Knuth", Email = "donald@example.com" },
            new AppUser { Id = 6, Name = "Barbara Liskov", Email = "barbara@example.com" });

        await dbContext.SaveChangesAsync();
    }
}
