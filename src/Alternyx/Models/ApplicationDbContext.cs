using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Alternyx.Models;
using Microsoft.Data.Entity.Metadata;

namespace Alternyx.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<Idea> Ideas { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Idea>().HasOne(_ => _.Author).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
