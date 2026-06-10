using SharedDto.PatientDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Interface
{
    public interface IPatientApiService
    {
        Task<List<PatientDto>>
            GetAllPatientsAsync();

        Task<PatientDto>
            GetPatientByIdAsync(
                int id);

        Task CreatePatientAsync(
            CreatePatientDto dto);

        Task UpdatePatientAsync(
            int id,
            CreatePatientDto dto);

        Task<List<PatientDto>>
            SearchByNameAsync(
                string name);

        Task<int>
            GetAppointmentCountAsync(
                int patientId);
    }
}