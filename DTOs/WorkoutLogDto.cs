namespace FitnessTracker.DTOs
{
    public class WorkoutLogDto
    {
        public int WorkoutId { get; set; }
        public DateTime DateCompleted { get; set; } = DateTime.UtcNow;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal? Weight { get; set; }
        public string? Notes { get; set; }
    }
}