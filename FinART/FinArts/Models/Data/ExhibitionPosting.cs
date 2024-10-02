using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineArt.Models.Data
{
    public class ExhibitionPosting
    {
        [Key]
        public int ExPID { get; set; }
        public string? PaintingTitle { get; set; }
        public double Price { get; set; }
        public Sold? IsSold { get; set; }
        public Paid? IsPaidToStudent { get; set; }
        public double AmountPaid { get; set; }
        [NotMapped]
        public IFormFile? EPFILE { get; set; }
        public string? EPIMG {  get; set; } 
        public enum Sold
        {
            Sold = 0,
            NotSold = 1
        }
        public enum Paid
        {
            Paid = 0,
            NotPaid = 1
        }
    }

}
