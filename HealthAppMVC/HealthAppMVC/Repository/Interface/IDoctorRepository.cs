using HealthAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppMVC.Repository.Interface
{

    public interface IDoctorRepository
    {
        List<Doctor> GetAll();

        Doctor GetById(int id);

        void Add(Doctor doctor);

        void Update(Doctor doctor);

        void ChangeStatus(int id, bool isActive);

        List<Doctor> SearchBySpecialisation(
            SpecialisationType specialisation);

        List<Doctor> SearchByName(string name);
    }
}
