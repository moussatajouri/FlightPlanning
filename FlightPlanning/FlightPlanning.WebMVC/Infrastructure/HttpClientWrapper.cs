using FlightPlanning.WebMVC.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.Infrastructure
{
    public class HttpClientWrapper
    {
        private HttpClient _httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BasicResponse<T>> GetAsync<T>(string uri)
        {
            var apiResponse = await _httpClient.GetAsync(uri);

            var response = new BasicResponse<T>();

            if (!apiResponse.IsSuccessStatusCode)
            {
                response.Anomaly = new Anomaly { Code = "Resource Access error" };
                return response;
            }

            var content = await apiResponse.Content.ReadAsStringAsync();

            response.Data = JsonConvert.DeserializeObject<T>(content);

            if (response.Data == null)
            {
                response.Anomaly = JsonConvert.DeserializeObject<Anomaly>(content);
            }

            return response;
        }        

        public async Task<BasicResponse<T>> PostAsync<T>(string uri, object request)
        {
            var response = new BasicResponse<T>();

            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var apiResponse = _httpClient.PostAsync(uri, requestContent).Result;

            if (!apiResponse.IsSuccessStatusCode)
            {
                response.Anomaly = new Anomaly { Code = "Resource Access error" };
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            response.Anomaly = JsonConvert.DeserializeObject<Anomaly>(responseContent);

            return response;
        }

        public async Task<BasicResponse<T>> PutAsync<T>(string uri, object request)
        {
            var response = new BasicResponse<T>();
            
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var apiResponse = _httpClient.PutAsync(uri, requestContent).Result;

            if (!apiResponse.IsSuccessStatusCode)
            {
                response.Anomaly = new Anomaly { Code = "Resource Access error" };
            }

            var responseContent = await apiResponse.Content.ReadAsStringAsync();

            response.Anomaly = JsonConvert.DeserializeObject<Anomaly>(responseContent);

            return response;
        }

        public async Task<BasicResponse<T>> DeleteAsync<T>(string uri)
        {
            var response = new BasicResponse<T>();
            
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
