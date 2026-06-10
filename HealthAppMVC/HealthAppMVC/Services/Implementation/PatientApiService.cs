using HealthAppMVC.Services.Interface;
using Newtonsoft.Json;
using SharedDto.PatientDtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Implementation
{
    public class PatientApiService
        : IPatientApiService
    {
        private readonly HttpClient _client;

        public PatientApiService()
        {
            _client = new HttpClient();

            _client.BaseAddress =
                new Uri(
                    ConfigurationManager
                    .AppSettings["ApiBaseUrl"]);
        }

        public async Task<List<PatientDto>>
            GetAllPatientsAsync()
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    "patients");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<PatientDto>>(json);
        }

        public async Task<PatientDto>
            GetPatientByIdAsync(int id)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"patients/{id}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <PatientDto>(json);
        }

        public async Task CreatePatientAsync(
            CreatePatientDto dto)
        {
            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response =
                await _client.PostAsync(
                    "patients",
                    content);

            if (!response.IsSuccessStatusCode)
            {
                string error =
                    await response.Content
                        .ReadAsStringAsync();

                throw new Exception(error);
            }

        }

        public async Task UpdatePatientAsync(
            int id,
            CreatePatientDto dto)
        {
            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response =
                await _client.PutAsync(
                    $"patients/{id}",
                    content);

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<PatientDto>>
            SearchByNameAsync(
                string name)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"patients/search?name={name}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<PatientDto>>(json);
        }

        public async Task<int>
            GetAppointmentCountAsync(
                int patientId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"patients/{patientId}/appointmentcount");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject<int>(json);
        }
    }
}