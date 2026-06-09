using HealthAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Repository.Interface
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();

        Appointment GetById(int id);

        void Add(Appointment appointment);

        void UpdateStatus(
            int appointmentId,
            AppointmentStatus status,
            string cancellationReason);

       

        bool IsSlotAvailable(
            int doctorId,
            string date,
            string timeSlot);

        List<Appointment> GetAppointmentsByPatient(
            int patientId);

        List<Appointment> GetAppointmentsByDoctor(
            int doctorId);


        bool IsDoctorSlotBooked(
    int doctorId,
    DateTime scheduledDate,
    string timeSlot);

        bool HasPatientAppointmentOnDate(
            int patientId,
            int doctorId,
            DateTime scheduledDate);

        bool HasPatientSlotConflict(
            int patientId,
            DateTime scheduledDate,
            string timeSlot);

     

        bool HealthRecordExists(
            int appointmentId);

        List<Appointment> GetUpcomingAppointments();
        List<Appointment> GetUpcomingAppointmentsByDoctor(string doctorName);

        List<string> GetAvailableSlots(
    int doctorId,
    DateTime scheduledDate);

        List<Appointment> GetAppointmentsByPatientName(
            string patientName);
    }
}
    