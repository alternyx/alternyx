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
                .OrderByDescending(_ => _.Value);

            var userIdStr = User.GetUserId();
            if (userIdStr != null)
            {
                var userId = int.Parse(userIdStr);

                var votesUp = _context.Votes.Where(v => v.UserId == userId && v.Value == 1).Select(v => v.IdeaId).ToList();
                ViewBag.VotesUp = votesUp;

                var votesDown = _context.Votes.Where(v => v.UserId == userId && v.Value == -1).Select(v => v.IdeaId).ToList();
                ViewBag.VotesDown = votesDown;
            }

            return View(openIdeas.ToList());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
