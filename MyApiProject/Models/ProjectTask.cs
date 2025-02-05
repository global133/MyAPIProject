

namespace MyApiProject.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Project? Project { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
