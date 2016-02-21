using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Alternyx.Models;

namespace Alternyx.Controllers
{
    public class VotesController : Controller
    {
        private ApplicationDbContext _context;

        public VotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Votes
        //public IActionResult Index(int count = 100)
        //{
        //    var votes = _context.Votes.Skip(Math.Max(0, _context.Votes.Count() - count)).Include(v => v.Idea).Include(v => v.User);
        //    return View(votes.ToList());
        //}

        public int VoteUp(int ideaId)
        {
            return Vote(ideaId, 1);
        }

        public int VoteDown(int ideaId)
        {
            return Vote(ideaId, -1);
        }

        private int Vote(int ideaId, int value)
        {
            var userIdStr = User.GetUserId();
            if (userIdStr == null) return 0;
            var userId = int.Parse(userIdStr);

            var vote = _context.Votes.SingleOrDefault(v => v.UserId == userId && v.IdeaId == ideaId);
            bool userChangedVote = false;
            // if this user alrady voted before
            if (vote != null)
            {
                if (vote.Value != value)
                {
                    vote.Value += 2*value;
                    _context.Update(vote);
                    userChangedVote = true;
                }
            }
            else
            {
                vote = new Vote
                {
                    DateTime = DateTime.Now,
                    UserId = userId,
                    IdeaId = ideaId,
                    Value = value
                };

                _context.Add(vote);
            }

            var idea = _context.Ideas.SingleOrDefault(i => i.Id == ideaId);
            if (idea != null)
            {
                idea.Value += value;
                if (userChangedVote)
                {
                    idea.Value += value;
                }

                _context.Update(idea);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }

            return idea?.Value ?? 0;
        }
    }
}
