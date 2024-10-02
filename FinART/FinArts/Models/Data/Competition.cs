using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace FineArt.Models.Data
{
    public class Competition
    {
        [Key]
        public int CId { get; set; }
        [Required(ErrorMessage = "Competition Name is required")]
        public string? Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string? Conditions { get; set; }
        [Required]
        [Display(Name ="Award")]
        public string? AwardDetails { get; set; }
        [NotMapped]
        public IFormFile Pic {  get; set; }
        [Column(TypeName = "Varchar(max)")]
        public string? IMG {  get; set; }

    }


}
