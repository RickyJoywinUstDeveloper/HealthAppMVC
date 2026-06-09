using HealthAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Interface
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAllAppointments();

        Appointment GetAppointmentById(int id);

        void BookAppointment(
            Appointment appointment);

        void ConfirmAppointment(
            int appointmentId);

        void CancelAppointment(
            int appointmentId,
            string reason);

        

        IEnumerable<Appointment>
            GetAppointmentsByPatient(
            int patientId);

        IEnumerable<Appointment>
            GetAppointmentsByDoctor(
            int doctorId);

        IEnumerable<Appointment>
        GetUpcomingAppointments();

        IEnumerable<Appointment>
       GetUpcomingAppointmentsByDoctor(
       string doctorName);


    }
}
