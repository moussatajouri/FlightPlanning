using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlightPlanning.WebMVC.Infrastructure;
using FlightPlanning.WebMVC.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FlightPlanning.WebMVC.BusinessLogic
{
    public class AirportService : IAirportService
    {
        private HttpClient _httpClient;
        private readonly IOptions<ApiConfiguration> _apiConfiguration;

        public AirportService(HttpClient httpClient, IOptions<ApiConfiguration> apiConfiguration)
        {
            _httpClient = httpClient;
            _apiConfiguration = apiConfiguration;
        }

        public async Task<IEnumerable<Airport>> GetAllAirports()
        {
            var uri = API.Airport.GetAllAirport(_apiConfiguration.Value.AirportApiBasePath);

            var response = await _httpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                return new List<Airport>();
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<Airport>>(content);
        }

        public void InsertAirport(Airport airport)
        {
            var uri = API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath);

            var content = new StringContent(JsonConvert.SerializeObject(airport), Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(uri, content).Result;

            response.EnsureSuccessStatusCode();
        }

        public void UpdateAirport(Airport airport)
        {
            var uri = API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath);

            var content = new StringContent(JsonConvert.SerializeObject(airport), Encoding.UTF8, "application/json");
            var response = _httpClient.PutAsync(uri, content).Result;

            response.EnsureSuccessStatusCode();
        }

        public void DeleteAirport(int airportId)
        {
            var uri = $"{API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath)}/{airportId}";

            var response = _httpClient.DeleteAsync(uri).Result;

            response.EnsureSuccessStatusCode();
        }
    }
}
