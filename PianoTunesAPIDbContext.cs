using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using PianoTunesAPI.Models;

public class PianoTunesAPIDbContext : DbContext
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Song>().HasData(new Song[]
        {
            new Song {Id = 1, Title = "Looking Good", ArtistId = 5, Album = "Forever", Length = 3.01M },
            new Song {Id = 2, Title = "Could Not Ask For More", ArtistId = 2, Album = "Games", Length = 3.11M },
            new Song {Id = 3, Title = "In Your Eyes", ArtistId = 1, Album = "Hero", Length = 4.01M },
            new Song {Id = 4, Title = "I'll Be", ArtistId = 2, Album = "Games", Length = 5.01M },
            new Song {Id = 5, Title = "I Will Always Love You", ArtistId = 4, Album = "SoulMates", Length = 3.31M }
            });

        modelBuilder.Entity<Artist>().HasData(new Artist[]
        {
            new Artist {Id = 1, Name = "Joey Ebach", Age = 31, Bio = "Young, handsome man who loves to write a good love song." },
            new Artist {Id = 2, Name = "Edwin McCain", Age = 52, Bio = "Wrote many classic love songs, which have been played at many weddings."},
            new Artist {Id = 3, Name = "Chris Tomlin", Age = 53, Bio = "Christian artist who writes inspiring music about God."},
            new Artist {Id = 4, Name = "Whitney Houston", Age = 52, Bio = "She has a beautiful voice and loves singing upbeat tunes."},
            new Artist {Id = 5, Name = "Mariah Carey", Age = 55, Bio = "She can hold her pitch like no one else!"}
            });

        modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
            new Genre {Id = 1, Description = "Soul"},
            new Genre {Id = 2, Description = "Pop"},
            new Genre {Id = 3, Description = "Rock"},
            new Genre {Id = 5, Description = "Country"}
            });
    }

    public PianoTunesAPIDbContext(DbContextOptions<PianoTunesAPIDbContext> context) : base(context)
    {

    }
    
};