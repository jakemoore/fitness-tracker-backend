using FitnessTracker.Models;
using FitnessTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FitnessTracker.DTOs;

namespace FitnessTracker.Controllers;

[Authorize] // Requires a valid Firebase JWT
[ApiController]
[Route("api/[controller]")]
public class WorkoutsController : ControllerBase
{
    private readonly WorkoutRepository _repo;

    public WorkoutsController(WorkoutRepository repo)
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
    public async Task<IActionResult> Add([FromBody] WorkoutDto workoutDto)
    {
        if (string.IsNullOrEmpty(workoutDto.Name)) return BadRequest("Workout name is required.");

        var user = HttpContext.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("User not found.");
        }

        var workout = new Workout
        {
            UserId = userId,
            Name = workoutDto.Name,
            Sets = workoutDto.Sets,
            Reps = workoutDto.Reps,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddWorkout(workout);
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

    [HttpPost("logs")]
    public async Task<IActionResult> AddWorkoutLog([FromBody] WorkoutLogDto logDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not found.");
        }

        var workoutLog = new WorkoutLog
        {
            UserId = userId,
            WorkoutId = logDto.WorkoutId,
            DateCompleted = logDto.DateCompleted,
            Sets = logDto.Sets,
            Reps = logDto.Reps,
            Weight = logDto.Weight,
            Notes = logDto.Notes
        };

        var logId = await _repo.AddWorkoutLog(workoutLog);
        return CreatedAtAction(nameof(GetWorkoutLogById), new { id = logId }, workoutLog);
    }

    [HttpGet("logs/{id}")]
    public async Task<IActionResult> GetWorkoutLogById(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        var workoutLog = await _repo.GetWorkoutLogById(userId, id);
        if (workoutLog == null)
        {
            return NotFound("Workout log not found.");
        }
        return Ok(workoutLog);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetWorkoutHistory()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var history = await _repo.GetWorkoutHistory(userId);
        return Ok(history);
    }
}