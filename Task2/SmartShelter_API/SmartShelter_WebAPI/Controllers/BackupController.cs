using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {

        private readonly SmartShelterDBContext _context;
        private readonly IConfiguration _configuration;

        public BackupController(SmartShelterDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                var backupFilePath = "C:\\Program Files\\Microsoft SQL Server\\MSSQL16.SQLEXPRESS\\MSSQL\\Backup\\backup.bak"; 
                if (!System.IO.File.Exists(backupFilePath))
                {
                    using (var fs = System.IO.File.Create(backupFilePath))
                    {
                        
                    }
                }

                await BackupDatabase(_configuration.GetConnectionString("defaultConnection"), backupFilePath);
                
                byte[] fileBytes = System.IO.File.ReadAllBytes(backupFilePath);
                
                return File(fileBytes, "application/octet-stream", "backup.bak");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating backup: {ex.Message}");
            }
        }

        private async Task BackupDatabase(string connectionString, string backupFilePath)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"BACKUP DATABASE SmartShelter TO DISK = '{backupFilePath}'";
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }

}
