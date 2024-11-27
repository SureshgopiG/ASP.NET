using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly string API_KEY = "e754abdbd956545bfceddaeade78fedb";

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest(new { error = "City name is required" });
            }

            var apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_KEY}&units=metric";

            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound(new { error = "City not found" });
                    }

                    return StatusCode(500, new { error = "Error fetching weather data" });
                }

                var responseData = await response.Content.ReadAsStringAsync();
                var weatherData = JsonDocument.Parse(responseData).RootElement;

                var weatherResponse = new WeatherResponse
                {
                    City = weatherData.GetProperty("name").GetString(),
                    Temperature = weatherData.GetProperty("main").GetProperty("temp").GetDouble(),
                    Weather = weatherData.GetProperty("weather")[0].GetProperty("description").GetString()
                };

                return Ok(weatherResponse);
            }
            catch
            {
                return StatusCode(500, new { error = "Error fetching weather data" });
            }
        }
    }
}
