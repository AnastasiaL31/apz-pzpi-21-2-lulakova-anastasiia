
using AutoMapper;
using SmartShelter_WebAPI.Dtos;
using SmartShelter_WebAPI.Models;

namespace SmartShelter_WebAPI.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly SmartShelterDBContext _dbContext;
        private readonly IMapper _mapper;

        public AnimalService(SmartShelterDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
        public bool AddAnimal(AddAnimalDto animalDto)
        {
            var animal = _mapper.Map<Animal>(animalDto);
            animal.AcceptanceDate = DateTime.Now;
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

        public List<TreatmentWithStaff> GetAllTreatments(int id)
        {
            var diseases = _dbContext.Diseases.Where(x => x.AnimalId == id).ToList();
            var treatments = new List<TreatmentWithStaff>();
            foreach (var disease in diseases)
            {
                treatments.AddRange(GetDiseaseTreatments(disease.Id));
            }
            return treatments;
        }
        public bool AddTreatment(AddTreatmentDto treatmentDto)
        {
            var treatment = _mapper.Map<Treatment>(treatmentDto);
            treatment.Date = DateTime.Now;

            _dbContext.Add(treatment);
            return _dbContext.SaveChanges() != 0;
        }

        public bool AddDiseaseTreatment(AddTreatmentDto treatmentDto, int diseaseId)
        {
            var treatment = _mapper.Map<Treatment>(treatmentDto);
            treatment.Date = DateTime.Now;
            var addedTreatment = _dbContext.Add(treatment);
            _dbContext.SaveChanges();
            addedTreatment.State = EntityState.Detached;
            _dbContext.Add(new DiseaseTreatments()
            {
                DiseaseId = diseaseId,
                TreatmentId = addedTreatment.Entity.Id
            });
            return _dbContext.SaveChanges() != 0;
        }

        public bool UpdateDisease(Disease disease)
        {
            _dbContext.Update(disease);
            return _dbContext.SaveChanges() != 0;
        }

        public bool AddDisease(AddDiseaseDto diseaseDto)
        {
            var disease = _mapper.Map<Disease>(diseaseDto);
            _dbContext.Add(disease);
            return _dbContext.SaveChanges() != 0;
        }
        public List<Disease> GetAnimalDiseases(int animalId)
        {
            var diseasesList = _dbContext.Diseases.Where(x => x.AnimalId == animalId).ToList();
            return diseasesList;
        }
        public bool AddTreatmentSupplies(int treatmentId, List<AddSupplyDto> supplyList)
        {
            if (supplyList.Count > 0 && treatmentId > 0)
            {
                foreach (var supply in _mapper.Map<List<Supply>>(supplyList))
                {
                    supply.TreatmentId = treatmentId;
                    _dbContext.Add(supply);
                }
                return _dbContext.SaveChanges() != 0;
            }

            return false;
        }
        public List<TreatmentWithStaff> GetDiseaseTreatments(int diseaseId)
        {
            var treatments = _dbContext.DiseasesTreatments
                .Where(x => x.DiseaseId == diseaseId)
                .Select(ts => new TreatmentWithStaff()
                {
                    Treatment = ts.Treatment,
                    StaffName = ts.Treatment.Staff.Name
                })
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
        public bool AddMealPlan(AddMealPlanDto mealPlanDto)
        {
            var mealPlan = _mapper.Map<MealPlan>(mealPlanDto);
            _dbContext.Add(mealPlan);
            return _dbContext.SaveChanges() != 0;
        }
        public bool ChangeMealPlan(MealPlan newMealPlan)
        {
            _dbContext.Update(newMealPlan);
            return _dbContext.SaveChanges() != 0;
        }

        public bool UpdateAnimal(Animal animal)
        {
            _dbContext.Update(animal);
            return _dbContext.SaveChanges() != 0;
        }

        public Disease? GetDisease(int diseaseId)
        {
            var disease = _dbContext.Diseases.FirstOrDefault(x => x.Id.Equals(diseaseId));
            return disease;

        }
    }
}
