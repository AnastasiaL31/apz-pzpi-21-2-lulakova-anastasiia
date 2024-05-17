using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartShelter_WebAPI.Dtos;
using System.Data;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Storekeeper")]
    
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IStaffService _staffService;

        public StorageController(IStorageService storageService, IStaffService staffService)
        {
            _storageService = storageService;
            _staffService = staffService;
        }

       
        [HttpGet("orders/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<OrderDto>> GetAllOrders()
        {
            var ordersList = _storageService.GetOrderList();
            if (ordersList == null || ordersList.Count == 0)
            {
                return NotFound();
            }

            return Ok(ordersList);
        }

        [HttpGet("orders/{staffId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<OrderDto>> GetStaffOrders(int staffId)
        {
            var ordersList = _storageService.GetApprovedOrders(staffId);
            if (ordersList == null || ordersList.Count == 0)
            {
                return NotFound();
            }

            return Ok(ordersList);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("approve/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult ApproveOrder(int orderId)
        {
            var result = _storageService.ApproveOrder(orderId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPost("order/add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddOrder([FromBody] AddOrderDto orderDto)
        {
            int staffId;
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            else
            {
                staffId = await _staffService.GetStaffId(userName);
            }
            var result =_storageService.CreateOrder(orderDto, staffId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("order/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateOrder(int staffId, [FromBody] UpdateOrderDto orderDto)
        {
            var result = _storageService.UpdateOrder(orderDto, staffId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("order/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  ActionResult DeleteOrder(int staffId, int orderId)
        {
            var result = _storageService.DeleteOrder(orderId, staffId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Storage>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Storage>> GetFullStorage()
        {
            var storage = _storageService.GetFullStorage();
            if (storage == null || storage.Count == 0)
            {
                return NotFound();
            }

            return Ok(storage);
        }

    
        [HttpGet("all/grouped")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Storage>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Storage>> GetFullStorageGrouped()
        {
            var storage = _storageService.GetGroupedStorage();
            if (storage == null || storage.Count == 0)
            {
                return NotFound();
            }

            return Ok(storage);
        }
    }
}
