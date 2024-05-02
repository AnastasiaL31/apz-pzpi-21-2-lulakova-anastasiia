
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

        public List<TreatmentWithStaff> GetOtherTreatments(int id)
        {
            var treatments = _dbContext.Treatments
                .Where(x => x.AnimalId == id && (_dbContext.DiseasesTreatments.FirstOrDefault(y => y.TreatmentId == x.Id) == null))
                .Select(ts => new TreatmentWithStaff()
                {
                    Treatment = ts,
                    StaffName = ts.Staff.Name
                })
                .ToList();
            
            return treatments;
        }
        public int AddTreatment(AddTreatmentDto treatmentDto)
        {
            var treatment = _mapper.Map<Treatment>(treatmentDto);
            treatment.Date = DateTime.Now;

            var treatmentEntity = _dbContext.Add(treatment);
            _dbContext.SaveChanges();
            return treatmentEntity.Entity.Id;
        }

        public int AddDiseaseTreatment(AddTreatmentDto treatmentDto, int diseaseId)
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
            _dbContext.SaveChanges();
            return addedTreatment.Entity.Id;
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

        public bool DeleteTreatment(int id)
        {
            var treatment = _dbContext.Treatments.FirstOrDefault(x => x.Id == id);
            if(treatment != null)
            {
                _dbContext.Remove(treatment);
                return _dbContext.SaveChanges() != 0;
            }
            return false;
        }

        public bool DeleteSupply(int id)
        {
            var supply = _dbContext.Supplies.FirstOrDefault(x => x.Id == id);
            if (supply != null)
            {
                _dbContext.Remove(supply);
                return _dbContext.SaveChanges() != 0;
            }
            return false;
        }
    }
}
