using Microsoft.Data.Sqlite;

namespace StudentManagementAPI.Database;

public static class DatabaseInitializer
{
    public static void Initialize(IConfiguration configuration, ILogger logger)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is missing in appsettings.json.");

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Students (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL COLLATE NOCASE UNIQUE,
                Course TEXT NOT NULL,
                CreatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX IF NOT EXISTS IX_Students_Email ON Students(Email);
            """;
        command.ExecuteNonQuery();

        command.CommandText = """
            INSERT OR IGNORE INTO Students (Name, Email, Course) VALUES
            ('John Doe', 'john.doe@example.com', 'Computer Science'),
            ('Jane Smith', 'jane.smith@example.com', 'Software Engineering'),
            ('Mike Johnson', 'mike.johnson@example.com', 'Data Science'),
            ('Sarah Williams', 'sarah.williams@example.com', 'Web Development');
            """;
        command.ExecuteNonQuery();

        logger.LogInformation("Student database initialized successfully.");
    }
}
