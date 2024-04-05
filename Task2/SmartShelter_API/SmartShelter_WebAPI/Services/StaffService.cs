using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SmartShelter_WebAPI.Models;

namespace SmartShelter_WebAPI.Services
{
    public class StaffService : IStaffService
    {
        private readonly SmartShelterDBContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        

        public StaffService(SmartShelterDBContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            
        }

        public async Task<bool> AddRole(int staffId, string roleName, string senderUsername)
        {
            var access = await CheckAccess(null, "Admin", senderUsername);
            if (!access)
            {
                return false;
            }

            var identityUserId = GetIdentityId(staffId);
            var identityUser = await _userManager.FindByIdAsync(identityUserId);
            if (identityUser != null)
            {
                var result = _userManager.AddToRoleAsync(identityUser, roleName);
                if (result.IsCompletedSuccessfully)
                {
                    var user = _dbContext.Staff.FirstOrDefault(x => x.Id == staffId);
                    if (user != null)
                    {
                        user.HasRole = true;
                        return Save();
                    }
                }
            }

            return false;
        }

        public async Task<Staff?> GetById(int id, string senderUsername)
        {
            var userId = GetIdentityId(id);
            if (userId.IsNullOrEmpty())
            {
                return null;
            }
            if (await CheckAccess(userId, "", senderUsername))
            {
                var user = _dbContext.Staff.FirstOrDefault(x => x.Id == id);
                return user;
            }
            return null;
        }

        public async Task<List<StaffTask>?> GetRoleTask(string role, string username)
        {
            var access = await CheckAccess(null, role, username);
            if (access)
            {
                var tasks = _dbContext.Tasks.Where(x => x.StaffRole == role).Include(x => x.Order).ToList();
                return tasks;
            }

            return null;
        }

        public async Task<List<StaffTask>?> GetUserTasks(int staffId, string senderUsername)
        {
            var userId = GetIdentityId(staffId);
            if (userId.IsNullOrEmpty())
            {
                return null;
            }
            var access = await CheckAccess(userId, "", senderUsername);
            if (access)
            {
                var tasks = _dbContext.Tasks.Where(x => x.AimStaffId == staffId).Include(x => x.Order).ToList();
                return tasks;
            }
            return null;
        }

        public bool CreateTask(StaffTask task, int staffId)
        {
            task.ByStaffId = staffId;
            _dbContext.Add(task);
            return Save();
        }

        public async Task<bool> DeleteTask(StaffTask task, string senderUsername)
        {
            var creatorId = GetIdentityId(task.ByStaffId);
            if (creatorId.IsNullOrEmpty())
            {
                return false;
            }
            var access = await CheckAccess(creatorId, "", senderUsername);
            if (access)
            {
                _dbContext.Remove(task);
                return Save();
            }

            return false;
        }

        public async Task<bool> UpdateTask(StaffTask task, string senderUsername)
        {
            if (task.IsAccepted)
            {
                return false;
            }
            var creatorId = GetIdentityId(task.ByStaffId);
            if (creatorId.IsNullOrEmpty())
            {
                return false;
            }
            var access = await CheckAccess(creatorId, "", senderUsername);
            if (access)
            {
                _dbContext.Update(task);
                return Save();
            }
            return false;
        }

        public async Task<List<Staff>?> GetStaffList(string username)
        {
            var access = await CheckAccess(null, "Admin", username);
            if (access)
            {
                var staffList = _dbContext.Staff.ToList();
                return staffList;
            }

            return null;
        }
        public async Task<List<Staff>?> GetStaffToAcceptList(string username)
        {
            var access = await CheckAccess(null, "Admin", username);
            if (access)
            {
                var staffList = _dbContext.Staff.Where(x => x.HasRole == false).ToList();
                return staffList;
            }

            return null;
        }

        public async Task<bool> AcceptTask(StaffTask task, string senderUsername)
        {
            if (task.AimStaffId == null)
            {
                return false;
            }
            var executorId = GetIdentityId((int)task.AimStaffId);
            if (executorId.IsNullOrEmpty())
            {
                return false;
            }
            var access = await CheckAccess(executorId, "", senderUsername);
            if (access)
            {
                task.IsAccepted = true;
                _dbContext.Update(task);
                return Save();
            }

            return false;
        }


        


        private string GetIdentityId(int staffId)
        {
            var user = _dbContext.Staff.FirstOrDefault(x => x.Id == staffId);
            if (user == null)
            {
                return "";
            }

            return user.IdentityUserId;
        }

        //neededRole is empty when getting info about yourself
        //id is not null when accessing info about staff with id
        private async Task<bool> CheckAccess(string? id, string neededRole, string senderUsername)
        {
            var sender = await _userManager.FindByEmailAsync(senderUsername);
            if (sender == null)
            {
                return false;
            }

            if (id != null)
            {
                var identityUser = await _userManager.FindByIdAsync(id);
                if (identityUser == null)
                {
                    return false;
                }

                if (sender.Id == identityUser.Id)
                {
                    return true;
                }
            }

            var senderRoles = await _userManager.GetRolesAsync(sender);
            
            if (senderRoles.Any())
            {
                if (senderRoles[0] == neededRole || senderRoles[0] == "Admin")
                {
                    return true;
                }
            }

            return false;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() != 0;
        }
    }
}
