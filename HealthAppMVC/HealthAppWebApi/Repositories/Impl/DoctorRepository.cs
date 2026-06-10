using HealthAppWebApi.App_Data;
using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebApi.Repositories.Impl
{
    public class DoctorRepository
        : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Doctor>>
            GetAllAsync()
        {
            return await _context.Doctors
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public async Task<Doctor>
            GetByIdAsync(int id)
        {
            return await _context.Doctors
                .FindAsync(id);
        }

        public async Task AddAsync(
            Doctor doctor)
        {
            _context.Doctors.Add(doctor);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            Doctor doctor)
        {
            _context.Entry(doctor).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(
            int id,
            bool isActive)
        {
            Doctor doctor =
                await _context.Doctors
                .FindAsync(id);

            if (doctor == null)
                return;

            doctor.IsActive = isActive;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Doctor>>
            GetBySpecialisationAsync(
                SpecialisationType specialisation)
        {
            return await _context.Doctors
                .Where(d =>
                    d.Specialisation ==
                    specialisation)
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }

        public async Task<List<Doctor>>
            SearchByNameAsync(
                string name)
        {
            return await _context.Doctors
                .Where(d =>
                    d.FullName.Contains(name))
                .OrderBy(d => d.FullName)
                .ToListAsync();
        }
    }
}