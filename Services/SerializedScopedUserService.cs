using ConcurrencyApp1.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace ConcurrencyApp1.Services;

public sealed class SerializedScopedUserService(AppDbContext dbContext) : ISerializedScopedUserService, IDisposable
{
    private static readonly TimeSpan AlignmentDelay = TimeSpan.FromMilliseconds(150);
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly string _serviceInstanceId = LookupDiagnostics.CreateInstanceId();

    public async Task<UserLookupResult> FindUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        await Task.Delay(AlignmentDelay, cancellationToken);
        await _gate.WaitAsync(cancellationToken);

        try
        {
            var user = await dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);

            return new UserLookupResult(
                userId,
                user?.Name,
                user?.Email,
                _serviceInstanceId,
                LookupDiagnostics.FormatContextId(dbContext.ContextId.InstanceId));
        }
        finally
        {
            _gate.Release();
        }
    }

    public void Dispose()
    {
        _gate.Dispose();
    }
}
