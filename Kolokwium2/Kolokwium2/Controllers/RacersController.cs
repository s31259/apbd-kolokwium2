using Kolokwium2.DTOs;
using Kolokwium2.Exceptions;
using Kolokwium2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium2.Controllers;

[ApiController]
[Route("api")]
public class RacersController : ControllerBase
{
    private readonly IDbService _dbService;

    public RacersController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpGet("racers/{racerId}/participations")]
    public async Task<IActionResult> GetRacerAsync(int racerId)
    {
        try
        {
            var racerDto = await _dbService.GetRacerAsync(racerId);
            return Ok(racerDto);
        }
        catch (NotFoundException nfe)
        {
            return NotFound(nfe.Message);
        }
    }
    
    [HttpPost("track-races/participants")]
    public async Task<IActionResult> AddParticipationsAsync(AddParticipationsDto requestDto)
    {
        try
        {
            await _dbService.AddParticipationsAsync(requestDto);
            return Created();
        }
        catch (NotFoundException nfe)
        {
            return NotFound(nfe.Message);
        }
    }
}