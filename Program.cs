using ConcurrencyApp1.Components;
using ConcurrencyApp1.Data;
using ConcurrencyApp1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var databasePath = Path.Combine(builder.Environment.ContentRootPath, "concurrency-demo.db");
var connectionString = $"Data Source={databasePath}";

builder.Services.AddSingleton<DelayCommandInterceptor>();
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    options.UseSqlite(connectionString);
    options.AddInterceptors(serviceProvider.GetRequiredService<DelayCommandInterceptor>());
});
builder.Services.AddDbContextFactory<AppDbContext>((serviceProvider, options) =>
{
    options.UseSqlite(connectionString);
    options.AddInterceptors(serviceProvider.GetRequiredService<DelayCommandInterceptor>());
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IScopedUserService, ScopedUserService>();
builder.Services.AddScoped<IFactoryUserService, FactoryUserService>();
builder.Services.AddScoped<ISerializedScopedUserService, SerializedScopedUserService>();

var app = builder.Build();

await DbInitializer.InitializeAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
