using PianoTunesAPI.Models;
using PianoTunesAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace PianoTunesAPI.Controllers
{
    public class Songs
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("api/songs", (PianoTunesAPIDbContext db) =>
            {
                return db.Songs
                .ToList();
            });

            //get a single song with the artist and genres
            app.MapGet("api/songs/{songId}", (PianoTunesAPIDbContext db, int songId) =>
            {
                Song singleSong = db.Songs
                .Include(s => s.Artist)
                .Include(s => s.Genres)
                .FirstOrDefault(s => s.Id == songId);

                if (singleSong == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleSong);
            });

            //delete a single song
            app.MapDelete("api/songs/delete/{songId}", (PianoTunesAPIDbContext db, int songId) =>
            {
                Song deleteSong = db.Songs.FirstOrDefault(s => s.Id == songId);

                try
                {
                    db.Songs.Remove(deleteSong);
                    db.SaveChanges();
                    return Results.NoContent();
                }
                catch (DbUpdateException)
                {
                    return Results.NoContent();
                }
            });

            //update a song
            app.MapPut("/api/songs/update/{songId}", (PianoTunesAPIDbContext db, int songId, SongDto dto) =>
            {
                Song song = db.Songs.FirstOrDefault(s => s.Id == songId);

                if (song == null)
                {
                    return Results.BadRequest("Song not found!");
                }
             
                song.Title = dto.Title;
                song.Album = dto.Album;
                song.Length = dto.Length;
                db.SaveChanges();
                return Results.Ok(song);
            });

            //create new song
            app.MapPost("/api/songs/new", (PianoTunesAPIDbContext db, SongDto dto) =>
            {
                // Assuming dto includes ArtistId
                Song newSong = new Song { Title = dto.Title, Album = dto.Album, Length = dto.Length, ArtistId = dto.ArtistId };

                // Ensure the Genres collection is initialized
                newSong.Genres = new List<Genre>();

                foreach (int genreId in dto.GenreId)
                {
                    Genre addGenre = db.Genres.SingleOrDefault(g => g.Id == genreId);
                    if (addGenre != null)
                    {
                        // Instead of adding the genre directly, you should associate it with the new song
                        newSong.Genres.Add(addGenre);
                    }
                    else
                    {
                        return Results.NoContent();
                    }
                }

                db.Songs.Add(newSong);
                db.SaveChanges();

                return Results.Created($"/api/songs/new/{newSong.Id}", newSong);
            });


        }
    }
    
}
