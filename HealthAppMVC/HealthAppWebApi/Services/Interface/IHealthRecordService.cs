using SharedDto.HealthRecordDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebApi.Services.Interface
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>>
            GetAllAsync();

        Task<HealthRecordDto>
            GetByIdAsync(int id);

        Task<List<HealthRecordDto>>
            GetPatientHistoryAsync(
                int patientId);

        Task AddAsync(
            CreateHealthRecordDto dto);
    }
}