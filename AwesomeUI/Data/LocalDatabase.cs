using SQLite;

namespace AwesomeUI.Data.Local;

public class LocalDatabase : SQLiteAsyncConnection
{
    private void CreateTables()
    {
        CreateTableAsync<Blog>().Wait();
    }
    public LocalDatabase(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks)
    {
        CreateTables();
    }

    public LocalDatabase(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = true) : base(databasePath, openFlags, storeDateTimeAsTicks)
    {
        CreateTables();
    }

    public LocalDatabase(SQLiteConnectionString connectionString) : base(connectionString)
    {
        CreateTables();
    }
}