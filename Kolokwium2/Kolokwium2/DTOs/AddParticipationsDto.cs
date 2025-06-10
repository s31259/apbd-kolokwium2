namespace Kolokwium2.DTOs;

public class AddParticipationsDto
{
    public string RaceName { get; set; } = null!;
    public string TrackName { get; set; } = null!;
    public List<ParticipationToAddDto> Participations { get; set; } = null!;
}

public class ParticipationToAddDto
{
    public int RacerId { get; set; }
    public int Position { get; set; }
    public int FinishTimeInSeconds { get; set; }
}