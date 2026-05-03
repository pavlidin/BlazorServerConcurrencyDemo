namespace BlazorDbContextScopeDemo.Services;

public interface IScopedUserService
{
    Task<UserLookupResult> FindUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}
