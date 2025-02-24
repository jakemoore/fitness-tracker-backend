using FitnessTracker.Models;
using FitnessTracker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutController : ControllerBase
{
    private readonly WorkoutRepository _repo;

    public WorkoutController(WorkoutRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var workouts = await _repo.GetWorkouts();
        return Ok(workouts);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Workout workout)
    {
        if (string.IsNullOrEmpty(workout.Name)) return BadRequest("Workout name is required.");

        await _repo.AddWorkout(workout);
        return Ok(new { Message = "Workout added!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkout(int id)
    {
        await _repo.DeleteWorkout(id);
        return Ok(new { Message = "Workout deleted!" });
    }
}