using System.ComponentModel.DataAnnotations;

namespace YoutubiApi.Models
{
    public class Criador
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
    }
}
