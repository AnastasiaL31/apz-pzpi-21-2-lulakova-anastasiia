using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<List<StaffDto>>> GetAllStaff()
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var staffList = await _staffService.GetStaffList(userName);
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
        public async Task<ActionResult<List<StaffDto>>> GetAllStaffToAccept()
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var staffList = await _staffService.GetStaffToAcceptList(userName);
            if (staffList == null || staffList.Count == 0)
            {
                return NotFound();
            }

            return Ok(staffList);
        }

        [AllowAnonymous]
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> AddStaff([FromBody] AddStaffDto staffDto, [FromQuery] string email)
        {
           
            var result = await _staffService.AddStaff(staffDto, email);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateStaff([FromBody] StaffDto staffDto)
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            
            var result = await _staffService.UpdateStaff(staffDto, userName);
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
        public async Task<ActionResult<bool>> AddRole(string roleName, int staffId)
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var result = await _staffService.AddRole(staffId, roleName, userName);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }



        [HttpGet("all/{staffId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StaffDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StaffDetailsDto>> GetStaffById(int staffId)
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var staff = await _staffService.GetById(staffId, userName);
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
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var staff = await _staffService.GetRoleTask(role, userName);
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
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var staff = await _staffService.GetUserTasks(staffId, userName);
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
        public async Task<ActionResult<bool>> AcceptTask(int taskId)
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var result = await _staffService.AcceptTask(taskId, userName);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpDelete("delete/task/{taskId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteTask(int taskId)
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var result = await _staffService.DeleteTask(taskId, userName);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("update/task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdateTask([FromBody] UpdateStaffTaskDto taskDto)
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            var result = await _staffService.UpdateTask(taskDto, userName);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
