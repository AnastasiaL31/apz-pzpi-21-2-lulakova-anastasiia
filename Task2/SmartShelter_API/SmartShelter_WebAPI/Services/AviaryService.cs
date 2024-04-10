using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartShelter_WebAPI.Dtos;
using SmartShelter_WebAPI.Models;
using System.Net.Mail;
using System.Net;

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
            CheckConditions(sensorData);
            _dbContext.Add(sensorData);
            return Save();
        }

        public string? CheckFood(float food, DateTime time, int animalId)
        {
            var meals = _dbContext.MealPlans.Where(x => x.AnimalId == animalId).ToList();
            if (meals.Count > 0)
            {
                string problem = "";
                var max = meals.MaxBy(x => x.Amount);
                if (max.Amount < food * 1.5)
                {
                    problem += "Pet doesn't eat food. You need to check it";
                    return problem;
                }

                foreach (var meal in meals)
                {
                    if (meal.Time.TimeOfDay.Add(new TimeSpan(0, 0, 5, 0)) >= time.TimeOfDay 
                        && meal.Time.TimeOfDay > time.TimeOfDay)
                    {
                        if (food < meal.Amount * 0.75)
                        {
                            problem += "Check mechanism of adding food";
                            return problem;
                        }
                    }
                }
            }

            return null;
        }

        public bool CheckConditions(SensorData sensorData)
        {
            var sensor = _dbContext.Sensors.FirstOrDefault(s => s.Id == sensorData.SensorId);
            if (sensor == null)
            {
                return false;
            }

            var aviary = _dbContext.Aviaries.Include(x => x.AviaryCondition).Include(x => x.Animal).FirstOrDefault(x => x.Id == sensor.AviaryId);
            
            if (aviary != null && aviary.AviaryConditionId != null)
            {
                if (aviary.AnimalId == null)
                {
                    return false;
                }
                string aviaryProblem = "";
                string problem = "";
                if (sensorData.Temperature > aviary.AviaryCondition.MaxTemperature)
                {
                    problem += $" - Temperature  is equal to {sensorData.Temperature} and higher then needed ({aviary.AviaryCondition.MaxTemperature})\n";
                    aviaryProblem += "\nDecrease temperature in " +
                                    (sensorData.Temperature - (aviary.AviaryCondition.MaxTemperature - aviary.AviaryCondition.MinTemperature) / 2);
                }
                else if (sensorData.Temperature < aviary.AviaryCondition.MinTemperature)
                {
                    problem += $" - Temperature  is equal to {sensorData.Temperature} and lower then needed ({aviary.AviaryCondition.MinTemperature})\n";
                    aviaryProblem += "\nIncrease temperature in " +
                                    ((aviary.AviaryCondition.MaxTemperature - aviary.AviaryCondition.MinTemperature) / 2 - sensorData.Temperature);
                }

                if (sensorData.Humidity > aviary.AviaryCondition.MaxHumidity)
                {
                    problem += $" - Humidity is equal to {sensorData.Humidity} and higher then needed ({aviary.AviaryCondition.MaxHumidity})\n";
                    aviaryProblem += "\nit is necessary to increase the air humidifier setting by" +
                                    (aviary.AviaryCondition.MaxHumidity - aviary.AviaryCondition.MaxHumidity) / 2;
                }
                else if (sensorData.Humidity < aviary.AviaryCondition.MinHumidity)
                {
                    problem += $" - Humidity  is equal to {sensorData.Humidity} and lower then needed ({aviary.AviaryCondition.MinHumidity})\n";
                    aviaryProblem += "\nit is necessary to decrease the air humidifier setting by" +
                                    (aviary.AviaryCondition.MaxHumidity - aviary.AviaryCondition.MaxHumidity) / 2;
                }

                var tenPercent = sensorData.Water / 10;
                if (sensorData.Water + tenPercent <= aviary.AviaryCondition.MinWater)
                {
                    problem += $" - Water level is low and equal to {sensorData.Water} \n";
                    aviaryProblem += "\nit is necessary to add more water, at least " + tenPercent;
                }

                if (sensorData.IHS >= 70)
                {
                    problem += "- risk of heat stress is ";
                    aviaryProblem += "\nit is necessary to make temperature lower, give more water to pet";

                    if (sensorData.IHS <= 79)
                    {
                        problem += "moderate";
                    }else if (sensorData.IHS >= 80 && sensorData.IHS < 89)
                    {
                        problem += "high";
                    }
                    else if (sensorData.IHS >= 89)
                    {
                        problem += "VERY HIGH";
                        aviaryProblem += "YOU NEED TO HURRY UP AND HELP";
                    }
                }

                problem += CheckFood(sensorData.Food, sensorData.Date, aviary.Animal.Id);

                if (problem.Length > 0)
                {
                    problem = $"Your aviary {sensor.AviaryId} with {aviary.Animal.Name} has problems: \n" + problem;

                    //var user = _dbContext.Users.FirstOrDefault(u => u.Id = id);
                    //if (user != null)
                    //{
                        return SendEmail("n@gmail.com", problem + "\n\n" + aviaryProblem, $"Problem with pet aviary {aviary.Animal.Name}");
                    //}
                }

                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool SendEmail(string toEmail, string message, string header)
        {
            string fromEmail = "anastasiia.lulakova@nure.ua";
            string password = "";

            MailAddress from = new MailAddress(fromEmail);
            MailAddress to = new MailAddress(toEmail);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage(from, to)
            {
                Subject = header,
                Body = message,
            };

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }


        //public bool SendExtremeConditions(int sensorId, float temperature, float dewPoint)
        //{
        //    Sensor sensor = _context.Sensors.Where(s => s.Id == sensorId).Include(s => s.Plant).FirstOrDefault();
        //    if (sensor == null)
        //    {
        //        return false;
        //    }

        //    if (sensor.Plant == null)
        //    {
        //        return false;
        //    }
        //    string header = $"Plant {sensor.Plant.Name} close to condensation conditions";
        //    string message =
        //        $"Your plant {sensor.Plant.Name} has air temperature equal to {temperature}C which is close to dew point {dewPoint}C. Please, pay attention to this.";
        //    var user = _context.Users.FirstOrDefault(u => u.Id == sensor.Plant.UserId);
        //    if (user != null)
        //    {
        //        return SendEmail(user.Email, message, header);
        //    }

        //    return false;
        //}


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
