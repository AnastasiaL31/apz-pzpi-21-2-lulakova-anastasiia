

using Microsoft.EntityFrameworkCore.Storage;
using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Interfaces
{
    public interface IAnimalService
    {
        public List<Animal> GetAll();
        public Animal? GetById(int id);
        public bool AddAnimal(AddAnimalDto animalDto);
        public bool RemoveAnimal(int id);

        public List<Treatment> GetAllTreatments(int id);
        public List<Treatment> GetDiseaseTreatments(int diseaseId);
        public bool AddTreatment(AddTreatmentDto treatmentDto);
        public bool AddDiseaseTreatment(AddTreatmentDto treatmentDto, int diseaseId);
        public bool UpdateDisease(AddDiseaseDto diseaseDto, int diseaseId);
        public bool AddDisease(AddDiseaseDto diseaseDto);
        public List<Disease> GetAnimalDiseases(int animalId);
        public List<Supply> GetTreatmentSupplies(int treatmentId);
        public bool AddTreatmentSupplies(int treatmentId, List<AddSupplyDto> supplyList);

        public List<MealPlan> GetAnimalMealPlan(int animalId);
        public bool RemoveMealPlan(int id);
        public bool AddMealPlan(AddMealPlanDto mealPlanDto);
        public bool ChangeMealPlan(MealPlan newMealPlan);

    }
}
