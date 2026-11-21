-- Reference SQL Server schema from Experiment 10.
-- Optional: use this script if you want to recreate the original SQL Server database.
IF DB_ID('StudentAPI') IS NULL
    CREATE DATABASE StudentAPI;
GO

USE StudentAPI;
GO

IF OBJECT_ID('dbo.Students', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Course NVARCHAR(100) NOT NULL,
        CreatedDate DATETIME DEFAULT GETUTCDATE(),
        UpdatedDate DATETIME DEFAULT GETUTCDATE()
    );

    CREATE NONCLUSTERED INDEX IX_Students_Email ON dbo.Students(Email);

    INSERT INTO dbo.Students (Name, Email, Course) VALUES
    ('John Doe', 'john.doe@example.com', 'Computer Science'),
    ('Jane Smith', 'jane.smith@example.com', 'Software Engineering'),
    ('Mike Johnson', 'mike.johnson@example.com', 'Data Science'),
    ('Sarah Williams', 'sarah.williams@example.com', 'Web Development');
END
GO
