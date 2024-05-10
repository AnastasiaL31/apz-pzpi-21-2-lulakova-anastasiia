using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartShelter_WebAPI.Dtos;
using SmartShelter_WebAPI.Models;
using System.Net.Mail;
using System.Net;
using System.Linq;

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

        public List<AviaryDescription> GetAllAviaries()
        {
            List<AviaryDescription> aviaries = _dbContext.Aviaries
                .Include(x => x.Animal)
                .Include(x => x.AviaryCondition)
                .Select(x => new AviaryDescription()
                {
                    Aviary = x,
                    FoodPerDay = _dbContext.MealPlans.Where(m => m.AnimalId == x.AnimalId).Select(m => m.Amount).Sum(),
                    LastRecharge = _dbContext.AviariesRecharges.Where(r => r.AviaryId == x.Id && r.Type == "Food").Select(r => r.Date).Max(),
                    WaterNow = _dbContext.SensorsData.Where(d => d.SensorId == 
                    (_dbContext.Sensors.FirstOrDefault(s => s.AviaryId == x.Id)).Id).OrderByDescending(d => d.Date).FirstOrDefault().Water
                   
                })
                .ToList();
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

        public List<int> GetFreeAviaries()
        {
            var list = _dbContext.Aviaries.Where(x => x.AnimalId == null).ToList();
            var aviariesId = list.ConvertAll(x => x.Id);
            return aviariesId;
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

        public int AddAviaryCondition(AviaryCondition condition, int aviaryId)
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
            return addedCondition.Entity.Id;
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
                recharge.Date = DateTime.Now;
                _dbContext.Add(recharge);
                if (recharge.Type == "Water")
                {
                    var lastCondition = _dbContext.SensorsData.Where(d => d.SensorId ==
                    (_dbContext.Sensors.Where(s => s.AviaryId == aviaryId).Select(s => s.Id).FirstOrDefault()))
                        .OrderByDescending(d => d.Date).FirstOrDefault();
                    lastCondition.Water = recharge.Amount;
                    lastCondition.Id = 0;
                    lastCondition.Date = DateTime.Now;
                    _dbContext.Add(lastCondition);
                }
                _dbContext.Add(new Storage()
                {
                    Type = recharge.Type,
                    Amount = recharge.Amount * (-1),
                    Date = DateTime.Now,
                    UnitOfMeasure = "",
                    Name = recharge.Name == null ? recharge.Type : " ",
                    Price = 0,
                    StaffId = staffId
                });
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


        public string CheckIhs(float ihs)
        {
            string problem = string.Empty;
            if (ihs >= 70)
            {
                problem += "- risk of heat stress is ";
                

                if (ihs <= 79)
                {
                    problem += "moderate";
                }
                else if (ihs >= 80 && ihs < 89)
                {
                    problem += "high";
                    problem += "\nit is necessary to make temperature lower, give more water to pet";
                }
                else if (ihs >= 89)
                {
                    problem += "VERY HIGH\n";
                    problem += "YOU NEED TO HURRY UP AND HELP";
                }
            }
            return problem;
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

                problem += CheckIhs(sensorData.IHS);
                problem += CheckFood(sensorData.Food, sensorData.Date, aviary.Animal.Id);

                if (problem.Length > 0)
                {
                    problem = $"Your aviary {sensor.AviaryId} with {aviary.Animal.Name} has problems: \n" + problem;

                        

            
                        return SendEmail("n@gmail.com", problem + "\n\n" + aviaryProblem, $"Problem with pet aviary {aviary.Animal.Name}");
                    
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
            string password = "glsz imxn famy lhkh";

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


        public bool SendExtremeConditions(float ihs, int sensorId)
        {
            var sensor = _dbContext.Sensors.Where(s => s.Id == sensorId).Include(x => x.Aviary).FirstOrDefault();
            if (sensor == null)
            {
                return false; 
            }
            
            string header = $"Aviary {sensor.Aviary.Id} has high heat stress index\n";
            string message = CheckIhs(ihs);
            //var user = _context.Users.FirstOrDefault(u => u.Id == sensor.Plant.UserId);
            //if (user != null)
            //{
                return SendEmail("n@gmail.com", message, header);
            //}

            return false;
        }

        public int GetSensorFrequency(int sensorId)
        {
            var sensor = _dbContext.Sensors.FirstOrDefault(x => x.Id == sensorId);
            if (sensor == null)
            {

                return 0;
            }
            return sensor.Frequency;
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

        public bool UpdateAviary(Aviary aviary)
        {
            _dbContext.Update(aviary);
            return Save();
        }

        public bool FillAviary(int aviaryId, int staffId)
        {
            var totalMeal = _dbContext.MealPlans
                .Where(x => x.AnimalId == 
                ( _dbContext.Aviaries.Where(a => a.Id == aviaryId)
                .Select(x => x.AnimalId).FirstOrDefault())).Select(x => x.Amount).Sum();

            if(totalMeal > 0)
            {
                _dbContext.Add(new AviaryRecharge()
                {
                    Amount = totalMeal,
                    AviaryId = aviaryId,
                    StaffId = staffId,
                    Date = DateTime.Now,
                    Type = "Food"
                });

                _dbContext.Add(new Storage()
                {
                    Type = "Food",
                    Amount = totalMeal*(-1),
                    Date = DateTime.Now,
                    UnitOfMeasure = "kg",
                    Name = "Food",
                    Price = 0,
                    StaffId= staffId
                });
                return Save();
            }
            return true;
            
        }

        public bool UpdateAviarySensor(Sensor sensor)
        {
            _dbContext.Update (sensor);
            return Save();
        }

        public bool RemoveAviarySensor(int sensorId)
        {
            var sensor  = _dbContext.Sensors.FirstOrDefault(x => x.Id == sensorId);
            if(sensor != null)
            {
                _dbContext.Remove(sensor);
            }
            return Save();
        }
    }

    
}
