using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Doctor")]
    [ApiController]
    public class MedicalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly IStaffService _staffService;

        public MedicalController(IAnimalService animalService, IStaffService service)
        {
            _animalService = animalService;
            _staffService = service;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TreatmentWithStaff>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/treatments/{id:int}")]
        public ActionResult<List<TreatmentWithStaff>> GetAnimalTreatments(int id)
        {
            var allTreatments = _animalService.GetAllTreatments(id);
            if (allTreatments.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(allTreatments);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Disease))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/disease/{id:int}")]
        public ActionResult<Disease> GetDisease(int id)
        {
            var disease = _animalService.GetDisease(id);
            if (disease == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(disease);
            }
        }

        [HttpPost]
        [Route("/addTreatment/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddTreatment([FromBody] AddTreatmentDto treatmentDto, int? diseaseId)
        {
            var result = false;
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest();
            }
            else
            {
                treatmentDto.StaffId = await _staffService.GetStaffId(userName);
            }

            if (diseaseId != null && diseaseId > 0)
            {
                result = _animalService.AddDiseaseTreatment(treatmentDto, (int)diseaseId);
            }
            else
            {
                result = _animalService.AddTreatment(treatmentDto);
            }

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("/addSupplies/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddTreatmentSupplies([FromQuery]int treatmentId, [FromBody] List<AddSupplyDto> supplyList)
        {
            if (treatmentId <= 0 || !supplyList.Any())
            {
                return BadRequest();
            }
            var result = _animalService.AddTreatmentSupplies(treatmentId, supplyList);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("/addDisease/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddDisease([FromBody] AddDiseaseDto diseaseDto)
        {
            var result = _animalService.AddDisease(diseaseDto);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        //[HttpPut]
        //[Route("/updateDisease/")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult UpdateDisease(int diseaseId, [FromBody] AddDiseaseDto diseaseDto)
        //{
        //    var result = _animalService.UpdateDisease(diseaseDto, diseaseId);
        //    if (result)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest();
        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TreatmentWithStaff>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/treatment/disease/{diseaseId:int}")]
        public ActionResult<List<TreatmentWithStaff>> GetDiseaseTreatments(int diseaseId)
        {
            var diseaseTreatments = _animalService.GetDiseaseTreatments(diseaseId);
            if (diseaseTreatments.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(diseaseTreatments);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Supply>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/treatment/{treatmentId:int}/supplies")]
        public ActionResult<List<Supply>> GetTreatmentSupplies(int treatmentId)
        {
            var treatmentSupplies = _animalService.GetTreatmentSupplies(treatmentId);
            if (treatmentSupplies.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(treatmentSupplies);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Disease>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/animal/{animalId:int}/diseases")]
        public ActionResult<List<Disease>> GetAnimalDiseases(int animalId)
        {
            var diseasesList = _animalService.GetAnimalDiseases(animalId);
            if (diseasesList.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(diseasesList);
            }
        }

        [HttpPut]
        [Route("/updateDisease/group")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateMultipleDiseases([FromBody] List<Disease> diseases)
        {
            var result = false;
            foreach (var disease in diseases)
            {
                result = _animalService.UpdateDisease(disease);
                if (!result)
                {
                    return BadRequest("Cannot update all diseases");
                }
            }
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
