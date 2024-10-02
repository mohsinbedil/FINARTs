using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FineArt.Models.Data
{
    public class Exhibition
    {
        [Key]
        public int Exid { get; set; }
        public string? Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [NotMapped]
        public IFormFile? ExhibitionFile { get; set; }
        public string? ExhibitionIMG { get; set; }



    }
}

