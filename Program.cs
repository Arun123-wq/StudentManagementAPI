using StudentManagementAPI.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Create the SQLite database and Students table automatically on first run.
DatabaseInitializer.Initialize(app.Configuration, app.Logger);

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
