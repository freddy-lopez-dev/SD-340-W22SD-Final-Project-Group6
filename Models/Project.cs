using System.ComponentModel.DataAnnotations;

namespace SD_340_W22SD_Final_Project_Group6.Models
{
    public class Project
    {
        public int Id { get; set; }
        [StringLength(200, ErrorMessage = "Project Name should be from 5 upto 200 characters only")]
        [MinLength(5)]
        public string ProjectName { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<ProjectUser> AssignedTo { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        public Project()
        {
            AssignedTo = new HashSet<ProjectUser>();
            Tickets = new HashSet<Ticket>();
        }

    }
}
