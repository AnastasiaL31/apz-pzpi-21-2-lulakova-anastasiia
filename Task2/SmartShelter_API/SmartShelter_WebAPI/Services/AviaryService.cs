using Microsoft.EntityFrameworkCore;

namespace SmartShelter_WebAPI.Services
{
    public class AviaryService: IAviaryService
    {
        private readonly SmartShelterDBContext _dbContext;
        public AviaryService(SmartShelterDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Aviary? GetAviary(int id)
        {
            var aviary = _dbContext.Aviaries.Include(x => x.AviaryCondition).FirstOrDefault(x => x.AnimalId == id);
            return aviary;
        }

        public List<Aviary> GetAllAviaries()
        {
            var aviaries = _dbContext.Aviaries.ToList();
            return aviaries;
        }

        public bool ChangeAviary(int animalId, int newAviaryId)
        {
            throw new NotImplementedException();
        }

        public bool AddAviary(Aviary aviary)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAviary(int id)
        {
            throw new NotImplementedException();
        }
    }
}
