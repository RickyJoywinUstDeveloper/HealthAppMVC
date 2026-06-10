using HealthAppWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebApi.Repositories.Interface
{
    public interface IHealthRecordRepository
    {
        Task<List<HealthRecord>> GetAllAsync();

        Task<HealthRecord> GetByIdAsync(
            int id);

        Task<HealthRecord> GetByAppointmentIdAsync(
            int appointmentId);

        Task<List<HealthRecord>> GetByPatientIdAsync(
            int patientId);

        Task AddAsync(
            HealthRecord record);
    }
}