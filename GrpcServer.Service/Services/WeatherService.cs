using Grpc.Core;
using GrpcServer.Service.Contracts;
using GrpcServer.Service.Protos;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrpcServer.Service.Services
{
    public class WeatherService : Protos.WeatherService.WeatherServiceBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public WeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public override async Task<WeatherResponse> GetCurrentWeather(
            GetCurrentWeatherForCityRequest request,
            ServerCallContext context)
        {
            var httpclient = _httpClientFactory.CreateClient();
            string APIURL = $"https://api.openweathermap.org/data/2.5/weather?q={request.City}&<API KEY>&units={request.Units}";
            var responseText = await httpclient.GetStringAsync(APIURL);
            var temperatures = JsonSerializer.Deserialize<Temptatures>(responseText);
            return new WeatherResponse
            {
                Temperature = temperatures!.Main.Temp,
                FeelsLike = temperatures.Main.FeelsLike,
            };
            
        }
    }
}
