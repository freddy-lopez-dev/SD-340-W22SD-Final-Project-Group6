using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SD_340_W22SD_Final_Project_Group6.DAL;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;
using System.Web.Mvc;

namespace SD_340_W22SD_Final_Project_Group6.BLL
{
    public class ProjectBusinessLogic
    {
        private readonly ProjectRepository _projectRepository;
        private readonly TicketRepository _ticketRepository;
        private readonly UserManager<ApplicationUser> _users;

        public ProjectBusinessLogic(ProjectRepository projectRepository, TicketRepository ticketRepository, UserManager<ApplicationUser> users)
        {
            _projectRepository = projectRepository;
            _ticketRepository = ticketRepository;
            _users = users;
        }

        public void AddProject(Project project)
        {
            _projectRepository.Add(project);
        }

        public void DelelteProject(Project project)
        {
            _projectRepository.Delete(project);
        }

        public Project GetProject(int id)
        {
            return _projectRepository.Get(id);
        }

        public Project GetProject(Func<Project, bool> predicate)
        {
            return _projectRepository.Get(predicate);
        }

        public List<Project> GetProjects()
        {
            return (List<Project>)_projectRepository.GetAll();
        }

        public List<Project> GetProjects(Func<Project, bool> predicate)
        {
            return (List<Project>)_projectRepository.GetList(predicate);
        }

        public Project UpdateProject(Project project)
        {
            return _projectRepository.Update(project);
        }

        public void SaveChanges()
        {
            _projectRepository.Save();
        }

        public ProjectRepository Get_projectRepository()
        {
            return _projectRepository;
        }

        public void Delete(int? id)
        {
            if (id == null || _projectRepository == null)
            {
                throw new ArgumentNullException("id");
            }

            var project = _projectRepository
                .Get(m => m.Id == id);
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
        }
    }
}