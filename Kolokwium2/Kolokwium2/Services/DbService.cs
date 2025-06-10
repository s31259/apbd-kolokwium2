using Kolokwium2.Data;
using Kolokwium2.DTOs;
using Kolokwium2.Exceptions;
using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _dbContext;

    public DbService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RacerDto> GetRacerAsync(int racerId)
    {
        var racerDto = await _dbContext.Racers.Select(r => new RacerDto
        {
            RacerId = r.RacerId,
            FirstName = r.FirstName,
            LastName = r.LastName,
            Participations = r.RaceParticipations.Select(rp => new ParticipationDto
            {
                Race = new RaceDto()
                {
                    Name = rp.TrackRace.Race.Name,
                    Location = rp.TrackRace.Race.Location,
                    Date = rp.TrackRace.Race.Date
                },
                Track = new TrackDto()
                {
                    Name = rp.TrackRace.Track.Name,
                    LengthInKm = rp.TrackRace.Track.LengthInKm
                },
                Laps = rp.TrackRace.Laps,
                FinishTimeInSeconde = rp.FinishTimeInSeconds,
                Position = rp.Position
            }).ToList()
        }).FirstOrDefaultAsync(r => r.RacerId == racerId);
        
        if (racerDto is null)
        {
            throw new NotFoundException($"Racer with given ID - {racerId} doesn't exist");
        }

        return racerDto;
    }

    public async Task AddParticipationsAsync(AddParticipationsDto requestDto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            if (!await _dbContext.Races.AnyAsync(r => r.Name.Equals(requestDto.RaceName)))
            {
                throw new NotFoundException($"Race with given name - {requestDto.RaceName} doesn't exist");
            }
            
            if (!await _dbContext.Tracks.AnyAsync(t => t.Name.Equals(requestDto.TrackName)))
            {
                throw new NotFoundException($"Track with given name - {requestDto.TrackName} doesn't exist");
            }
            
            foreach (var participationToAddDto in requestDto.Participations)
            {
                if (!await _dbContext.Racers.AnyAsync(r => r.RacerId == participationToAddDto.RacerId))
                {
                    throw new NotFoundException($"Racer with given ID - {participationToAddDto.RacerId} doesn't exist");
                }
                
            }
            
            
            var race = await _dbContext.Races.FirstAsync(r => r.Name.Equals(requestDto.RaceName));
            var track = await _dbContext.Tracks.FirstAsync(r => r.Name.Equals(requestDto.TrackName));
            
            var trackRace = new TrackRace
            {
                Track = track,
                Race = race,
                Laps = 10,
                BestTimeInSeconds = null
            };
            
            
            foreach (var participationToAddDto in requestDto.Participations)
            {
                var racer = await _dbContext.Racers.FirstAsync(r => r.RacerId == participationToAddDto.RacerId);

                var raceParticipation = new RaceParticipation
                {
                    Racer = racer,
                    TrackRace = trackRace,
                    Position = participationToAddDto.Position,
                    FinishTimeInSeconds = participationToAddDto.FinishTimeInSeconds
                };

                if (participationToAddDto.FinishTimeInSeconds < trackRace.BestTimeInSeconds)
                {
                    trackRace.BestTimeInSeconds = participationToAddDto.FinishTimeInSeconds;
                }
                
                _dbContext.RaceParticipations.Add(raceParticipation);
            }
            
            _dbContext.TrackRaces.Add(trackRace);
            
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}