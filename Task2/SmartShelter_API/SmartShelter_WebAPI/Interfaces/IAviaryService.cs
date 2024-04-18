using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Interfaces
{
    public interface IAviaryService
    {
        public Aviary? GetAnimalAviary(int animalId);
        public List<Aviary> GetAllAviaries();
        public bool AddAviary(AddAviaryDto aviaryDto);
        public bool ChangeAviary(int animalId, int newAviaryId);
        public bool RemoveAviary(int id);
        public AviaryCondition? GetCondition(int id);
        public bool AddAviaryCondition(AviaryCondition condition, int aviaryId);
        public bool ChangeCondition(AviaryCondition condition);
        public Sensor? GetAviarySensor(int aviaryId);
        public bool AddSensor(AddSensorDto sensorDto);
        public List<AviaryRecharge>? GetAllRecharges(int id);
        public bool AddRecharges(List<AddAviaryRechargeDto> list, int staffId, int aviaryId);
        public List<SensorData>? GetSensorData(int sensorId);
        public bool AddSensorData(AddSensorDataDto sensorDataDto);
        bool SendExtremeConditions(float ihs, int sensorId);
        int GetSensorFrequency(int sensorId);
    }
}
