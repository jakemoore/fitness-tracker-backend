using FitnessTracker.Models;
using FitnessTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FitnessTracker.Controllers;

[Authorize] // Requires a valid Firebase JWT
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
        var user = HttpContext.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User not found.");
        }

        var workouts = await _repo.GetWorkouts(userId);
        return Ok(workouts);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Workout workout)
    {
        if (string.IsNullOrEmpty(workout.Name)) return BadRequest("Workout name is required.");

        var user = HttpContext.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User not found.");
        }

        await _repo.AddWorkout(userId, workout);
        return Ok(new { Message = "Workout added!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkout(int id)
    {
        var user = HttpContext.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User not found.");
        }

        await _repo.DeleteWorkout(userId, id);
        return Ok(new { Message = "Workout deleted!" });
    }
}