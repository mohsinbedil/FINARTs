using System.Collections.Generic;
using FineArt.Models; // Import necessary models
using FineArt.Models.Data;

namespace FineArt.ViewModels
{
    public class GetModels
    {       
        public List<Competition>? Competitions { get; set; }
        public List<Submission>? Submissions { get; set; }
        public List<Exhibition>? Exhibitions { get; set; }
        public List<ExhibitionPosting>? ExhibitionPostings { get; set; }
        public List<Staff>? Staffs { get; set; }
        
 
       
    }
}
