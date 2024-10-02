using FineArt.Models.Data;
using FineArt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FineArt.Controllers
{
    public class WebController : Controller
    {
		private readonly ApplicationDbContext _applicationdb;

		public WebController(ApplicationDbContext applicationdb)
		{
			_applicationdb = applicationdb;

		}
		public IActionResult Index()
        {
			var viewModel = new GetModels
			{
				
				Competitions = _applicationdb.Competitions.ToList(),
				Submissions = _applicationdb.Submissions.ToList(),
				Exhibitions = _applicationdb.Exhibitions.ToList(),
			};
			return View(viewModel);
        }
        public IActionResult about()
        {
            return View();
        }
        public IActionResult contact()
        {
            return View();
        }
        public IActionResult events()
        {
            return View();
        }
        public IActionResult gallery()
        {
			var viewModel = new GetModels
			{
				ExhibitionPostings = _applicationdb.ExhibitionPostings.ToList(),
			};
			return View(viewModel);
		
		}	
    }
}
