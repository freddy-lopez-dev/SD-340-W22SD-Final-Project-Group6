using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.DAL
{
    public class ProjectRepository : IRepository<Project>
    {
        private ApplicationDbContext _db;
        public ProjectRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Project project)
        {
            _db.Projects.Add(project);
        }

        public void Delete(Project project)
        {
            _db.Projects.Remove(project);
        }

        public Project Get(int id)
        {
            return _db.Projects.Include(p => p.AssignedTo).Include(p => p.Tickets).First(p => p.Id == id);
        }

        public Project Get(Func<Project, bool> predicate)
        {
            return _db.Projects.Include(p => p.AssignedTo).Include(p => p.Tickets).First(predicate);
        }

        public ICollection<Project> GetAll()
        {
            return _db.Projects.Include(p => p.AssignedTo).Include(p => p.Tickets).ToList();
        }

        public ICollection<Project> GetList(Func<Project, bool> predicate)
        {
            return _db.Projects.Include(p => p.AssignedTo).Include(p => p.Tickets).Where(predicate).ToList();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public Project Update(Project project)
        {
            _db.Projects.Update(project);
            return project;
        }
    }
}
