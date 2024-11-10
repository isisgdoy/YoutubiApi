using System.ComponentModel.DataAnnotations;
namespace YoutubiApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public List<ItemPlaylist> ItemPlaylists { get; set; }
    }
}
