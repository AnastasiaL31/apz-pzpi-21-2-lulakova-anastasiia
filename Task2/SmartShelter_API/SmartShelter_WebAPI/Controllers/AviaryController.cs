using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AviaryController : ControllerBase
    {
        private readonly IAviaryService _aviaryService;


        public AviaryController(IAviaryService aviaryService)
        {
            _aviaryService = aviaryService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Aviary>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Animal>> GetAll()
        {
            var animalList = _aviaryService.GetAllAviaries();
            if (animalList.Count == 0)
            {
                return NotFound();
            }

            return Ok(animalList);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddAviaryCondition([FromQuery] int aviaryId ,[FromBody] AviaryCondition condition)
        {
            var result = _aviaryService.AddAviaryCondition(condition, aviaryId);
            if (result)
            {
                return Ok();
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

    }
}

