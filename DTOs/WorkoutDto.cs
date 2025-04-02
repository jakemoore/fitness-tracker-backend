
namespace FitnessTracker.DTOs
{
    public class WorkoutDto
    {
      public string Name { get; set; } = string.Empty;
      public int Sets { get; set; }
      public int Reps { get; set; }
      public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}