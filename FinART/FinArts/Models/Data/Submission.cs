using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineArt.Models.Data
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Competition Name")]
        public string? Compet_Name { get; set; }
        [Required]
        [Display(Name = "Posted By")]
        public string? Stud_Name { get; set; } // Quotation, Story, Poem, etc.
        [Required]
        [Display(Name = "Tell About Your Design")]
        public string? Description { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public string? Marks { get; set; }
        public string? FeedBack { get; set; }
        //public bool Disqualified { get; set; }
        [NotMapped]
        public IFormFile Design { get; set; }
        [Column(TypeName = "Varchar(max)")]
        public string? DIMG { get; set; }

        public bool IsDisqualified(DateTime competitionEndDate)
        {
            // Check if submission date is beyond the end date for the competition
            return SubmissionDate > competitionEndDate;
        }

    }
}
