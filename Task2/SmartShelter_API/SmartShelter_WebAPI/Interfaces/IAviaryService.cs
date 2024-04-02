namespace SmartShelter_WebAPI.Interfaces
{
    public interface IAviaryService
    {
        public Aviary? GetAviary(int id);
        public List<Aviary> GetAllAviaries();
        public bool AddAviary(Aviary aviary);
        public bool ChangeAviary(int animalId, int newAviaryId);
        public bool RemoveAviary(int id);
    }
}
