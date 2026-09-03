// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#undef OFFLINE_SYNC_ENABLED

namespace Purse.Services;

/// <summary>
/// Use this class to initialize the database. In this sample, we just create the database. However, you may want to use
/// migrations.
/// </summary>
/// <param name="context">The context for the database.</param>
public class DbContextInitializer(LocalDbContext context)
    : IDbInitializer
{
    public void Initialize()
    {
        // Todo: Remove this, as we now have this in the LocalDBContext constructor, and we want to avoid having multiple calls to EnsureCreated() or Migrate() in the app, as this can lead to performance issues and other problems. We should have a single point of initialization for the database, and that should be in the LocalDBContext constructor, where we can also handle any necessary setup for the database file path and other configurations. Once we have a working implementation of the database initialization, we can remove this method and any calls to it from the app.
        // _ = context.Database.EnsureCreated();
        // context.Database.Migrate();
        // Task.Run(async () => await context.Database.MigrateAsync());
        // Task.Run(async () => await context.SynchronizeAsync());
        // Task.Run(async () => await context.SynchronizeAsync());
    }

    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        return context.Database.EnsureCreatedAsync(cancellationToken);
    }
}
