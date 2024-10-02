using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineArt.Models.Data
{
    public class Staff
    {
        [Key]
        public int SID { get; set; }
        [Required]
        public string? SName { get; set; }
        [Required]
        public string? Gender { get; set;}
        [Required]
        public string? Email { get; set;}
        [Required]
        public string? Password { get; set;}
        [NotMapped]
        public DateOnly Joined { get; set; }
        public string? classes { get; set; }
        public string? Subject { get; set; }





    }
}
