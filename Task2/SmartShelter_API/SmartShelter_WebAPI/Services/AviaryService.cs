using Microsoft.EntityFrameworkCore;
using SmartShelter_WebAPI.Models;

namespace SmartShelter_WebAPI.Services
{
    public class AviaryService: IAviaryService
    {
        private readonly SmartShelterDBContext _dbContext;
        public AviaryService(SmartShelterDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Aviary? GetAnimalAviary(int animalId)
        {
            var aviary = _dbContext.Aviaries.Include(x => x.AviaryCondition).FirstOrDefault(x => x.AnimalId == animalId);
            return aviary;
        }

        public List<Aviary> GetAllAviaries()
        {
            var aviaries = _dbContext.Aviaries.ToList();
            return aviaries;
        }

        public bool ChangeAviary(int animalId, int newAviaryId)
        {
            var oldAviary = _dbContext.Aviaries.FirstOrDefault(x => x.AnimalId == animalId);
            if (oldAviary != null)
            {
                oldAviary.AnimalId = null;
                _dbContext.Update(oldAviary);
            }
            var newAviary = _dbContext.Aviaries.FirstOrDefault(x => x.Id == newAviaryId);
            if (newAviary != null && newAviary.AnimalId == null)
            {
                newAviary.AnimalId = animalId;
            }
            else
            {
                return false;
            }
            return Save();
        }

        public bool AddAviary(Aviary aviary)
        {
            _dbContext.Add(aviary);
            return Save();
        }

        public bool RemoveAviary(int id)
        {
            var aviary = _dbContext.Aviaries.FirstOrDefault(x => x.Id == id);
            if (aviary != null)
            {
                _dbContext.Remove(aviary);
                return Save();
            }

            return false;
        }

        public AviaryCondition? GetCondition(int id)
        {
            var aviary = _dbContext.Aviaries.Include(x => x.AviaryCondition).FirstOrDefault(x => x.Id == id);
            if (aviary != null)
            {
                return aviary.AviaryCondition;
            }

            return null;
        }

        public Sensor? GetSensor(int id)
        {
            var sensor = _dbContext.Sensors.FirstOrDefault(x => x.AviaryId == id);
            if (sensor != null)
            {
                return sensor;
            }

            return null;
        }

        public bool AddSensor(Sensor sensor)
        {
            _dbContext.Add(sensor);
            return Save();
        }

        public List<AviaryRecharge>? GetAllRecharges(int id)
        {
            var recharges = _dbContext.AviariesRecharges.Where(x=> x.AviaryId == id).ToList();
            return recharges;
        }

        public bool AddRecharges(List<AviaryRecharge> list, int staffId, int aviaryId)
        {
            foreach (var recharge in list)
            {
                recharge.StaffId = staffId;
                recharge.AviaryId = aviaryId;
                _dbContext.Add(recharge);
            }
            return Save();
        }

        public List<SensorData>? GetSensorData(int sensorId)
        {
            var sensorData = _dbContext.SensorsData.Where(x => x.SensorId == sensorId).ToList();
            return sensorData;
        }

        public bool AddSensorData(SensorData sensorData)
        {
            _dbContext.Add(sensorData);
            return Save();
        }


        public bool Save()
        {
            return _dbContext.SaveChanges() != 0;
        }

        public bool ChangeCondition(AviaryCondition condition)
        {
            _dbContext.Update(condition);
            return Save();
        }
    }

    
}
