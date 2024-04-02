
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartShelter_WebAPI.Models;
using System.Collections.Generic;
using Task = SmartShelter_WebAPI.Models.Task;

namespace SmartShelter_WebAPI.Data
{
    public class SmartShelterDBContext: DbContext
    {
        public SmartShelterDBContext(DbContextOptions<SmartShelterDBContext> options) : base(options)
        {
           
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Aviary> Aviaries { get; set; }
        public DbSet<AviaryCondition> AviariesConditions { get; set; }
        public DbSet<AviaryRecharge> AviariesRecharges { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorsData { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Treatment> Treatments { get; set; }

        public DbSet<DiseaseTreatments> DiseasesTreatments { get; set;}



    }
}
