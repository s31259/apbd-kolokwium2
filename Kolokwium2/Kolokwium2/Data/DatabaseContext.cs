using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Race> Races { get; set; }
    public DbSet<RaceParticipation> RaceParticipations { get; set; }
    public DbSet<Racer> Racers { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<TrackRace> TrackRaces { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Race>().HasData(new List<Race>()
        {
            new Race() {RaceId = 1, Name = "TestRace1", Location = "TestLocation1", Date = DateTime.Parse("2025-06-10")},
            new Race() {RaceId = 2, Name = "TestRace2", Location = "TestLocation2", Date = DateTime.Parse("2025-06-09")},
            new Race() {RaceId = 3, Name = "TestRace3", Location = "TestLocation3", Date = DateTime.Parse("2025-06-08")}
        });
        
        modelBuilder.Entity<Track>().HasData(new List<Track>()
        {
            new Track() {TrackId = 1, Name = "TestTrack1", LengthInKm = 20.5},
            new Track() {TrackId = 2, Name = "TestTrack2", LengthInKm = 10.7},
            new Track() {TrackId = 3, Name = "TestTrack3", LengthInKm = 15.3},
        });
        
        modelBuilder.Entity<TrackRace>().HasData(new List<TrackRace>()
        {
            new TrackRace() {TrackRaceId = 1, TrackId = 1, RaceId = 1, Laps = 4, BestTimeInSeconds = 335},
            new TrackRace() {TrackRaceId = 2, TrackId = 2, RaceId = 2, Laps = 8, BestTimeInSeconds = 270},
            new TrackRace() {TrackRaceId = 3, TrackId = 3, RaceId = 3, Laps = 12, BestTimeInSeconds = null}
        });
        
        modelBuilder.Entity<Racer>().HasData(new List<Racer>()
        {
            new Racer() {RacerId = 1, FirstName = "TestFirstName1", LastName = "TestLastName1"},
            new Racer() {RacerId = 2, FirstName = "TestFirstName2", LastName = "TestLastName2"},
            new Racer() {RacerId = 3, FirstName = "TestFirstName3", LastName = "TestLastName3"}
        });
        
        modelBuilder.Entity<RaceParticipation>().HasData(new List<RaceParticipation>()
        {
            new RaceParticipation() {TrackRaceId = 1, RacerId = 1, FinishTimeInSeconds = 578, Position = 6},
            new RaceParticipation() {TrackRaceId = 2, RacerId = 2, FinishTimeInSeconds = 642, Position = 4},
            new RaceParticipation() {TrackRaceId = 3, RacerId = 3, FinishTimeInSeconds = 523, Position = 3}
        });
    }
}