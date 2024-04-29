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

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
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


        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  ActionResult AddOrder(int staffId, [FromBody] AddOrderDto orderDto)
        {
            var result =_storageService.CreateOrder(orderDto, staffId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("update")]
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

        [HttpPut("delete")]
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
    }
}
