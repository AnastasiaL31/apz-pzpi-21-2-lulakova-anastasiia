using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {

        private readonly IAnimalService _animalService;
        public MealsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MealPlan>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/animalMeal/{animalId:int}")]
        public ActionResult<List<MealPlan>> GetAnimalMealPlan(int animalId)
        {
            var mealPlanList = _animalService.GetAnimalMealPlan(animalId);
            if (mealPlanList.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(mealPlanList);
            }
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteMealPlan(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var result = _animalService.RemoveMealPlan(id);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("/addMealPlan/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddMealPlan([FromBody] AddMealPlanDto mealPlanDto)
        {
            
            var result = _animalService.AddMealPlan(mealPlanDto);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("/updateMealPlan/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateMealPlan([FromBody] MealPlan mealPlan)
        {

            var result = _animalService.ChangeMealPlan(mealPlan);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
