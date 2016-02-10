using System.Linq;
using System.Security.Claims;
using Alternyx.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace Alternyx.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var openIdeas = _context.Ideas
                .Where(_ => _.Status == IdeaStatus.Open)
                .Include(i => i.Author)
                .OrderByDescending(_=>_.Value);

            var userIdStr = User.GetUserId();
            if (userIdStr != null)
            {
                var userId = int.Parse(userIdStr);
                var votes = _context.Votes.Where(v => v.UserId == userId).Select(v=>v.IdeaId).ToList();
                ViewBag.Votes = votes;
            }
            
            return View(openIdeas.ToList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
