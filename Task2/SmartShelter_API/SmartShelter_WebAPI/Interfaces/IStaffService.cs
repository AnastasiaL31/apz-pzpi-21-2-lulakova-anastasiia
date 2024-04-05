namespace SmartShelter_WebAPI.Interfaces
{
    public interface IStaffService
    {
        public Task<List<Staff>?> GetStaffList(string username);
        public Task<List<Staff>?> GetStaffToAcceptList(string username);
        public Task<bool> AddRole(int staffId,  string roleName, string senderUsername);
        public Task<Staff?> GetById(int id, string username);
        public Task<List<StaffTask>?> GetRoleTask(string role, string username);
        public Task<List<StaffTask>?> GetUserTasks(int staffId, string senderUsername);
        public bool CreateTask(StaffTask task, int staffId);
        public Task<bool> AcceptTask(StaffTask task, string senderUsername);
        public Task<bool> DeleteTask(StaffTask task, string senderUsername);
        public Task<bool> UpdateTask(StaffTask task, string senderUsername);
        
    }
}