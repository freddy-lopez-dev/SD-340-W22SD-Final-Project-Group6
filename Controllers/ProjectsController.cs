using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.BLL;
using SD_340_W22SD_Final_Project_Group6.DAL;
using SD_340_W22SD_Final_Project_Group6.Data;
using SD_340_W22SD_Final_Project_Group6.Models;
using X.PagedList;

namespace SD_340_W22SD_Final_Project_Group6.Controllers
{
    [Authorize(Roles = "ProjectManager, Developer")]
    public class ProjectsController : Controller
    {
        private readonly ProjectBusinessLogic projectBL;
        private readonly TicketBusinessLogic ticketBL;
        private readonly UserProjectBusinessLogic userProjectBL;
        private UserBusinessLogic userBL;

        public ProjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            ticketBL = new TicketBusinessLogic(new TicketRepository(context));
            userBL = new UserBusinessLogic(userManager);
            projectBL = new ProjectBusinessLogic(new ProjectRepository(context));
            userProjectBL = new UserProjectBusinessLogic(new UserProjectRepository(context));
        }
        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index(string? sortOrder, int? page, bool? sort, string? userId)
        {
            List<Project> SortedProjs = new List<Project>();
            List<ApplicationUser> allUsers = userBL.GetAllUsers(userId);
            List<SelectListItem> users = new List<SelectListItem>();

            allUsers.ForEach(au =>
            {
                users.Add(new SelectListItem(au.UserName, au.Id.ToString()));
            });
            ViewBag.Users = users;
            switch (sortOrder)
            {
                case "Priority":
                    if (sort == true)
                    {
                        SortedProjs = projectBL.OrderByPriority();
                    }
                    else
                    {
                        SortedProjs = projectBL.OrderByPriorityAsc();
                    }

                    break;
                case "RequiredHrs":
                    if (sort == true)
                    {
                        SortedProjs = projectBL.OrderByRequiredHours();
                    }
                    else
                    {
                        SortedProjs = projectBL.OrderByRequiredHoursAsc();
                    }

                    break;
                case "Completed":
                    SortedProjs = projectBL.GetCompletedProjects();
                    break;
                default:
                    if (userId != null)
                    {
                        SortedProjs = projectBL.GetAllProjects();
                    }
                    else
                    {
                        SortedProjs = projectBL.GetAllProjects();
                    }
                    break;
            }

            var LogedUserName = User.Identity.Name;  // logined user name
            var user = userBL.GetUserByName(LogedUserName);
            var rolenames = userBL.ToString();
            var AssinedProject = new List<Project>();

            if (rolenames.Contains("Developer"))
            {
                AssinedProject = SortedProjs;
            }
            else
            {
                AssinedProject = SortedProjs;
            }
            IPagedList<Project> projList = AssinedProject.ToPagedList(page ?? 1, 3);

            return View(projList);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || projectBL == null)
            {
                return NotFound();
            }

            var project = projectBL.GetProject((int)id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public async Task<IActionResult> RemoveAssignedUser(string id, int projId)
        {
            if (id == null)
            {
                return NotFound();
            }
            UserProject currUserProj = userProjectBL.GetUserProject(up => up.ProjectId == projId && up.UserId == id);
            userProjectBL.Delete(currUserProj);

            return RedirectToAction("Edit", new { id = projId });
        }

        // GET: Projects/Create
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> CreateAsync()
        {
            List<ApplicationUser> allUsers = (List<ApplicationUser>)userBL.GetAllUsers("Developer");

            List<SelectListItem> users = new List<SelectListItem>();
            allUsers.ForEach(au =>
            {
                users.Add(new SelectListItem(au.UserName, au.Id.ToString()));
            });
            ViewBag.Users = users;

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Create([Bind("Id,ProjectName")] Project project, List<string> userIds)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;

                ApplicationUser createdBy = await userBL.GetUserByName(userName);
                userIds.ForEach(async (user) =>
                {
                    ApplicationUser currUser = await userBL.GetUser(user);
                    UserProject newUserProj = new UserProject();
                    newUserProj.ApplicationUser = currUser;
                    newUserProj.UserId = currUser.Id;
                    newUserProj.Project = project;
                    project.AssignedTo.Add(newUserProj);
                });
                projectBL.CreateProject(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || projectBL == null)
            {
                return NotFound();
            }

            var project = projectBL.GetProject((int)id);

            if (project == null)
            {
                return NotFound();
            }

            List<ApplicationUser> results = await userBL.GetAllUsers(project.Id.ToString()).ToListAsync();
            List<SelectListItem> currUsers = new List<SelectListItem>();

            results.ForEach(r =>
            {
                currUsers.Add(new SelectListItem(r.UserName, r.Id.ToString()));
            });
            ViewBag.Users = currUsers;

            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Edit(int id, List<string> userIds, [Bind("Id,ProjectName")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userIds.ForEach(async (user) =>
                    {
                        ApplicationUser currUser = await userBL.GetUserByName(user);
                        UserProject newUserProj = new UserProject();
                        newUserProj.ApplicationUser = currUser;
                        newUserProj.UserId = currUser.Id;
                        newUserProj.Project = project;
                        project.AssignedTo.Add(newUserProj);
                    });
                    projectBL.EditProject(project);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || projectBL == null)
            {
                return NotFound();
            }

            var project = projectBL.GetProject((int)id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (projectBL == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Projects'  is null.");
            }
            var project = projectBL.GetProject(id);

            if (project != null)
            {
                List<Ticket> tickets = project.Tickets.ToList();
                tickets.ForEach(ticket =>
                {
                    ticketBL.DeleteTicket(ticket);
                });

                List<UserProject> userProjects = userProjectBL.GetUserProjects(up => up.ProjectId == project.Id).ToList();
                userProjects.ForEach(userProj =>
                {
                    userProjectBL.Delete(userProj);
                });

                projectBL.DeleteProject(project);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return (projectBL.GetAllProjects()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}