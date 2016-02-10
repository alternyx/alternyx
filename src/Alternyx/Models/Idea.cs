using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alternyx.Models
{
    public class Idea
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(5000)]
        [MinLength(10)]
        public string Description { get; set; }

        public int Value { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser Author { get; set; }

        public IdeaStatus Status { get; set; }
    }

    public enum IdeaStatus
    {
        Open,
        OnReview,
        Implemented,
        Canceled
    }
}