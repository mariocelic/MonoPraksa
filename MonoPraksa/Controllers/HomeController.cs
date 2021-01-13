using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonoPraksa.Data;
using MonoPraksa.Models;
using MonoPraksa.Models.SchoolViewModels;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Proba.Controllers
{
    public class HomeController : Controller
    {
        private readonly PraksaContext _context;

        public HomeController(PraksaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            IQueryable<EnrollmentDateGroup> data =
                from student in _context.Students
                group student by student.EnrollmentDate into dateGroup
                select new EnrollmentDateGroup()
                {
                    EnrollmentDate = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
