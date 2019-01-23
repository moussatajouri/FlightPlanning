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
        private HttpClientWrapper _httpClientWrapper;
        private readonly IOptions<ApiConfiguration> _apiConfiguration;

        public AirportService(HttpClient httpClient, IOptions<ApiConfiguration> apiConfiguration)
        {
            _httpClient = httpClient;
            _httpClientWrapper = new HttpClientWrapper(_httpClient);
            _apiConfiguration = apiConfiguration;
        }

        public async Task<BasicResponse<IEnumerable<Airport>>> GetAllAirports()
        {
            var uri = API.Airport.GetAllAirport(_apiConfiguration.Value.AirportApiBasePath);
            return await _httpClientWrapper.GetAsync<IEnumerable<Airport>>(uri);
        }

        public async Task<BasicResponse<string>> InsertAirport(Airport airport)
        {
            var uri = API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath);

            return await _httpClientWrapper.PostAsync<string>(uri, airport);
        }

        public async Task<BasicResponse<string>> UpdateAirport(Airport airport)
        {
            var uri = API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath);

            return await _httpClientWrapper.PutAsync<string>(uri, airport);
        }

        public async Task<BasicResponse<string>> DeleteAirport(int airportId)
        {
            var uri = $"{API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath)}/{airportId}";

            return await _httpClientWrapper.DeleteAsync<string>(uri);
        }
    }
}
