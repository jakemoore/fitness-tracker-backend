namespace FitnessTracker.DTOs
{
  public class DashboardStatsDto
  {
      public int WorkoutsThisWeek { get; set; }
      public int CurrentStreak { get; set; }
      public string MostFrequentWorkout { get; set; } = string.Empty;
  }
}
