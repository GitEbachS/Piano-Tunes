using PianoTunesAPI.Models;
using PianoTunesAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace PianoTunesAPI.Controllers
{
    public class Genres
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/api/genres", (PianoTunesAPIDbContext db) =>
            {
                return db.Genres
                .ToList();
            });

            //get the single genre with the songs
            app.MapGet("/api/genres/{genreId}", (PianoTunesAPIDbContext db, int genreId) =>
            {
                Genre singleGenre = db.Genres
                .Include(g => g.Songs)
                .FirstOrDefault(s => s.Id == genreId);

                if (singleGenre == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleGenre);
            });

            app.MapDelete("/api/deleteGenre/{genreId}", (PianoTunesAPIDbContext db, int genreId) =>
                {
                    Genre deleteGenre = db.Genres.SingleOrDefault(g => g.Id == genreId);
                    try
                    {
                        db.Genres.Remove(deleteGenre);
                        db.SaveChanges();
                        return Results.NoContent();
                    }
                    catch (DbUpdateException)
                    {
                        return Results.NoContent();
                    }

                });

            app.MapPut("/api/genres/update/{genreId}", (PianoTunesAPIDbContext db, int genreId, GenreDto dto) =>
            {
                Genre genre = db.Genres.SingleOrDefault(s => s.Id == genreId);

                if (genre == null)
                {
                    return Results.BadRequest("Genre not found!");
                }

                genre.Description = dto.Description;
                db.SaveChanges();
                return Results.Ok(genre);
            });

            //add a new genre
            app.MapPost("/api/genres/new", (PianoTunesAPIDbContext db, GenreDto dto) =>
            {
                Genre newGenre = new() { Description = dto.Description };
                db.Genres.Add(newGenre);
                db.SaveChanges();
                return Results.Created($"/api/genres/new/{newGenre.Id}", newGenre);
            });

            //add a genre to a song
            app.MapPost("/api/song/addGenre", (PianoTunesAPIDbContext db, GenreSongDto genreSong) =>
            {
                var singleSongToUpdate = db.Songs
                .Include(s => s.Genres)
                .FirstOrDefault(s => s.Id == genreSong.SongId);
                var genreToAdd = db.Genres.FirstOrDefault(g => g.Id == genreSong.GenreId);

                try
                {
                    singleSongToUpdate.Genres.Add(genreToAdd);
                    db.SaveChanges();
                    return Results.NoContent();

                }
                catch (DbUpdateException)
                {
                    return Results.BadRequest("Invalid data submitted");
                }
            });

            //delete a genre from a song
            app.MapDelete("/api/song/{songId}/deleteGenre/{genreId}", (PianoTunesAPIDbContext db, int genreId, int songId) =>
            {
                 var singleSongToUpdate = db.Songs
                .Include(s => s.Genres)
                .FirstOrDefault(s => s.Id == songId);
                var genreToDelete = db.Genres.FirstOrDefault(g => g.Id == genreId);

                try
                {
                    singleSongToUpdate.Genres.Remove(genreToDelete);
                    db.SaveChanges();
                    return Results.NoContent();

                }
                catch (DbUpdateException)
                {
                    return Results.BadRequest("Invalid data submitted");
                }
            });

        }
    }

}
