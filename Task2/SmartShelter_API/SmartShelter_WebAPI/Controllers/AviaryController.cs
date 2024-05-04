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
    
    public class AviaryController : ControllerBase
    {
        private readonly IAviaryService _aviaryService;
        private readonly IStaffService _staffService;

        public AviaryController(IAviaryService aviaryService, IStaffService staffService)
        {
            _aviaryService = aviaryService;
            _staffService = staffService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AviaryDescription>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<AviaryDescription>> GetAll()
        {
            var aviaryList = _aviaryService.GetAllAviaries();
            if (aviaryList.Count == 0)
            {
                return NotFound();
            }

            return Ok(aviaryList);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Aviary))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("aviary/{animalId:int}")]
        public ActionResult<Aviary> GetAnimalAviary(int animalId)
        {
            if (animalId <= 0)
            {
                return BadRequest();
            }
            var aviary = _aviaryService.GetAnimalAviary(animalId);
            if (aviary == null)
            {
                return NotFound("");
            }
            return Ok(aviary);
        }


        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddAviary([FromBody] AddAviaryDto aviaryDto)
        {
            var result = _aviaryService.AddAviary(aviaryDto);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("change")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult ChangeAnimalAviary( int animalId, int newAviaryId)
        {
            if (animalId <= 0 || newAviaryId <= 0)
            {
                return BadRequest();
            }
            var result = _aviaryService.ChangeAviary(animalId, newAviaryId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("aviary/free")]
        public ActionResult<Aviary> GetFreeAviaries()
        {
            
            var aviaries = _aviaryService.GetFreeAviaries();
            if (aviaries.Count == 0)
            {
                return NotFound("");
            }
            return Ok(aviaries);
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateAviary(Aviary aviary)
        {
            
            var result = _aviaryService.UpdateAviary(aviary);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id:int}")]
        public ActionResult DeleteAviary(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var result = _aviaryService.RemoveAviary(id);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AviaryCondition))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("/condition/{aviaryId:int}")]
        public ActionResult<AviaryCondition> GetAviaryCondition(int aviaryId)
        {
            if (aviaryId <= 0)
            {
                return BadRequest();
            }
            var condition = _aviaryService.GetCondition(aviaryId);
            if (condition == null)
            {
                return NotFound("");
            }
            return Ok(condition);
        }

        [HttpPost("/add/condition")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> AddAviaryCondition([FromQuery] int aviaryId ,[FromBody] AviaryCondition condition)
        {
            var result = _aviaryService.AddAviaryCondition(condition, aviaryId);
            if (result != 0)
            {
                return Ok(result);
            }

            return BadRequest();
        }


        [HttpPut("/condition/change")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult ChangeAviaryCondition(AviaryCondition condition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _aviaryService.ChangeCondition(condition);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Sensor))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("sensor/{aviaryId:int}")]
        public ActionResult<Sensor> GetAviarySensor(int aviaryId)
        {
            if (aviaryId <= 0)
            {
                return BadRequest();
            }
            var sensor = _aviaryService.GetAviarySensor(aviaryId);
            if (sensor == null)
            {
                return NotFound("");
            }
            return Ok(sensor);
        }

        [HttpPost("/add/sensor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddSensor([FromBody] AddSensorDto sensorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _aviaryService.AddSensor(sensorDto);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AviaryRecharge>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("recharges/{aviaryId:int}")]
        public ActionResult<List<AviaryRecharge>> GetAviaryRecharges(int aviaryId)
        {
            if (aviaryId <= 0)
            {
                return BadRequest();
            }
            var allRecharges = _aviaryService.GetAllRecharges(aviaryId);
            if (allRecharges != null || !allRecharges.Any())
            {
                return NotFound("");
            }
            return Ok(allRecharges);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SensorData>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("sensordata/{sensorId:int}")]
        public ActionResult<List<SensorData>> GetSensorData(int sensorId)
        {
            if (sensorId <= 0)
            {
                return BadRequest();
            }
            var sensorData = _aviaryService.GetSensorData(sensorId);
            if (sensorData == null || !sensorData.Any())
            {
                return NotFound("");
            }
            return Ok(sensorData);
        }


        [HttpPost("/add/sensordata")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddSensorData([FromBody] AddSensorDataDto sensorDataDto)
        {
            var result = _aviaryService.AddSensorData(sensorDataDto);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPost("addRecharges")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddAviaryRecharges(int aviaryId, int staffId, [FromBody] List<AddAviaryRechargeDto> list)
        {
            var result = _aviaryService.AddRecharges(list, staffId, aviaryId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }



        [HttpPost("/extreme/")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult SendExtremeConditions([FromQuery] int sensorId, float ihs)
        {
            var result = _aviaryService.SendExtremeConditions(ihs, sensorId);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("time/{sensorId}")]
        public ActionResult<int> GetSensorFrequency(int sensorId)
        {
            if (sensorId <= 0)
            {
                return BadRequest();
            }
            var frequency = _aviaryService.GetSensorFrequency(sensorId);
            if (frequency == 0)
            {
                return NotFound("");
            }
            return Ok(frequency);
        }




        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("/aviaries/fill")]
        public async Task<ActionResult> FillAviariesAsync([FromBody] int[] aviariesId)
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
            foreach (var aviary in aviariesId)
            {
                var result = _aviaryService.FillAviary(aviary, staffId);

                if (!result)
                {
                    return BadRequest("Not all aviaries are updated");
                }
            }

            return Ok();
        }
    }
}

