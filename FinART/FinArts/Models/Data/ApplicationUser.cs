using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FineArt.Models.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string? Name { get; set; }
      


       


    }
}
