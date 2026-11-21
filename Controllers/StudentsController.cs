using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly string _connectionString;

    public StudentsController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is missing.");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetAll()
    {
        var students = new List<Student>();
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Email, Course FROM Students ORDER BY Id;";
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            students.Add(ReadStudent(reader));

        return Ok(students);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Student>> GetById(int id)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Email, Course FROM Students WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return NotFound(new { message = $"Student with ID {id} was not found." });

        return Ok(ReadStudent(reader));
    }

    [HttpPost]
    public async Task<ActionResult<Student>> Create(Student student)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Students (Name, Email, Course)
            VALUES ($name, $email, $course);
            SELECT last_insert_rowid();
            """;
        AddStudentParameters(command, student);

        try
        {
            var id = Convert.ToInt32(await command.ExecuteScalarAsync());
            student.Id = id;
            return CreatedAtAction(nameof(GetById), new { id }, student);
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
        {
            return Conflict(new { message = "A student with this email already exists." });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Student student)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Students
            SET Name = $name, Email = $email, Course = $course,
                UpdatedDate = CURRENT_TIMESTAMP
            WHERE Id = $id;
            """;
        AddStudentParameters(command, student);
        command.Parameters.AddWithValue("$id", id);

        try
        {
            var affected = await command.ExecuteNonQueryAsync();
            if (affected == 0)
                return NotFound(new { message = $"Student with ID {id} was not found." });

            return NoContent();
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
        {
            return Conflict(new { message = "A student with this email already exists." });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Students WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);

        var affected = await command.ExecuteNonQueryAsync();
        if (affected == 0)
            return NotFound(new { message = $"Student with ID {id} was not found." });

        return NoContent();
    }

    private static Student ReadStudent(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt32(0),
        Name = reader.GetString(1),
        Email = reader.GetString(2),
        Course = reader.GetString(3)
    };

    private static void AddStudentParameters(SqliteCommand command, Student student)
    {
        command.Parameters.AddWithValue("$name", student.Name);
        command.Parameters.AddWithValue("$email", student.Email);
        command.Parameters.AddWithValue("$course", student.Course);
    }
}
