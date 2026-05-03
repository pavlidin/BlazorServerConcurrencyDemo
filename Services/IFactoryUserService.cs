namespace ConcurrencyApp1.Services;

public interface IFactoryUserService
{
    Task<UserLookupResult> FindUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}
