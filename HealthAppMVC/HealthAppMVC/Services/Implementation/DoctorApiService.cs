using HealthAppMVC.Services.Interface;
using Newtonsoft.Json;
using SharedDto.DoctorDtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Implementation
{
    public class DoctorApiService
        : IDoctorApiService
    {
        private readonly HttpClient _client;

        public DoctorApiService()
        {
            _client = new HttpClient();

            _client.BaseAddress =
                new Uri(
                    ConfigurationManager
                    .AppSettings["ApiBaseUrl"]);
        }

        public async Task<List<DoctorDto>>
            GetAllDoctorsAsync()
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    "doctors");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<DoctorDto>>(json);
        }

        public async Task<DoctorDto>
            GetDoctorByIdAsync(int id)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"doctors/{id}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <DoctorDto>(json);
        }

        public async Task AddDoctorAsync(
            CreateDoctorDto dto)
        {
            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response =
                await _client.PostAsync(
                    "doctors",
                    content);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateDoctorAsync(
            int id,
            CreateDoctorDto dto)
        {
            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response =
                await _client.PutAsync(
                    $"doctors/{id}",
                    content);

            response.EnsureSuccessStatusCode();
        }

        public async Task ChangeStatusAsync(
     int id,
     bool isActive)
        {
            var request =
                new HttpRequestMessage(
                    new HttpMethod("PATCH"),
                    $"doctors/{id}/status?isActive={isActive}");

            HttpResponseMessage response =
                await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<DoctorDto>>
            GetDoctorsBySpecialisationAsync(
                string specialisation)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"doctors/specialisation/{specialisation}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<DoctorDto>>(json);
        }

        public async Task<List<DoctorDto>>
            SearchByNameAsync(
                string name)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"doctors/search?name={name}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<DoctorDto>>(json);
        }
    }

}