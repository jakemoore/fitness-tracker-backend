public class WorkoutLog
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public int WorkoutId { get; set; }
    public DateTime DateCompleted { get; set; } = DateTime.UtcNow;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal? Weight { get; set; }
    public string? Notes { get; set; }
}
