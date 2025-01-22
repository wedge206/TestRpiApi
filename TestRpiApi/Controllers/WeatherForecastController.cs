using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace TestRpiApi.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var bytes = Program.GetBytes();
            var forcastList = new List<WeatherForecast>()
            {
                new WeatherForecast()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(bytes[0])),
                    TemperatureC = bytes[0],
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                },
                new WeatherForecast()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(bytes[1])),
                    TemperatureC = bytes[1],
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                },
                new WeatherForecast()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(bytes[2])),
                    TemperatureC = bytes[2],
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                },
                new WeatherForecast()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(bytes[3])),
                    TemperatureC = bytes[3],
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                },
                new WeatherForecast()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(bytes[4])),
                    TemperatureC = bytes[4],
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                },
            };

            return forcastList;
        }
    }
}
