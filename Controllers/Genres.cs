using PianoTunesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PianoTunesAPI.Controllers
{
    public class Genres
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("api/genres", (PianoTunesAPIDbContext db) =>
            {
                return db.Genres
                .ToList();
            });

            app.MapGet("api/genres/{genreId}", (PianoTunesAPIDbContext db, int genreId) =>
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

            app.MapDelete("/api/genres/{genreId}", (PianoTunesAPIDbContext db, int genreId) =>
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
        }
    }

}
