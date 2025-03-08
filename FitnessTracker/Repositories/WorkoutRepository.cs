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

    public async Task<IEnumerable<Workout>> GetWorkouts(string userId)
    {
        using var db = Connection;
        string sql = "SELECT * FROM Workouts WHERE UserId = @UserId";
        return await db.QueryAsync<Workout>(sql, new { UserId = userId });
    }

    public async Task<int> AddWorkout(string userId, Workout workout)
    {
        using var db = Connection;
        string sql = "INSERT INTO Workouts (UserId, Name, Sets, Reps) VALUES (@UserId, @Name, @Sets, @Reps)";
        return await db.ExecuteAsync(sql, new { UserId = userId, Name = workout.Name, Sets = workout.Sets, Reps = workout.Reps });
    }

    public async Task<int> DeleteWorkout(string userId, int id)
    {
        using var db = Connection;
        string sql = "DELETE FROM Workouts WHERE Id = @Id AND UserId = @UserId";
        return await db.ExecuteAsync(sql, new { Id = id, UserId = userId });
    }
}
