namespace SmartShelter_WebAPI.Interfaces
{
    public interface IAviaryService
    {
        public Aviary? GetAnimalAviary(int animalId);
        public List<Aviary> GetAllAviaries();
        public bool AddAviary(Aviary aviary);
        public bool ChangeAviary(int animalId, int newAviaryId);
        public bool RemoveAviary(int id);
        public AviaryCondition? GetCondition(int id);
        public bool ChangeCondition(AviaryCondition condition);
        public Sensor? GetSensor(int id);
        public bool AddSensor(Sensor sensor);
        public List<AviaryRecharge>? GetAllRecharges(int id);
        public bool AddRecharges(List<AviaryRecharge> list, int staffId, int aviaryId);
        public List<SensorData>? GetSensorData(int sensorId);
        public bool AddSensorData(SensorData sensorData);


    }
}
