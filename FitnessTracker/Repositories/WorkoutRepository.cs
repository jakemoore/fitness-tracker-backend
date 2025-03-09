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

    public async Task<int> AddWorkout(Workout workout)
    {
        using var db = Connection;
        string sql = "INSERT INTO Workouts (UserId, Name, Sets, Reps) VALUES (@UserId, @Name, @Sets, @Reps)";
        return await db.ExecuteAsync(sql, workout);
    }

    public async Task<int> DeleteWorkout(string userId, int id)
    {
        using var db = Connection;
        string sql = "DELETE FROM Workouts WHERE Id = @Id AND UserId = @UserId";
        return await db.ExecuteAsync(sql, new { Id = id, UserId = userId });
    }

    public async Task<int> AddWorkoutLog(WorkoutLog log)
    {
        var sql = @"
            INSERT INTO WorkoutLogs (UserId, WorkoutId, DateCompleted, Sets, Reps, Weight, Notes)
            VALUES (@UserId, @WorkoutId, @DateCompleted, @Sets, @Reps, @Weight, @Notes)
            RETURNING Id;";

        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(sql, log); // Returns the new log's Id
    }

    public async Task<WorkoutLog?> GetWorkoutLogById(string userId, int id)
    {
        var sql = @"SELECT * FROM WorkoutLogs WHERE Id = @Id AND UserId = @UserId";
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<WorkoutLog>(sql, new { Id = id, UserId = userId });
    }

    public async Task<IEnumerable<WorkoutLog>> GetWorkoutHistory(string userId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        var query = "SELECT * FROM WorkoutLogs WHERE UserId = @UserId ORDER BY DateCompleted DESC";
        
        return await connection.QueryAsync<WorkoutLog>(query, new { UserId = userId });
    }
}
