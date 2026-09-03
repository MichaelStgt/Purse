// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Data.Sqlite;

namespace Purse.Services;

internal sealed class SqliteHelper
{
    // private const string ConnectionString = "Data Source=:memory:";
    private const string ConnectionString = "Data Source=:memory:";

    private static readonly Lazy<SqliteHelper> singleton = new(() => new SqliteHelper());

    public static SqliteConnection Connection => singleton.Value.SqliteConnection;

    #region SqliteHelper Internals
    private SqliteHelper()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "purse.db");
        var connectionstring = $"Data Source={dbPath}";

        // SqliteConnection = new SqliteConnection(ConnectionString);
        SqliteConnection = new SqliteConnection(connectionstring);
        SqliteConnection.Open();
    }

    private SqliteConnection SqliteConnection
    {
        get;
    }
    #endregion
}
