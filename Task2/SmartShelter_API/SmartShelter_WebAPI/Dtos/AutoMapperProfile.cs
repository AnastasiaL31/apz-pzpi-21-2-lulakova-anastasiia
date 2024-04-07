using AutoMapper;

namespace SmartShelter_WebAPI.Dtos
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddAnimalDto, Animal>();
            CreateMap<AddDiseaseDto, Disease>();
            CreateMap<AddMealPlanDto, MealPlan>();
            CreateMap<AddSupplyDto, Supply>();
            CreateMap<AddTreatmentDto, Treatment>();
            CreateMap<AddAviaryDto, Aviary>();
            CreateMap<AddAviaryRechargeDto, AviaryRecharge>();
            CreateMap<AddSensorDto, Sensor>();
            CreateMap<AddSensorDataDto, SensorData>();
            CreateMap<AddStaffDto, Staff>();
            CreateMap<StaffDto, Staff>();
            CreateMap<Staff, StaffDto>();
            CreateMap<AddStaffTaskDto, StaffTask>();
            CreateMap<GetStaffTaskDto, StaffTask>();
            CreateMap<StaffTask, GetStaffTaskDto>();
            CreateMap<OrderDto, Order>();
            //CreateMap();
            //CreateMap<>();

        }
    }
}
