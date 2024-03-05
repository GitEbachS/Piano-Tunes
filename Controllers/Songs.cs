using PianoTunesAPI.Models;
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

            app.MapGet("api/songs/{songId}", (PianoTunesAPIDbContext db, int songId) =>
            {
                Song singleSong = db.Songs
                .Include(s => s.Artist)
                .FirstOrDefault(s => s.Id == songId);

                if (singleSong == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleSong);
            });

            //delete a single song
            app.MapDelete("api/songs/{songId}", (PianoTunesAPIDbContext db, int songId) =>
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
        }
    }
    
}
