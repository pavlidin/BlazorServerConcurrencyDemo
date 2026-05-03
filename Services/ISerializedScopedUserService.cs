namespace ConcurrencyApp1.Services;

public interface ISerializedScopedUserService
{
    Task<UserLookupResult> FindUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}
