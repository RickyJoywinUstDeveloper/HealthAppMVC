using HealthAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Repository.Interface
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();

        Patient GetById(int id);

        void Add(Patient patient);

        void Update(Patient patient);

      

        bool EmailExists(string email);

        int GetAppointmentCount(int patientId);

        List<Patient> SearchByName(string name);
    }
}
