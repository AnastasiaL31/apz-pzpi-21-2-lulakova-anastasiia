using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Interfaces
{
    public interface IStaffService
    {
        public Task<List<StaffDto>?> GetStaffList(string username);
        public Task<List<StaffDto>?> GetStaffToAcceptList(string username);
        public Task<bool> AddStaff(AddStaffDto newStaffDto, string username);
        public Task<bool> UpdateStaff(StaffDto staffDto, string username);
        public Task<bool> AddRole(int staffId,  string roleName, string senderUsername);
        public Task<StaffDto?> GetById(int id, string username);
        public Task<List<GetStaffTaskDto>?> GetRoleTask(string role, string username);
        public Task<List<GetStaffTaskDto>?> GetUserTasks(int staffId, string senderUsername);
        public bool CreateTask(AddStaffTaskDto taskDto, int staffId);
        public Task<bool> AcceptTask(int taskId, string senderUsername);
        public Task<bool> DeleteTask(int taskId, string senderUsername);
        public Task<bool> UpdateTask(UpdateStaffTaskDto taskDto, string senderUsername);
        
    }
}