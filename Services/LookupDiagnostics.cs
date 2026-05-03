namespace BlazorDbContextScopeDemo.Services;

internal static class LookupDiagnostics
{
    public static string CreateInstanceId() => Guid.NewGuid().ToString("N")[..8];

    public static string FormatContextId(Guid contextId) => contextId.ToString("N")[..8];
}
