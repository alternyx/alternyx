using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Alternyx.Models;
using Microsoft.AspNet.Authorization;

namespace Alternyx.Controllers
{
    public class IdeasController : Controller
    {
        private ApplicationDbContext _context;

        public IdeasController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Ideas
        public IActionResult Index()
        {
            var myIdeas = _context.Ideas
                .Where(_=>_.UserId.ToString() == User.GetUserId())
                .Include(i => i.Author);

            return View(myIdeas.ToList());
        }

        // GET: Ideas/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Idea idea = _context.Ideas.Single(m => m.Id == id);
            if (idea == null)
            {
                return HttpNotFound();
            }

            ViewBag.Comments = _context.Comments.Where(c => c.IdeaId == id).Include(c=>c.User);
            return View(idea);
        }

        // GET: Ideas/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(int ideaId, string commentText)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.GetUserId());

                Comment comment = new Comment
                {
                    DateTime = DateTime.Now,
                    UserId = userId,
                    IdeaId = ideaId,
                    Text = commentText
                };
                _context.Add(comment);
                _context.SaveChanges();
            }

            return Details(ideaId);
        }

        // GET: Ideas/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ideas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Idea idea)
        {
            if (ModelState.IsValid)
            {
                idea.UserId = int.Parse(User.GetUserId());

                _context.Ideas.Add(idea);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(idea);
        }

        // GET: Ideas/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Idea idea = _context.Ideas.Single(m => m.Id == id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Author", idea.UserId);
            return View(idea);
        }

        // POST: Ideas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Idea idea)
        {
            if (ModelState.IsValid)
            {
                _context.Attach(idea);

                _context.Entry(idea).Property(_ => _.Name).IsModified = true;
                _context.Entry(idea).Property(_ => _.Description).IsModified = true;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Author", idea.UserId);
            return View(idea);
        }

        // GET: Ideas/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Idea idea = _context.Ideas.Single(m => m.Id == id);
            if (idea == null)
            {
                return HttpNotFound();
            }

            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Idea idea = _context.Ideas.Single(m => m.Id == id);
            _context.Ideas.Remove(idea);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Ideas/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddComment(Idea idea)
        {
            if (ModelState.IsValid)
            {
                idea.UserId = int.Parse(User.GetUserId());

                _context.Ideas.Add(idea);
                _context.SaveChanges();
                
            }

            return RedirectToAction("Details");
        }
    }
}
