using HealthAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Interface
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAllPatients();

        Patient GetPatientById(int id);

        void RegisterPatient(Patient patient);

        void UpdatePatient(Patient patient);

        IEnumerable<Patient> SearchByName(string name);

        int GetAppointmentCount(int patientId);

       
    }
    
    }
