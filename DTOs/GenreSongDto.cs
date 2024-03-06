using PianoTunesAPI.Models;
using System.ComponentModel.DataAnnotations;
namespace PianoTunesAPI.DTOs
{
    public class GenreSongDto
    {
        public int GenreId { get; set; }
        public int SongId { get; set; }
    }
}
