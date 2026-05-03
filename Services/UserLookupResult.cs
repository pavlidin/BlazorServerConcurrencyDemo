namespace ConcurrencyApp1.Services;

public sealed record UserLookupResult(
    int UserId,
    string? Name,
    string? Email,
    string ServiceInstanceId,
    string DbContextInstanceId);
