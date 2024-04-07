using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartShelter_WebAPI.Dtos;
using SmartShelter_WebAPI.Models;

namespace SmartShelter_WebAPI.Services
{
    public class AviaryService: IAviaryService
    {
        private readonly SmartShelterDBContext _dbContext;
        private readonly IMapper _mapper;

        public AviaryService(SmartShelterDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

        public bool AddAviary(AddAviaryDto aviaryDto)
        {
            var aviary = _mapper.Map<Aviary>(aviaryDto);
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

        public bool AddAviaryCondition(AviaryCondition condition, int aviaryId)
        {
            var addedCondition = _dbContext.Add(condition);
            _dbContext.SaveChanges();
            //addedCondition.State = EntityState.Detached;
            var aviary = _dbContext.Aviaries.FirstOrDefault(x => x.Id == aviaryId);
            if (aviary != null && aviary.AviaryConditionId == null)
            {
                aviary.AviaryConditionId = addedCondition.Entity.Id;
                _dbContext.Update(aviary);
            }
            return _dbContext.SaveChanges() != 0;
        }

        public Sensor? GetAviarySensor(int aviaryId)
        {
            var sensor = _dbContext.Sensors.FirstOrDefault(x => x.AviaryId == aviaryId);
            if (sensor != null)
            {
                return sensor;
            }

            return null;
        }

        public bool AddSensor(AddSensorDto sensorDto)
        {
            var sensor = _mapper.Map<Sensor>(sensorDto);
            _dbContext.Add(sensor);
            return Save();
        }

        public List<AviaryRecharge>? GetAllRecharges(int id)
        {
            var recharges = _dbContext.AviariesRecharges.Where(x=> x.AviaryId == id).ToList();
            return recharges;
        }

        public bool AddRecharges(List<AddAviaryRechargeDto> list, int staffId, int aviaryId)
        {
            foreach (var recharge in _mapper.Map<List<AviaryRecharge>>(list))
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

        public bool AddSensorData(AddSensorDataDto sensorDataDto)
        {
            var sensorData = _mapper.Map<SensorData>(sensorDataDto);
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
