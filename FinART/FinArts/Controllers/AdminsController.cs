using FineArt.Models.Data;
using FineArt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FineArt.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _applicationdb;

        public AdminsController (ApplicationDbContext applicationdb )
        {
            _applicationdb = applicationdb;
           
        }

        public IActionResult Index()
        {
            var viewModel = new GetModels
    {
        Staffs = _applicationdb.Staffs.ToList(),
        Competitions = _applicationdb.Competitions.ToList()
    };

    return View(viewModel);
        }
    }
}
