# Blazor DbContext Scope Demo

Small Blazor Server sample that shows why a circuit-shared scoped `DbContext` can fail under parallel component startup, plus three safe alternatives.

## Run

```bash
dotnet run --project BlazorDbContextScopeDemo.csproj
```

## Pages

- Shared scoped context: reproduces the failure.
- Factory-created context: uses `IDbContextFactory<TContext>`.
- Serialized access: queues work through a gate.
- Component-owned scope: gives each card its own DI scope.
