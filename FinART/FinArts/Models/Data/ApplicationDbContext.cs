using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FineArt.Models.Data;

namespace FineArt.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Staff>? Staffs { get; set; }
        public DbSet<Competition>? Competitions { get; set; }
        public DbSet<Submission>? Submissions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<ExhibitionPosting> ExhibitionPostings { get; set; }
        


    }
}
   

