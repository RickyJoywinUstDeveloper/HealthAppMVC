using HealthAppWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebApi.Repositories.Interface
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync();

        Task<Patient> GetByIdAsync(int id);

        Task AddAsync(Patient patient);

        Task UpdateAsync(Patient patient);

        Task<bool> EmailExistsAsync(string email);

        Task<int> GetAppointmentCountAsync(int patientId);

        Task<List<Patient>> SearchByNameAsync(string name);
    }
}