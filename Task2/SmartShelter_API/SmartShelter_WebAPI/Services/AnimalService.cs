
using SmartShelter_WebAPI.Models;

namespace SmartShelter_WebAPI.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly SmartShelterDBContext _dbContext;

        public AnimalService(SmartShelterDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Animal> GetAll()
        {
            var animalList = _dbContext.Animals.ToList();
            return animalList;
        }
        public Animal? GetById(int id)
        {
            var animal = _dbContext.Animals.Find(id);
            return animal;
        }
        public bool AddAnimal(Animal animal)
        {
            _dbContext.Add(animal);
            return _dbContext.SaveChanges() != 0;
        }
        public bool RemoveAnimal(int id)
        {
            var animal = GetById(id);
            if (animal == null) return false;
            var diseasesList = GetAnimalDiseases(animal.Id);
            foreach (var disease in diseasesList)
            {
                _dbContext.Remove(disease);
            }
            _dbContext.Remove(animal);
            return _dbContext.SaveChanges() != 0;
        }



        public List<Treatment> GetAllTreatments(int id)
        {
            var diseases = _dbContext.Diseases.Where(x => x.AnimalId == id).ToList();
            var treatments = new List<Treatment>();
            foreach (var disease in diseases)
            {
                treatments.AddRange(GetDiseaseTreatments(disease.Id));
            }
            return treatments;
        }
        public bool AddTreatment(Treatment treatment)
        {
            _dbContext.Add(treatment);
            return _dbContext.SaveChanges() != 0;
        }

        public bool AddDiseaseTreatment(Treatment treatment, int diseaseId)
        {
            var addedTreatment = _dbContext.Add(treatment);
            addedTreatment.State = EntityState.Detached;
            _dbContext.Add(new DiseaseTreatments()
            {
                DiseaseId = diseaseId,
                TreatmentId = addedTreatment.Entity.Id
            });
            return _dbContext.SaveChanges() != 0;
        }

        public bool AddDisease(Disease disease)
        {
            _dbContext.Add(disease);
            return _dbContext.SaveChanges() != 0;
        }
        public List<Disease> GetAnimalDiseases(int animalId)
        {
            var diseasesList = _dbContext.Diseases.Where(x => x.AnimalId == animalId).ToList();
            return diseasesList;
        }
        public bool AddTreatmentSupplies(int treatmentId, List<Supply> supplyList)
        {
            if (supplyList.Count > 0 && treatmentId > 0)
            {
                foreach (var supply in supplyList)
                {
                    supply.TreatmentId = treatmentId;
                    _dbContext.Add(supply);
                }
                return _dbContext.SaveChanges() != 0;
            }

            return false;
        }
        public List<Treatment> GetDiseaseTreatments(int diseaseId)
        {
            var treatments = _dbContext.DiseasesTreatments
                .Where(x => x.DiseaseId == diseaseId)
                .Select(x => x.Treatment)
                .ToList();
            return treatments;
        }
        public List<Supply> GetTreatmentSupplies(int treatmentId)
        {
            var supplies = _dbContext.Supplies.Where(x => x.TreatmentId == treatmentId).ToList();
            return supplies;
        }



        public List<MealPlan> GetAnimalMealPlan(int animalId)
        {
            var mealPlans = _dbContext.MealPlans.Where(x => x.AnimalId == animalId).ToList();
            return mealPlans;

        }
        public bool RemoveMealPlan(int id)
        {
            var mealPlan = _dbContext.MealPlans.FirstOrDefault(x => x.Id == id);
            if (mealPlan != null)
            {
                _dbContext.Remove(mealPlan);
                return _dbContext.SaveChanges() != 0;
            }

            return false;
        }
        public bool AddMealPlan(MealPlan mealPlan)
        {
            _dbContext.Add(mealPlan);
            return _dbContext.SaveChanges() != 0;
        }
        public bool ChangeMealPlan(MealPlan newMealPlan)
        {
            _dbContext.Update(newMealPlan);
            return _dbContext.SaveChanges() != 0;
        }


    }
}
