﻿using System;
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

        public async Task<BasicResponse<IEnumerable<Airport>>> GetAllAirports()
        {
            var response = new BasicResponse<IEnumerable<Airport>>();

            var uri = API.Airport.GetAllAirport(_apiConfiguration.Value.AirportApiBasePath);

            var apiResponse = await _httpClient.GetAsync(uri);
            if (!apiResponse.IsSuccessStatusCode)
            {
                response.Anomaly = new Anomaly { Code = "Resource Access error" };
                return response;
            }

            var content = await apiResponse.Content.ReadAsStringAsync();

            response.Data =  JsonConvert.DeserializeObject<IEnumerable<Airport>>(content);

            if (response.Data == null)
            {
                response.Anomaly = JsonConvert.DeserializeObject<Anomaly>(content);
            }

            return response;
        }

        public async Task<BasicResponse<string>> InsertAirport(Airport airport)
        {
            var response = new BasicResponse<string>();

            var uri = API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath);

            var requestContent = new StringContent(JsonConvert.SerializeObject(airport), Encoding.UTF8, "application/json");
            var apiResponse = _httpClient.PostAsync(uri, requestContent).Result;

            if (!apiResponse.IsSuccessStatusCode)
            {
                response.Anomaly = new Anomaly { Code = "Resource Access error" };
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            response.Anomaly = JsonConvert.DeserializeObject<Anomaly>(responseContent);

            return response;
        }

        public async Task<BasicResponse<string>> UpdateAirport(Airport airport)
        {
            var response = new BasicResponse<string>();

            var uri = API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath);

            var requestContent = new StringContent(JsonConvert.SerializeObject(airport), Encoding.UTF8, "application/json");
            var apiResponse = _httpClient.PutAsync(uri, requestContent).Result;

            if (!apiResponse.IsSuccessStatusCode)
            {
                response.Anomaly = new Anomaly { Code = "Resource Access error" };
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            response.Anomaly = JsonConvert.DeserializeObject<Anomaly>(responseContent);

            return response;
        }

        public async Task<BasicResponse<string>> DeleteAirport(int airportId)
        {
            var response = new BasicResponse<string>();

            var uri = $"{API.Airport.CrudAirport(_apiConfiguration.Value.AirportApiBasePath)}/{airportId}";

            var apiResponse = _httpClient.DeleteAsync(uri).Result;

            if (!apiResponse.IsSuccessStatusCode)
            {
                response.Anomaly = new Anomaly { Code = "Resource Access error" };
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            response.Anomaly = JsonConvert.DeserializeObject<Anomaly>(responseContent);

            return response;
        }
    }
}
