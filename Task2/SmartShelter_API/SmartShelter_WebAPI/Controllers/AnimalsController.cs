using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartShelter_WebAPI.Data;
using System.Data;
using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Doctor")]

    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalsController(IAnimalService animalService)
        {
         
            _animalService = animalService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type= typeof(List<Animal>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Animal>> GetAll()
        {
            var animalList = _animalService.GetAll();
            if (animalList.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(animalList);
            }
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Animal))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id:int}")]
        public ActionResult<Animal> GetAnimalById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var animal = _animalService.GetById(id);
            if (animal == null)
            {
                return NotFound("");
            }
            return Ok(animal);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddAnimal([FromBody] AddAnimalDto animalDto)
        {
            var result = _animalService.AddAnimal(animalDto);
            if (result)
            {
                return Ok();
            }
            
            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteAnimal(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var result = _animalService.RemoveAnimal(id);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("/updateAnimal/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateAnimal([FromBody] Animal animal)
        {

            var result = _animalService.UpdateAnimal(animal);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }



    }
}
