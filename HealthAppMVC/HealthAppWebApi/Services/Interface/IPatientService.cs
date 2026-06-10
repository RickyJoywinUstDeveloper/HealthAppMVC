using SharedDto.PatientDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebApi.Services.Interface
{
    public interface IPatientService
    {
        Task<List<PatientDto>>
            GetAllPatientsAsync();

        Task<PatientDto>
            GetPatientByIdAsync(int id);

        Task RegisterPatientAsync(
            CreatePatientDto dto);

        Task UpdatePatientAsync(
            int id,
            CreatePatientDto dto);

        Task<List<PatientDto>>
            SearchByNameAsync(string name);

        Task<int>
            GetAppointmentCountAsync(
                int patientId);
    }
}