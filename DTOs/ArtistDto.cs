using PianoTunesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace PianoTunesAPI.DTOs
{
    public class ArtistDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Bio { get; set; }
    }
}
