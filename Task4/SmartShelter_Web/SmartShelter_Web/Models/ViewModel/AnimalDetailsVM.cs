namespace SmartShelter_Web.Models.ViewModel
{
    public class AnimalDetailsVM
    {
        public Animal Animal { get; set; }
        public List<MealPlan> Meals { get; set; }
        public MealPlan NewMealPlan { get; set; }
        public Aviary Aviary { get; set; }
        public AviaryCondition AviaryCondition { 
            get{
                if(Aviary == null || Aviary.AviaryCondition == null)
                {
                    return new AviaryCondition();
                }
                return Aviary.AviaryCondition;
            }
            set
            {
                Aviary.AviaryCondition = value;
            }
        }

        public List<int> FreeAviaries { get; set; }
    }
}
