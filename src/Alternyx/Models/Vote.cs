using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alternyx.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public int IdeaId { get; set; }

        [ForeignKey("IdeaId")]
        public Idea Idea{ get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int Value { get; set; }

        public DateTime DateTime { get; set; }
    }
}
