using HealthAppMVC.Services.Interface;
using Newtonsoft.Json;
using SharedDto.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Implementation
{
    public class AppointmentApiService
        : IAppointmentApiService
    {
        private readonly HttpClient _client;

        public AppointmentApiService()
        {
            _client = new HttpClient();

            _client.BaseAddress =
                new Uri(
                    ConfigurationManager
                    .AppSettings["ApiBaseUrl"]);
        }

        public async Task<List<AppointmentDto>>
            GetAllAppointmentsAsync()
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    "appointments");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<AppointmentDto>>(json);
        }

        public async Task<AppointmentDto>
            GetAppointmentByIdAsync(
                int id)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"appointments/{id}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <AppointmentDto>(json);
        }

        public async Task BookAppointmentAsync(
            CreateAppointmentDto dto)
        {
            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response =
                await _client.PostAsync(
                    "appointments",
                    content);

            if (!response.IsSuccessStatusCode)
            {
                string error =
                    await response.Content
                        .ReadAsStringAsync();

                throw new Exception(error);
            }
        }

        public async Task ConfirmAppointmentAsync(
            int id)
        {
            HttpResponseMessage response =
                await _client.PutAsync(
                    $"appointments/{id}/confirm",
                    null);

            response.EnsureSuccessStatusCode();
        }

        public async Task CancelAppointmentAsync(
            int id,
            CancelAppointmentDto dto)
        {
            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            HttpResponseMessage response =
                await _client.PutAsync(
                    $"appointments/{id}/cancel",
                    content);

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<AppointmentDto>>
            GetAppointmentsForPatientAsync(
                int patientId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"appointments/patient/{patientId}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>>
            GetUpcomingAppointmentsAsync()
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    "appointments/upcoming");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<AppointmentDto>>(json);
        }

        public async Task<List<AppointmentDto>>
            GetUpcomingAppointmentsByDoctorAsync(
                string doctorName)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"appointments/upcoming/doctor?doctorName={doctorName}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<AppointmentDto>>(json);
        }

        public async Task<List<string>>
            GetAvailableSlotsAsync(
                int doctorId,
                DateTime scheduledDate)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"appointments/available-slots?doctorId={doctorId}&scheduledDate={scheduledDate:yyyy-MM-dd}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<string>>(json);
        }

        public async Task<List<AppointmentDto>>
            GetAppointmentsByPatientNameAsync(
                string patientName)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"appointments/search/patient?patientName={patientName}");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject
                <List<AppointmentDto>>(json);
        }

        public async Task<bool>
            HealthRecordExistsAsync(
                int appointmentId)
        {
            HttpResponseMessage response =
                await _client.GetAsync(
                    $"appointments/{appointmentId}/healthrecord-exists");

            response.EnsureSuccessStatusCode();

            string json =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert
                .DeserializeObject<bool>(json);
        }
    }
}