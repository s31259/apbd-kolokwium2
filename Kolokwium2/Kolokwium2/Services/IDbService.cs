using Kolokwium2.DTOs;

namespace Kolokwium2.Services;

public interface IDbService
{
    Task<RacerDto> GetRacerAsync(int racerId);
    Task AddParticipationsAsync(AddParticipationsDto requestDto);
}