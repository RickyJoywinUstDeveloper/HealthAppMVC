using SharedDto.DoctorDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Interface
{
    public interface IDoctorApiService
    {
        Task<List<DoctorDto>>
            GetAllDoctorsAsync();

        Task<DoctorDto>
            GetDoctorByIdAsync(
                int id);

        Task AddDoctorAsync(
            CreateDoctorDto dto);

        Task UpdateDoctorAsync(
            int id,
            CreateDoctorDto dto);

        Task ChangeStatusAsync(
            int id,
            bool isActive);

        Task<List<DoctorDto>>
            GetDoctorsBySpecialisationAsync(
                string specialisation);

        Task<List<DoctorDto>>
            SearchByNameAsync(
                string name);
    }
}