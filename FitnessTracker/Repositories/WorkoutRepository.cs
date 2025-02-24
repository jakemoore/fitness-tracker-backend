using System.Data;
using Dapper;
using FitnessTracker.Models;
using Npgsql;

namespace FitnessTracker.Repositories;

public class WorkoutRepository
{
    private readonly string _connectionString;

    public WorkoutRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    public async Task<IEnumerable<Workout>> GetWorkouts()
    {
        using var db = Connection;
        return await db.QueryAsync<Workout>("SELECT * FROM Workouts ORDER BY CreatedAt DESC");
    }

    public async Task<int> AddWorkout(Workout workout)
    {
        using var db = Connection;
        return await db.ExecuteAsync(
            "INSERT INTO Workouts (Name, Sets, Reps) VALUES (@Name, @Sets, @Reps)",
            workout);
    }

    public async Task<int> DeleteWorkout(int id)
    {
        using var db = Connection;
        return await db.ExecuteAsync("DELETE FROM Workouts WHERE Id = @Id", new { Id = id });
    }
}