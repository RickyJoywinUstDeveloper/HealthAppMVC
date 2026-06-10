using HealthAppMVC.Services.Interface;
using Newtonsoft.Json;
using SharedDto.HealthRecordDtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Implementation
{
    public class HealthRecordApiService
        : IHealthRecordApiService
    {
        private readonly HttpClient _client;

        public HealthRecordApiService()
        {
            _client = new HttpClient();

            _client.BaseAddress =
                new Uri(
                    ConfigurationManager
                    .AppSettings["ApiBaseUrl"]);
        }

        public async Task<List<HealthRecordDto>>
            GetAllAsync()
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    "healthrecords");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<HealthRecordDto>>(json);
        }

        public async Task<HealthRecordDto>
            GetByIdAsync(int id)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"healthrecords/{id}");

            if (!response.IsSuccessStatusCode)
            {
                string error =
                    await response.Content
                        .ReadAsStringAsync();

                throw new Exception(error);
            }

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <HealthRecordDto>(json);
        }

        public async Task<List<HealthRecordDto>>
            GetPatientHistoryAsync(
                int patientId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"healthrecords/patient/{patientId}");

            if (!response.IsSuccessStatusCode)
            {
                string error =
                    await response.Content
                        .ReadAsStringAsync();

                throw new Exception(error);
            }

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<HealthRecordDto>>(json);
        }

        public async Task AddAsync(
            CreateHealthRecordDto dto)
        {
            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response =
                await _client.PostAsync(
                    "healthrecords",
                    content);

            response.EnsureSuccessStatusCode();
        }
    }
}