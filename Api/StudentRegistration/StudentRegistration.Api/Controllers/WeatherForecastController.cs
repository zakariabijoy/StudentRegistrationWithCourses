using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentRegistration.DataAccess.Repository.IRepository;
using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICourseRepository _courseRepo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICourseRepository courseRepo)
        {
            _logger = logger;
            _courseRepo = courseRepo;
        }

        [HttpGet]
        public IEnumerable<Course> Get()
        {
          return  _courseRepo.GetAll();
        }
    }
}
