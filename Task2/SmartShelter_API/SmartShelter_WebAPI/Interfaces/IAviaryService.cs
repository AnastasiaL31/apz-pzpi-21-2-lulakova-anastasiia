using SmartShelter_WebAPI.Dtos;

namespace SmartShelter_WebAPI.Interfaces
{
    public interface IAviaryService
    {
        public Aviary? GetAnimalAviary(int animalId);
        public List<AviaryDescription> GetAllAviaries();
        public bool AddAviary(AddAviaryDto aviaryDto);
        public bool ChangeAviary(int animalId, int newAviaryId);
        public bool RemoveAviary(int id);
        public List<int> GetFreeAviaries();
        public bool UpdateAviary(Aviary aviary);
        public AviaryCondition? GetCondition(int id);
        public int AddAviaryCondition(AviaryCondition condition, int aviaryId);
        public bool ChangeCondition(AviaryCondition condition);
        public Sensor? GetAviarySensor(int aviaryId);
        public bool UpdateAviarySensor(Sensor sensor);
        public bool RemoveAviarySensor(int sensorId);
        public bool AddSensor(AddSensorDto sensorDto);
        public List<AviaryRecharge>? GetAllRecharges(int id);
        public bool AddRecharges(List<AddAviaryRechargeDto> list, int staffId, int aviaryId);
        public List<SensorData>? GetSensorData(int sensorId);
        public bool AddSensorData(AddSensorDataDto sensorDataDto);
        bool SendExtremeConditions(float ihs, int sensorId);
        int GetSensorFrequency(int sensorId);

        bool FillAviary(int aviaryId, int staffId);
    }
}
