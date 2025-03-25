using System.Security.Claims;
using FitnessTracker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize] // Requires a valid Firebase JWT
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly DashboardRepository _repo;

    public DashboardController(DashboardRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User not found.");

        var stats = await _repo.GetDashboardStats(userId);
        return Ok(stats);
    }
}
