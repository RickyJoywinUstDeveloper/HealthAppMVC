using HealthAppWebApi.App_Data;
using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebApi.Repositories.Impl
{
    public class PatientRepository
        : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>>
            GetAllAsync()
        {
            return await _context.Patients
                .AsNoTracking()
                .OrderBy(p => p.FullName)
                .ToListAsync();
        }

        public async Task<Patient>
            GetByIdAsync(int id)
        {
            return await _context.Patients
                .FindAsync(id);
        }

        public async Task AddAsync(
            Patient patient)
        {
            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            Patient patient)
        {
            _context.Entry(patient).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<bool>
            EmailExistsAsync(string email)
        {
            return await _context.Patients
                .AnyAsync(p =>
                    p.Email == email);
        }

        public async Task<int>
            GetAppointmentCountAsync(
                int patientId)
        {
            return await _context.Appointments
                .CountAsync(a =>
                    a.PatientId == patientId);
        }

        public async Task<List<Patient>>
            SearchByNameAsync(string name)
        {
            return await _context.Patients
                .Where(p =>
                    p.FullName.Contains(name))
                .OrderBy(p => p.FullName)
                .ToListAsync();
        }
    }
}