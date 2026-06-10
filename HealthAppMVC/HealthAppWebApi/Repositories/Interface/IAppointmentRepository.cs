using HealthAppWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebApi.Repositories.Interface
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAsync();

        Task<Appointment> GetByIdAsync(int id);

        Task AddAsync(Appointment appointment);

        Task UpdateAsync(Appointment appointment);

        Task<bool> IsDoctorSlotBookedAsync(
            int doctorId,
            DateTime date,
            string slot);

        Task<bool> HasPatientSlotConflictAsync(
            int patientId,
            DateTime date,
            string slot);

        Task<bool>
            HasAppointmentWithDoctorOnSameDayAsync(
                int patientId,
                int doctorId,
                DateTime date);

        Task<List<Appointment>>
            GetUpcomingConfirmedAppointmentsByDoctorAsync(
                int doctorId);

        Task<List<Appointment>>
            GetAppointmentsByPatientAsync(
                int patientId);

        Task<bool>
            HealthRecordExistsAsync(
                int appointmentId);

        Task<List<Appointment>>
            GetUpcomingAppointmentsAsync();

        Task<List<Appointment>>
            GetUpcomingAppointmentsByDoctorAsync(
                string doctorName);

        Task<List<string>>
            GetAvailableSlotsAsync(
                int doctorId,
                DateTime scheduledDate);

        Task<List<Appointment>>
            GetAppointmentsByPatientNameAsync(
                string patientName);
    }
}