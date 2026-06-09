using HealthAppMVC.Models;
using System.Collections.Generic;

namespace HealthAppMVC.Services.Interface
{
    public interface IDoctorService
    {
        IEnumerable<Doctor> GetAllDoctors();

        Doctor GetDoctorById(int id);

        void AddDoctor(Doctor doctor);

        void UpdateDoctor(Doctor doctor);

        void ChangeDoctorStatus(
            int doctorId,
            bool isActive);

        IEnumerable<Doctor> SearchBySpecialisation(
            SpecialisationType specialisation);

        IEnumerable<Doctor> SearchByName(string name);
    }
}