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
    public class AircraftService : IAircraftService
    {
        private HttpClient _httpClient;
        private HttpClientWrapper _httpClientWrapper;
        private readonly IOptions<ApiConfiguration> _apiConfiguration;

        public AircraftService(HttpClient httpClient, IOptions<ApiConfiguration> apiConfiguration)
        {
            _httpClient = httpClient;
            _httpClientWrapper = new HttpClientWrapper(_httpClient);
            _apiConfiguration = apiConfiguration;
        }

        public async Task<BasicResponse<IEnumerable<Aircraft>>> GetAllAircrafts()
        {
            var uri = API.Aircraft.GetAllAircraft(_apiConfiguration.Value.AircraftApiBasePath);
            return await _httpClientWrapper.GetAsync<IEnumerable<Aircraft>>(uri);
        }

        public async Task<BasicResponse<string>> InsertAircraft(Aircraft aircraft)
        {
            var uri = API.Aircraft.CrudAircraft(_apiConfiguration.Value.AircraftApiBasePath);

            return await _httpClientWrapper.PostAsync<string>(uri, aircraft);
        }

        public async Task<BasicResponse<string>> UpdateAircraft(Aircraft aircraft)
        {
            var uri = API.Aircraft.CrudAircraft(_apiConfiguration.Value.AircraftApiBasePath);

            return await _httpClientWrapper.PutAsync<string>(uri, aircraft);
        }

        public async Task<BasicResponse<string>> DeleteAircraft(int aircraftId)
        {
            var uri = $"{API.Aircraft.CrudAircraft(_apiConfiguration.Value.AircraftApiBasePath)}/{aircraftId}";

            return await _httpClientWrapper.DeleteAsync<string>(uri);
        }
    }
}
