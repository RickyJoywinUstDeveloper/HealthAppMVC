using SharedDto.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebApi.Services.Interface
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>>
            GetAllAppointmentsAsync();

        Task<AppointmentDto>
            GetAppointmentByIdAsync(
                int id);

        Task BookAppointmentAsync(
            CreateAppointmentDto dto);

        Task ConfirmAppointmentAsync(
            int id);

        Task CancelAppointmentAsync(
            int id,
            string reason);

        Task<List<AppointmentDto>>
            GetAppointmentsForPatientAsync(
                int patientId);

        Task<List<AppointmentDto>>
            GetUpcomingAppointmentsAsync();

        Task<List<AppointmentDto>>
            GetUpcomingAppointmentsByDoctorAsync(
                string doctorName);

        Task<List<string>>
            GetAvailableSlotsAsync(
                int doctorId,
                DateTime scheduledDate);

        Task<List<AppointmentDto>>
            GetAppointmentsByPatientNameAsync(
                string patientName);

        Task<bool>
            HealthRecordExistsAsync(
                int appointmentId);
    }
}