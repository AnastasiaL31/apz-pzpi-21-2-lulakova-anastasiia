

using Microsoft.EntityFrameworkCore.Storage;

namespace SmartShelter_WebAPI.Interfaces
{
    public interface IAnimalService
    {
        public List<Animal> GetAll();
        public Animal? GetById(int id);
        public bool AddAnimal(Animal animal);
        public bool RemoveAnimal(int id);

        public List<Treatment> GetAllTreatments(int id);
        public List<Treatment> GetDiseaseTreatments(int diseaseId);
        public bool AddTreatment(Treatment treatment);
        public bool AddDiseaseTreatment(Treatment treatment, int diseaseId);
        public bool AddDisease(Disease disease);
        public List<Disease> GetAnimalDiseases(int animalId);
        public List<Supply> GetTreatmentSupplies(int treatmentId);
        public bool AddTreatmentSupplies(int treatmentId, List<Supply> supplyList);


        public List<MealPlan> GetAnimalMealPlan(int animalId);
        public bool RemoveMealPlan(int id);
        public bool AddMealPlan(MealPlan mealPlan);
        public bool ChangeMealPlan(MealPlan newMealPlan);

    }
}
