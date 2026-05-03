using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlazorDbContextScopeDemo.Data;

public sealed class QueryDelayInterceptor : DbCommandInterceptor
{
    private static readonly TimeSpan QueryDelay = TimeSpan.FromMilliseconds(500);

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        await Task.Delay(QueryDelay, cancellationToken);
        return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }
}
