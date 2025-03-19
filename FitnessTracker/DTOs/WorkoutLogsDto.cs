namespace FitnessTracker.DTOs
{
  public class WorkoutLogsDto
  {
    public IEnumerable<WorkoutLogDetailDto>? WorkoutLogs { get; set; }
    public int TotalLogs { get; set; }
    public int TotalPages { get; set; }
  }
}
