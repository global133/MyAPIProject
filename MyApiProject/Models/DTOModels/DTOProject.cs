namespace MyApiProject.Models.DTOModels
{
    public class DTOProject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<int> UserIds { get; set; } = new List<int>();
        public List<int> TaskIds { get; set; } = new List<int>();
    }
}
