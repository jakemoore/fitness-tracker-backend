using FitnessTracker.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
);
builder.Services.AddSingleton<WorkoutRepository>();

var app = builder.Build();

// Configure middleware
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
