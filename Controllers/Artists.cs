using PianoTunesAPI.Models;
using PianoTunesAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace PianoTunesAPI.Controllers
{
    public class Artists
    {
        public static void Map(WebApplication app)
        {
            //get all the artists
            app.MapGet("/api/artists", (PianoTunesAPIDbContext db) =>
            {
                return db.Artists
                .ToList();
            });

            //get artists via id
            app.MapGet("api/artists/{id}", (PianoTunesAPIDbContext db, int id) =>
            {
                Artist singleArtist = db.Artists
                .SingleOrDefault(x => x.Id == id);
                if (singleArtist == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(singleArtist);

              
            });

            //delete an artist
            app.MapDelete("/api/artists/{artistId}", (PianoTunesAPIDbContext db, int artistId) =>
            {
                Artist singleArtist = db.Artists
                .FirstOrDefault(x => x.Id == artistId);

                try
                {
                    db.Artists.Remove(singleArtist);
                    db.SaveChanges();
                    return Results.NoContent();
                }
                catch (DbUpdateException)
                {
                    return Results.NoContent();
                }
            });

            //update an artist
            app.MapPut("/api/artists/{artistId}", (PianoTunesAPIDbContext db, int artistId, ArtistDto dto) =>
            {
                Artist artistToUpdate = db.Artists.SingleOrDefault(a => a.Id == artistId);
                if (artistToUpdate == null)
                {
                    return Results.NotFound();
                }
               
                artistToUpdate.Name = dto.Name;
                artistToUpdate.Age = dto.Age;
                artistToUpdate.Bio = dto.Bio;

                db.SaveChanges();
                return Results.Ok(artistToUpdate);
            });

            //View single artist with the list of songs
            app.MapGet("/api/artist/songs/{artistId}", (PianoTunesAPIDbContext db, int artistId) =>
            {
                var filteredArtist = db.Artists
                .Include(a => a.Songs)
                .ThenInclude(s => s.Genres)
                .SingleOrDefault(a => a.Id == artistId);
                return filteredArtist;

            });

            //Create an artist
            app.MapPost("/api/artist/new", (PianoTunesAPIDbContext db, ArtistDto dto) =>
            {
                Artist newArtist = new() { Name = dto.Name, Age = dto.Age, Bio = dto.Bio };
                db.Artists.Add(newArtist);
                db.SaveChanges();
                return Results.Created($"/api/artist/new/{newArtist.Id}", newArtist);
            });

        }
    }
}
