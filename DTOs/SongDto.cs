using PianoTunesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace PianoTunesAPI.DTOs
{
    public class SongDto
    {
        public string Title { get; set; }
        public string Album { get; set; }
        public int ArtistId { get; set; }
        public decimal Length { get; set; }
    }
}
