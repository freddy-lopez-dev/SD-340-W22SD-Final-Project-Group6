namespace SD_340_W22SD_Final_Project_Group6.Models
{
    public class ProjectUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }

        public ApplicationUser User { get; set; }
        public Project Project { get; set; }
    }
}
