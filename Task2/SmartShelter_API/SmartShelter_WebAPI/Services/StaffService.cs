using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SmartShelter_WebAPI.Dtos;
using SmartShelter_WebAPI.Models;

namespace SmartShelter_WebAPI.Services
{
    public class StaffService : IStaffService
    {
        private readonly SmartShelterDBContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;


        public StaffService(SmartShelterDBContext dbContext, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> AddStaff(AddStaffDto newStaffDto, string username)
        {
            var identityUser = await _userManager.FindByEmailAsync(username);
            if (identityUser == null)
            {
                return false;
            }

            var staff = _mapper.Map<Staff>(newStaffDto);
            if (staff != null)
            {
                staff.AcceptanceDate = DateTime.UtcNow;
                staff.IdentityUserId = identityUser.Id;
                _dbContext.Add(staff);
            }

            return Save();
        }

        public async Task<bool> UpdateStaff(StaffDto staffDto, string senderUsername)
        {
            var access = await CheckAccess(null, "Admin", senderUsername);
            if (!access)
            {
                return false;
            }

            var staff = _mapper.Map<Staff>(staffDto);
            _dbContext.Update(staff);
            return Save();
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
                var result = await _userManager.AddToRoleAsync(identityUser, roleName);
                if (result.Succeeded)
                {
                    var user = _dbContext.Staff.FirstOrDefault(x => x.Id == staffId);
                    if (user != null)
                    {
                        user.HasRole = true;
                        _dbContext.Update(user);
                        return Save();
                    }
                }
            }

            return false;
        }

        public async Task<StaffDto?> GetById(int id, string senderUsername)
        {
            var userId = GetIdentityId(id);
            if (userId.IsNullOrEmpty())
            {
                return null;
            }
            if (await CheckAccess(userId, "", senderUsername))
            {
                var user = _dbContext.Staff.FirstOrDefault(x => x.Id == id);
                var mappedUser = _mapper.Map<StaffDto>(user);
                return mappedUser;
            }
            return null;
        }

        public async Task<List<GetStaffTaskDto>?> GetRoleTask(string role, string username)
        {
            var access = await CheckAccess(null, role, username);
            if (access)
            {
                var tasks = _dbContext.Tasks.Where(x => x.StaffRole == role)
                    .Include(x => x.Order)
                    .Include(x => x.AimStaff)
                    .Include(x => x.ByStaff)
                    .ToList();

                var tasksDto = new List<GetStaffTaskDto>();
                //var mapper = StaffTask.InitializeMapper();//_mapper.Map<List<GetStaffTaskDto>>(tasks);
                foreach (var task in tasks)
                {
                    //tasksDto.Add(mapper.Map<GetStaffTaskDto>(tasks));
                    var mappedTask = _mapper.Map<GetStaffTaskDto>(task);
                    mappedTask.AimStaff = _mapper.Map<AddStaffDto>(task.AimStaff);
                    mappedTask.ByStaff = _mapper.Map<AddStaffDto>(task.ByStaff);
                    mappedTask.Order = _mapper.Map<OrderDto>(task.Order);
                    tasksDto.Add(mappedTask);
                }

                return tasksDto;
            }

            return null;
        }

        public async Task<List<GetStaffTaskDto>?> GetUserTasks(int staffId, string senderUsername)
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
                var tasksDto = _mapper.Map<List<GetStaffTaskDto>>(tasks);
                //foreach (var task in tasks)
                //{
                //    tasksDto.Add(new GetStaffTaskDto()
                //    {

                //    });
                //}

                return tasksDto;
            }
            return null;
        }

        public bool CreateTask(AddStaffTaskDto taskDto, int staffId)
        {
            var task = _mapper.Map<StaffTask>(taskDto);
            task.ByStaffId = staffId;
            _dbContext.Add(task);
            return Save();
        }

        public async Task<bool> DeleteTask(int taskId, string senderUsername)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (task == null)
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

        public async Task<List<StaffDto>?> GetStaffList(string username)
        {
            var access = await CheckAccess(null, "Admin", username);
            if (access)
            {
                var staffList = _dbContext.Staff.ToList();
                var mappedStaffList = _mapper.Map<List<StaffDto>>(staffList);
                return mappedStaffList;
            }

            return null;
        }
        public async Task<List<StaffDto>?> GetStaffToAcceptList(string username)
        {
            var access = await CheckAccess(null, "Admin", username);
            if (access)
            {
                var staffList = _dbContext.Staff.Where(x => x.HasRole == false).ToList();
                var mappedStaffList = _mapper.Map<List<StaffDto>>(staffList);
                return mappedStaffList;
            }

            return null;
        }

        public async Task<bool> AcceptTask(int taskId, string senderUsername)
        {
            var task = _dbContext.Tasks.FirstOrDefault(x => x.Id == taskId);
            if (task == null || task.AimStaffId == null)
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
