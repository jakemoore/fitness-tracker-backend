
using System.Data;
using Dapper;
using FitnessTracker.DTOs;
using Npgsql;

namespace FitnessTracker.Repositories;

public class DashboardRepository
{
    private readonly string _connectionString;

    public DashboardRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<DashboardStatsDto> GetDashboardStats(string userId)
    {
        using var db = CreateConnection();
        
        // Query for workouts this week
        string weeklyCountSql = @"
            SELECT COUNT(*) 
            FROM WorkoutLogs 
            WHERE UserId = @UserId AND DateCompleted >= @WeekStart AND DateCompleted < @WeekEnd";
        
        DateTime weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        DateTime weekEnd = weekStart.AddDays(7);
        
        var thisWeekCount = await db.ExecuteScalarAsync<int>(weeklyCountSql, new 
        {
            UserId = userId,
            WeekStart = weekStart,
            WeekEnd = weekEnd
        });

        // Query for current streak
        string currentStreakSql = @"
            WITH WorkoutDates AS (
                SELECT DISTINCT 
                    DATE(DateCompleted) AS WorkoutDate
                FROM WorkoutLogs
                WHERE UserId = @UserId
            ),
            DatesWithGroups AS (
                SELECT 
                    WorkoutDate,
                    WorkoutDate - INTERVAL '1 day' * ROW_NUMBER() OVER (ORDER BY WorkoutDate) AS StreakGroup
                FROM WorkoutDates
            ),
            MostRecentDate AS (
                SELECT MAX(WorkoutDate) AS LatestWorkoutDate
                FROM WorkoutDates
            ),
            CurrentStreakCheck AS (
                SELECT 
                    m.LatestWorkoutDate,
                    CASE 
                        -- If latest workout was yesterday or today, streak is active
                        WHEN m.LatestWorkoutDate >= CURRENT_DATE - INTERVAL '1 day' THEN 
                            (SELECT StreakGroup FROM DatesWithGroups WHERE WorkoutDate = m.LatestWorkoutDate)
                        -- Otherwise streak is broken
                        ELSE NULL
                    END AS ActiveStreakGroup
                FROM MostRecentDate m
            )
            SELECT 
                CASE 
                    WHEN c.ActiveStreakGroup IS NULL THEN 0
                    ELSE (SELECT COUNT(*) FROM DatesWithGroups WHERE StreakGroup = c.ActiveStreakGroup)
                END AS CurrentStreak
            FROM CurrentStreakCheck c;";

        var currentStreak = await db.ExecuteScalarAsync<int>(currentStreakSql, new { UserId = userId });
        
        // Query for most frequent workout(s) for this user
        string mostFrequentWorkoutSql = @"
            WITH workout_counts AS (
                SELECT WorkoutId, COUNT(*) as log_count
                FROM WorkoutLogs
                WHERE UserId = @UserId
                GROUP BY WorkoutId
            ),
            max_count AS (
                SELECT MAX(log_count) as max_log_count
                FROM workout_counts
            )
            SELECT w.Name
            FROM Workouts w
            JOIN workout_counts wc ON w.Id = wc.WorkoutId
            JOIN max_count mc ON wc.log_count = mc.max_log_count;";
        
        var mostFrequentWorkouts = await db.QueryAsync<string>(mostFrequentWorkoutSql, new { UserId = userId });
        
        // Join multiple workout names with commas if there are ties
        string mostFrequentWorkoutNames = mostFrequentWorkouts.Any() 
            ? string.Join(", ", mostFrequentWorkouts) 
            : "N/A";
        
        return new DashboardStatsDto
        {
            WorkoutsThisWeek = thisWeekCount,
            CurrentStreak = currentStreak,
            MostFrequentWorkout = mostFrequentWorkoutNames
        };
    }
}
