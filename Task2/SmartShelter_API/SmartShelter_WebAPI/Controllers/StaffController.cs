using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StaffDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<StaffDto>>> GetAllStaff(string senderUsername)
        {
            var staffList = await _staffService.GetStaffList(senderUsername);
            if (staffList == null || staffList.Count == 0)
            {
                return NotFound();
            }

            return Ok(staffList);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all/accept")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StaffDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<StaffDto>>> GetAllStaffToAccept(string senderUsername)
        {
            var staffList = await _staffService.GetStaffToAcceptList(senderUsername);
            if (staffList == null || staffList.Count == 0)
            {
                return NotFound();
            }

            return Ok(staffList);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddStaff(string username, [FromBody] AddStaffDto staffDto)
        {
            var result = await _staffService.AddStaff(staffDto, username);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateStaff(string username, [FromBody] StaffDto staffDto)
        {
            var result = await _staffService.UpdateStaff(staffDto, username);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("addRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddRole(string senderUsername, string roleName, int staffId)
        {
            var result = await _staffService.AddRole(staffId, roleName, senderUsername);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }



        [HttpGet("all/{staffId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Staff>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Staff>> GetStaffById(int staffId, string senderUsername)
        {
            var staff = await _staffService.GetById(staffId, senderUsername);
            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staff);
        }

        [HttpGet("{role}/tasks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetStaffTaskDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetStaffTaskDto>> GetRoleTasks(string role, string senderUsername)
        {
            var staff = await _staffService.GetRoleTask(role, senderUsername);
            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staff);
        }

        [HttpGet("{staffId:int}/tasks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetStaffTaskDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetStaffTaskDto>> GetUserTasks(int staffId, string senderUsername)
        {
            var staff = await _staffService.GetUserTasks(staffId, senderUsername);
            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staff);
        }



        [HttpPost("add/task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  ActionResult<bool> AddTask(int staffId, [FromBody]AddStaffTaskDto taskDto)
        {
            var result =  _staffService.CreateTask(taskDto, staffId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }



        [HttpPost("accept/task/{taskId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AcceptTask(int taskId, string senderUsername)
        {
            var result = await _staffService.AcceptTask(taskId, senderUsername);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpDelete("delete/task/{taskId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteTask(int taskId, string senderUsername)
        {
            var result = await _staffService.DeleteTask(taskId, senderUsername);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("update/task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateTask(string senderUsername, [FromBody] UpdateStaffTaskDto taskDto)
        {
            var result = await _staffService.UpdateTask(taskDto, senderUsername);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
