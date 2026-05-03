using BlazorDbContextScopeDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorDbContextScopeDemo.Services;

public sealed class ScopedUserService(AppDbContext dbContext) : IScopedUserService
{
    private static readonly TimeSpan AlignmentDelay = TimeSpan.FromMilliseconds(150);
    private readonly string _serviceInstanceId = LookupDiagnostics.CreateInstanceId();

    public async Task<UserLookupResult> FindUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        await Task.Delay(AlignmentDelay, cancellationToken);

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
}
