using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineArt.Models.Data
{
    public class Award
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Student Name")]
        public string? Stud_Name { get; set; }
        [Required]
        [Display(Name = "Competition Name")]
        public string? Competition_Name { get; set; }
        [Required]
        public string AwardDetails { get; set; }
      
    }
}
