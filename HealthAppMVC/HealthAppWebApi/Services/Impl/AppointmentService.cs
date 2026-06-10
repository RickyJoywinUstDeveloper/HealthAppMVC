using HealthAppWebApi.Constants;
using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Interface;
using SharedDto.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebApi.Services.Impl
{
    public class AppointmentService
        : IAppointmentService
    {
        private readonly
            IAppointmentRepository _repo;

        public AppointmentService(
            IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AppointmentDto>>
            GetAllAppointmentsAsync()
        {
            var appointments =
                await _repo.GetAllAsync();

            return appointments
                .Select(a => new AppointmentDto
                {
                    AppointmentId =
                        a.AppointmentId,

                    PatientName =
                        a.Patient.FullName,

                    DoctorName =
                        a.Doctor.FullName,

                    ScheduledDate =
                        a.ScheduledDate,

                    TimeSlot =
                        a.TimeSlot,

                    Status =
                        a.Status.ToString(),

                    CancellationReason =
                        a.CancellationReason
                })
                .ToList();
        }

        public async Task<AppointmentDto>
            GetAppointmentByIdAsync(
                int id)
        {
            Appointment appointment =
                await _repo.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            return new AppointmentDto
            {
                AppointmentId =
                    appointment.AppointmentId,

                PatientName =
                    appointment.Patient.FullName,

                DoctorName =
                    appointment.Doctor.FullName,

                ScheduledDate =
                    appointment.ScheduledDate,

                TimeSlot =
                    appointment.TimeSlot,

                Status =
                    appointment.Status.ToString(),

                CancellationReason =
                    appointment.CancellationReason
            };
        }

        public async Task BookAppointmentAsync(
            CreateAppointmentDto dto)
        {
            if (dto.ScheduledDate.Date <
                DateTime.Today)
            {
                throw new Exception(
                    "Past dates are not allowed.");
            }

            if (!TimeSlots.Slots
                .Contains(dto.TimeSlot))
            {
                throw new Exception(
                    "Invalid time slot.");
            }

            if (dto.ScheduledDate.Date ==
                DateTime.Today)
            {
                DateTime slotDateTime =
                    GetSlotDateTime(
                        dto.ScheduledDate,
                        dto.TimeSlot);

                if (slotDateTime <=
                    DateTime.Now)
                {
                    throw new Exception(
                        "Past time slots cannot be booked.");
                }
            }

            bool isBooked =
                await _repo
                .IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot);

            if (isBooked)
            {
                throw new Exception(
                    "Selected slot is already booked.");
            }

            bool hasConflict =
                await _repo
                .HasPatientSlotConflictAsync(
                    dto.PatientId,
                    dto.ScheduledDate,
                    dto.TimeSlot);

            if (hasConflict)
            {
                throw new Exception(
                    "Patient already has another appointment during this slot.");
            }

            bool alreadyExists =
                await _repo
                .HasAppointmentWithDoctorOnSameDayAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    dto.ScheduledDate);

            if (alreadyExists)
            {
                throw new Exception(
                    "Patient already has an appointment with this doctor on this date.");
            }

            Appointment appointment =
                new Appointment
                {
                    PatientId =
                        dto.PatientId,

                    DoctorId =
                        dto.DoctorId,

                    ScheduledDate =
                        dto.ScheduledDate,

                    TimeSlot =
                        dto.TimeSlot,

                    Status =
                        AppointmentStatus.Pending
                };

            await _repo.AddAsync(
                appointment);
        }

        public async Task ConfirmAppointmentAsync(
            int id)
        {
            Appointment appointment =
                await _repo.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            appointment.Status =
                AppointmentStatus.Confirmed;

            await _repo.UpdateAsync(
                appointment);
        }

        public async Task CancelAppointmentAsync(
            int id,
            string reason)
        {
            Appointment appointment =
                await _repo.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                throw new Exception(
                    "Completed appointments cannot be cancelled.");
            }

            if (string.IsNullOrWhiteSpace(
                reason))
            {
                throw new Exception(
                    "Cancellation reason is required.");
            }

            appointment.Status =
                AppointmentStatus.Cancelled;

            appointment.CancellationReason =
                reason;

            await _repo.UpdateAsync(
                appointment);
        }

        public async Task<List<AppointmentDto>>
            GetAppointmentsForPatientAsync(
                int patientId)
        {
            var appointments =
                await _repo
                .GetAppointmentsByPatientAsync(
                    patientId);

            return appointments
                .Select(a => new AppointmentDto
                {
                    AppointmentId =
                        a.AppointmentId,

                    PatientName =
                        a.Patient.FullName,

                    DoctorName =
                        a.Doctor.FullName,

                    ScheduledDate =
                        a.ScheduledDate,

                    TimeSlot =
                        a.TimeSlot,

                    Status =
                        a.Status.ToString(),

                    CancellationReason =
                        a.CancellationReason
                })
                .ToList();
        }

        public async Task<List<AppointmentDto>>
            GetUpcomingAppointmentsAsync()
        {
            var appointments =
                await _repo
                .GetUpcomingAppointmentsAsync();

            return appointments
                .Select(a => new AppointmentDto
                {
                    AppointmentId =
                        a.AppointmentId,

                    PatientName =
                        a.Patient.FullName,

                    DoctorName =
                        a.Doctor.FullName,

                    ScheduledDate =
                        a.ScheduledDate,

                    TimeSlot =
                        a.TimeSlot,

                    Status =
                        a.Status.ToString(),

                    CancellationReason =
                        a.CancellationReason
                })
                .ToList();
        }

        public async Task<List<AppointmentDto>>
            GetUpcomingAppointmentsByDoctorAsync(
                string doctorName)
        {
            var appointments =
                await _repo
                .GetUpcomingAppointmentsByDoctorAsync(
                    doctorName);

            return appointments
                .Select(a => new AppointmentDto
                {
                    AppointmentId =
                        a.AppointmentId,

                    PatientName =
                        a.Patient.FullName,

                    DoctorName =
                        a.Doctor.FullName,

                    ScheduledDate =
                        a.ScheduledDate,

                    TimeSlot =
                        a.TimeSlot,

                    Status =
                        a.Status.ToString(),

                    CancellationReason =
                        a.CancellationReason
                })
                .ToList();
        }

        public async Task<List<string>>
            GetAvailableSlotsAsync(
                int doctorId,
                DateTime scheduledDate)
        {
            return await _repo
                .GetAvailableSlotsAsync(
                    doctorId,
                    scheduledDate);
        }

        public async Task<List<AppointmentDto>>
            GetAppointmentsByPatientNameAsync(
                string patientName)
        {
            var appointments =
                await _repo
                .GetAppointmentsByPatientNameAsync(
                    patientName);

            return appointments
                .Select(a => new AppointmentDto
                {
                    AppointmentId =
                        a.AppointmentId,

                    PatientName =
                        a.Patient.FullName,

                    DoctorName =
                        a.Doctor.FullName,

                    ScheduledDate =
                        a.ScheduledDate,

                    TimeSlot =
                        a.TimeSlot,

                    Status =
                        a.Status.ToString(),

                    CancellationReason =
                        a.CancellationReason
                })
                .ToList();
        }

        public async Task<bool>
            HealthRecordExistsAsync(
                int appointmentId)
        {
            return await _repo
                .HealthRecordExistsAsync(
                    appointmentId);
        }

        private DateTime GetSlotDateTime(
            DateTime date,
            string slot)
        {
            string timePart =
                DateTime.Parse(slot)
                .ToString("HH:mm");

            return DateTime.Parse(
                date.ToString("yyyy-MM-dd")
                + " "
                + timePart);
        }
    }
}
