using Microsoft.AspNetCore.Identity;
using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.BLL
{
    public class UserBusinessLogic
    {
        public UserManager<ApplicationUser> _userManager;

        public UserBusinessLogic(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUser> GetUserByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<List<ApplicationUser>> GetUsersByRole(string role)
        {
            IList<ApplicationUser> users = await _userManager.GetUsersInRoleAsync(role);

            return users.ToList();
        }

        public async Task ReassignRole(string userId, string role)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("No user found with this Id");
            }

            ICollection<string> userRoles = await _userManager.GetRolesAsync(user);

            foreach (string userRole in userRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, userRole);
            }

            await _userManager.AddToRoleAsync(user, role);          
        }

        public async Task<List<string>> GetRoles(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("No user found with this Id");
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }

        public Task ReassignRole(ApplicationUser currUser, string v)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetRoles(ApplicationUser currUser)
        {
            throw new NotImplementedException();
        }
    }
}