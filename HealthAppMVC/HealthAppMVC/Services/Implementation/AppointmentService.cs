using HealthAppMVC.Models;
using HealthAppMVC.Repository.Interface;
using HealthAppMVC.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace HealthAppMVC.Services.Implementation
{
    public class AppointmentService
       : IAppointmentService
    {
        private readonly IAppointmentRepository
            _appointmentRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository =
                appointmentRepository;
        }

        public IEnumerable<Appointment>
            GetAllAppointments()
        {
            return _appointmentRepository.GetAll();
        }

        public Appointment GetAppointmentById(
            int id)
        {
            Appointment appointment =
                _appointmentRepository.GetById(id);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            return appointment;
        }

        public void BookAppointment(
            Appointment appointment)
        {
            if (appointment.ScheduledDate.Date <
                DateTime.Today)
            {
                throw new Exception(
                    "Past dates are not allowed.");
            }

            if (appointment.ScheduledDate.Date ==
    DateTime.Today)
            {
                DateTime slotDateTime;

                if (!DateTime.TryParse(
                        appointment.ScheduledDate
                            .ToString("yyyy-MM-dd")
                        + " "
                        + appointment.TimeSlot,
                        out slotDateTime))
                {
                    throw new Exception(
                        "Invalid appointment time slot.");
                }

                if (slotDateTime <= DateTime.Now)
                {
                    throw new Exception(
                        "Past time slots cannot be booked.");
                }
            }

            bool available =
                _appointmentRepository
                .IsSlotAvailable(
                    appointment.DoctorId,
                    appointment.ScheduledDate
                        .ToString("yyyy-MM-dd"),
                    appointment.TimeSlot);

            if (!available)
            {
                throw new Exception(
                    "Selected slot is already booked.");
            }

            if (_appointmentRepository
       .IsDoctorSlotBooked(
           appointment.DoctorId,
           appointment.ScheduledDate,
           appointment.TimeSlot))
            {
                throw new Exception(
                    "Selected doctor already has an appointment in this slot.");
            }

            if (_appointmentRepository
       .HasPatientAppointmentOnDate(
           appointment.PatientId,
           appointment.DoctorId,
           appointment.ScheduledDate))
            {
                throw new Exception(
                    "Patient already has an appointment with this doctor on the selected date.");
            }

            if (_appointmentRepository
       .HasPatientSlotConflict(
           appointment.PatientId,
           appointment.ScheduledDate,
           appointment.TimeSlot))
            {
                throw new Exception(
                    "Patient already has another appointment during this time slot.");
            }



            appointment.Status =
                AppointmentStatus.Pending;

            _appointmentRepository
                .Add(appointment);
        }

        public void ConfirmAppointment(
            int appointmentId)
        {
            Appointment appointment =
                _appointmentRepository
                .GetById(appointmentId);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            _appointmentRepository
                .UpdateStatus(
                    appointmentId,
                    AppointmentStatus.Confirmed,
                    null);
        }

        public void CancelAppointment(
            int appointmentId,
            string reason)
        {
            Appointment appointment =
                _appointmentRepository
                .GetById(appointmentId);

            if (appointment == null)
            {
                throw new Exception(
                    "Appointment not found.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new Exception("Completed appointments cannot be cancelled.");
            }


            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new Exception(
                    "Cancellation reason is required.");
            }

            _appointmentRepository
                .UpdateStatus(
                    appointmentId,
                    AppointmentStatus.Cancelled,
                    reason);
        }



        public IEnumerable<Appointment>
            GetAppointmentsByPatient(
            int patientId)
        {
            return _appointmentRepository
                .GetAppointmentsByPatient(
                    patientId);
        }

        public IEnumerable<Appointment>
            GetAppointmentsByDoctor(
            int doctorId)
        {
            return _appointmentRepository
                .GetAppointmentsByDoctor(
                    doctorId);
        }


        public IEnumerable<Appointment>
GetUpcomingAppointments()
        {
            return _appointmentRepository
                .GetUpcomingAppointments();
        }


        public IEnumerable<Appointment>
GetUpcomingAppointmentsByDoctor(
    string doctorName)
        {
            return _appointmentRepository
                .GetUpcomingAppointmentsByDoctor(
                    doctorName);
        }


        public IEnumerable<string>
    GetAvailableSlots(
        int doctorId,
        DateTime scheduledDate)
        {
            return _appointmentRepository
                .GetAvailableSlots(
                    doctorId,
                    scheduledDate);
        }


        public IEnumerable<Appointment>
    GetAppointmentsByPatientName(
        string patientName)
        {
            return _appointmentRepository
                .GetAppointmentsByPatientName(
                    patientName);
        }

        public bool HealthRecordExists(
    int appointmentId)
        {
            return _appointmentRepository
                .HealthRecordExists(appointmentId);
        }
    }
}