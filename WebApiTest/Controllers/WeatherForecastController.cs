using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using WebApiTest.Options;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/forecasts")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFeatureManager _featureManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFeatureManager featureManager)
        {
            _logger = logger;
            _featureManager = featureManager;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            if (await _featureManager.IsEnabledAsync(FeatureManagement.FeatureA))
            {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                    .ToArray();
            }

            return new List<WeatherForecast>();
        }

        [FeatureGate(FeatureManagement.FeatureA)]
        [HttpGet("id/{id}", Name = "GetWeatherForecastById")]
        public async Task<WeatherForecast> Get(int id)
        {
            return new WeatherForecast
            {
                Date = DateTime.Now.AddDays(id),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
        }

        [FeatureGate(FeatureManagement.FeatureC)]
        [HttpGet("client", Name = "GetWeatherForecastByClient")]
        public async Task<IEnumerable<WeatherForecast>> GetByClient()
        {
            return new List<WeatherForecast>();
        }
    }
}