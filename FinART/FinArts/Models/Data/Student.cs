using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineArt.Models.Data
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        public string? StudentName { get; set; }
        [Required]
        public string? StudentEmail { get; set; }
        [Required]
        public string? StudentPassword { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
       
        public DateTime DOB { get; set; }
        [Required]
        public int Contact { get; set; }
        [Required]
        public string? ResidentialAdress { get; set; }
        [Required]
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public string? Class { get; set; }
       
    }
   
}