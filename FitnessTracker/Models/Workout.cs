namespace FitnessTracker.Models;
public class Workout
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
