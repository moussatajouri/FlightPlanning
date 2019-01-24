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
    public class FlightService : IFlightService
    {
        private HttpClient _httpClient;
        private HttpClientWrapper _httpClientWrapper;
        private readonly IOptions<ApiConfiguration> _apiConfiguration;

        public FlightService(HttpClient httpClient, IOptions<ApiConfiguration> apiConfiguration)
        {
            _httpClient = httpClient;
            _httpClientWrapper = new HttpClientWrapper(_httpClient);
            _apiConfiguration = apiConfiguration;
        }

        public async Task<BasicResponse<IEnumerable<Flight>>> GetAllFlights()
        {
            var uri = API.Flight.GetAllFlight(_apiConfiguration.Value.FlightApiBasePath);
            return await _httpClientWrapper.GetAsync<IEnumerable<Flight>>(uri);
        }

        public async Task<BasicResponse<string>> InsertFlight(Flight flight)
        {
            var uri = API.Flight.CrudFlight(_apiConfiguration.Value.FlightApiBasePath);

            return await _httpClientWrapper.PostAsync<string>(uri, flight);
        }

        public async Task<BasicResponse<string>> UpdateFlight(Flight flight)
        {
            var uri = API.Flight.CrudFlight(_apiConfiguration.Value.FlightApiBasePath);

            return await _httpClientWrapper.PutAsync<string>(uri, flight);
        }

        public async Task<BasicResponse<string>> DeleteFlight(int flightId)
        {
            var uri = $"{API.Flight.CrudFlight(_apiConfiguration.Value.FlightApiBasePath)}/{flightId}";

            return await _httpClientWrapper.DeleteAsync<string>(uri);
        }

        public async Task<BasicResponse<IEnumerable<DetailedFlight>>> GetAllFlightsDetails()
        {
            var uri = API.Flight.GetAllFlightsDetails(_apiConfiguration.Value.FlightApiBasePath);
            return await _httpClientWrapper.GetAsync<IEnumerable<DetailedFlight>>(uri);
        }
    }
}
