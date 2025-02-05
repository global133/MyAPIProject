
namespace MyApiProject.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask> ();
    }
}
