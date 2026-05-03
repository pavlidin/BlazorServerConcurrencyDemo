using BlazorDbContextScopeDemo.Components;
using BlazorDbContextScopeDemo.Data;
using BlazorDbContextScopeDemo.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var databasePath = Path.Combine(builder.Environment.ContentRootPath, "dbcontext-scope-demo.db");
var connectionString = $"Data Source={databasePath}";

builder.Services.AddSingleton<QueryDelayInterceptor>();
AddDemoDataAccess(builder.Services, connectionString);

builder.Services.AddScoped<IScopedUserService, ScopedUserService>();
builder.Services.AddScoped<IFactoryUserService, FactoryUserService>();
builder.Services.AddScoped<ISerializedScopedUserService, SerializedScopedUserService>();

var app = builder.Build();

await DbInitializer.InitializeAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();

static void AddDemoDataAccess(IServiceCollection services, string connectionString)
{
    services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        ConfigureDataAccess(options, serviceProvider, connectionString));

    services.AddDbContextFactory<AppDbContext>((serviceProvider, options) =>
        ConfigureDataAccess(options, serviceProvider, connectionString), ServiceLifetime.Scoped);
}

static void ConfigureDataAccess(
    DbContextOptionsBuilder options,
    IServiceProvider serviceProvider,
    string connectionString)
{
    options.UseSqlite(connectionString);
    options.AddInterceptors(serviceProvider.GetRequiredService<QueryDelayInterceptor>());
}

