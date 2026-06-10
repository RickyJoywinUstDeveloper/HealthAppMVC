using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Interface;
using SharedDto.HealthRecordDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebApi.Services.Impl
{
    public class HealthRecordService
        : IHealthRecordService
    {
        private readonly
            IHealthRecordRepository _recordRepo;

        private readonly
            IAppointmentRepository _appointmentRepo;

        public HealthRecordService(
            IHealthRecordRepository recordRepo,
            IAppointmentRepository appointmentRepo)
        {
            _recordRepo = recordRepo;

            _appointmentRepo = appointmentRepo;
        }

        public async Task<List<HealthRecordDto>>
            GetAllAsync()
        {
            var records =
                await _recordRepo.GetAllAsync();

            return records
                .Select(h => new HealthRecordDto
                {
                    HealthRecordId =
                        h.HealthRecordId,

                    VisitDate =
                        h.VisitDate,

                    PatientName =
                        h.Appointment
                            .Patient
                            .FullName,

                    DoctorName =
                        h.Appointment
                            .Doctor
                            .FullName,

                    Diagnosis =
                        h.Diagnosis,

                    Prescription =
                        h.Prescription,

                    Notes =
                        h.Notes
                })
                .ToList();
        }

        public async Task<HealthRecordDto>
            GetByIdAsync(int id)
        {
            HealthRecord record =
                await _recordRepo
                    .GetByIdAsync(id);

            if (record == null)
            {
                throw new Exception(
                    "Health record not found.");
            }

            return new HealthRecordDto
            {
                HealthRecordId =
         record.HealthRecordId,

                AppointmentId =
         record.AppointmentId,

                PatientId =
         record.Appointment != null
             ? record.Appointment.PatientId
             : 0,

                VisitDate =
         record.VisitDate,

                PatientName =
         record.Appointment != null
         && record.Appointment.Patient != null
             ? record.Appointment
                 .Patient.FullName
             : "Unknown Patient",

                DoctorName =
         record.Appointment != null
         && record.Appointment.Doctor != null
             ? record.Appointment
                 .Doctor.FullName
             : "Unknown Doctor",

                Diagnosis =
         record.Diagnosis,

                Prescription =
         record.Prescription,

                Notes =
         record.Notes
            };
        }

        public async Task<List<HealthRecordDto>>
    GetPatientHistoryAsync(
        int patientId)
        {
            var records =
                await _recordRepo
                    .GetByPatientIdAsync(
                        patientId);

            return records
                .Select(h => new HealthRecordDto
                {
                    HealthRecordId =
                        h.HealthRecordId,

                    AppointmentId =
                        h.AppointmentId,

                    PatientId =
                        h.Appointment != null
                            ? h.Appointment.PatientId
                            : 0,

                    VisitDate =
                        h.VisitDate,

                    PatientName =
                        h.Appointment != null
                        && h.Appointment.Patient != null
                            ? h.Appointment
                                .Patient.FullName
                            : "Unknown Patient",

                    DoctorName =
                        h.Appointment != null
                        && h.Appointment.Doctor != null
                            ? h.Appointment
                                .Doctor.FullName
                            : "Unknown Doctor",

                    Diagnosis =
                        h.Diagnosis,

                    Prescription =
                        h.Prescription,

                    Notes =
                        h.Notes
                })
                .ToList();
        }

        public async Task AddAsync(
            CreateHealthRecordDto dto)
        {
            Appointment appointment =
                await _appointmentRepo
                    .GetByIdAsync(
                        dto.AppointmentId);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new Exception(
                    "Health record can only be added for confirmed appointments.");
            }

            HealthRecord existingRecord =
                await _recordRepo
                    .GetByAppointmentIdAsync(
                        dto.AppointmentId);

            if (existingRecord != null)
            {
                throw new Exception(
                    "Health record already exists for this appointment.");
            }

            HealthRecord record =
                new HealthRecord
                {
                    AppointmentId =
                        dto.AppointmentId,

                    VisitDate =
                        DateTime.UtcNow,

                    Diagnosis =
                        dto.Diagnosis,

                    Prescription =
                        dto.Prescription,

                    Notes =
                        dto.Notes
                };

            await _recordRepo
                .AddAsync(record);

            appointment.Status =
                AppointmentStatus.Completed;

            await _appointmentRepo
                .UpdateAsync(
                    appointment);
        }
    }
}