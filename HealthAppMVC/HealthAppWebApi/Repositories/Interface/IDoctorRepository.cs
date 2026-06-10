using HealthAppWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebApi.Repositories.Interface
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllAsync();

        Task<Doctor> GetByIdAsync(int id);

        Task AddAsync(Doctor doctor);

        Task UpdateAsync(Doctor doctor);

        Task ChangeStatusAsync(
            int id,
            bool isActive);

        Task<List<Doctor>>
            GetBySpecialisationAsync(
                SpecialisationType specialisation);

        Task<List<Doctor>>
            SearchByNameAsync(string name);
    }
}