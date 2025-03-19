namespace FitnessTracker.DTOs
{
    public class WorkoutLogDetailDto
    {
        public int Id { get; set; }
        public string WorkoutName { get; set; } = string.Empty;
        public DateTime DateCompleted { get; set; } = DateTime.UtcNow;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public decimal? Weight { get; set; }
        public string? Notes { get; set; }
    }
}
