using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Interface;
using SharedDto.DoctorDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebApi.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;

        public DoctorService(
            IDoctorRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DoctorDto>>
            GetAllDoctorsAsync()
        {
            var doctors =
                await _repo.GetAllAsync();

            return doctors
                .Select(d => new DoctorDto
                {
                    DoctorId =
                        d.DoctorId,

                    FullName =
                        d.FullName,

                    Specialisation =
                        d.Specialisation.ToString(),

                    ConsultationFee =
                        d.ConsultationFee,

                    IsActive =
                        d.IsActive,

                    DoctorPhoneNo =
                        d.DoctorPhoneNo,

                    DoctorEmail =
                        d.DoctorEmail
                })
                .ToList();
        }

        public async Task<DoctorDto>
            GetDoctorByIdAsync(int id)
        {
            var doctor =
                await _repo.GetByIdAsync(id);

            if (doctor == null)
            {
                return null;
            }

            return new DoctorDto
            {
                DoctorId =
                    doctor.DoctorId,

                FullName =
                    doctor.FullName,

                Specialisation =
                    doctor.Specialisation
                        .ToString(),

                ConsultationFee =
                    doctor.ConsultationFee,

                IsActive =
                    doctor.IsActive,

                DoctorPhoneNo =
                    doctor.DoctorPhoneNo,

                DoctorEmail =
                    doctor.DoctorEmail
            };
        }

        public async Task AddDoctorAsync(
            CreateDoctorDto dto)
        {
            if (!Enum.TryParse(
                dto.Specialisation,
                true,
                out SpecialisationType specialisation))
            {
                throw new Exception(
                    "Invalid Specialisation.");
            }

            if (dto.ConsultationFee <= 0)
            {
                throw new Exception(
                    "Consultation fee must be greater than zero.");
            }

            if (dto.YearsOfExperience < 0)
            {
                throw new Exception(
                    "Years of experience cannot be negative.");
            }

            Doctor doctor =
                new Doctor
                {
                    FullName =
                        dto.FullName,

                    Specialisation =
                        specialisation,

                    YearsOfExperience =
                        dto.YearsOfExperience,

                    ConsultationFee =
                        dto.ConsultationFee,

                    DoctorEmail =
                        dto.DoctorEmail,

                    DoctorPhoneNo =
                        dto.DoctorPhoneNo,

                    IsActive = true
                };

            await _repo.AddAsync(doctor);
        }

        public async Task UpdateDoctorAsync(
            int id,
            CreateDoctorDto dto)
        {
            Doctor doctor =
                await _repo.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new Exception(
                    "Doctor not found.");
            }

            if (!Enum.TryParse(
                dto.Specialisation,
                true,
                out SpecialisationType specialisation))
            {
                throw new Exception(
                    "Invalid Specialisation.");
            }

            if (dto.ConsultationFee <= 0)
            {
                throw new Exception(
                    "Consultation fee must be greater than zero.");
            }

            if (dto.YearsOfExperience < 0)
            {
                throw new Exception(
                    "Years of experience cannot be negative.");
            }

            doctor.FullName =
                dto.FullName;

            doctor.Specialisation =
                specialisation;

            doctor.YearsOfExperience =
                dto.YearsOfExperience;

            doctor.ConsultationFee =
                dto.ConsultationFee;

            doctor.DoctorEmail =
                dto.DoctorEmail;

            doctor.DoctorPhoneNo =
                dto.DoctorPhoneNo;

            await _repo.UpdateAsync(
                doctor);
        }

        public async Task ChangeStatusAsync(
            int id,
            bool isActive)
        {
            Doctor doctor =
                await _repo.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new Exception(
                    "Doctor not found.");
            }

            await _repo.ChangeStatusAsync(
                id,
                isActive);
        }

        public async Task<List<DoctorDto>>
            GetDoctorsBySpecialisationAsync(
                string specialisation)
        {
            if (!Enum.TryParse(
                    specialisation,
                    true,
                    out SpecialisationType spec))
            {
                throw new Exception(
                    "Invalid Specialisation.");
            }

            var doctors =
                await _repo
                    .GetBySpecialisationAsync(
                        spec);

            return doctors
                .Select(d => new DoctorDto
                {
                    DoctorId =
                        d.DoctorId,

                    FullName =
                        d.FullName,

                    Specialisation =
                        d.Specialisation
                            .ToString(),

                    ConsultationFee =
                        d.ConsultationFee,

                    IsActive =
                        d.IsActive,

                    DoctorPhoneNo =
                        d.DoctorPhoneNo,

                    DoctorEmail =
                        d.DoctorEmail
                })
                .ToList();
        }

        public async Task<List<DoctorDto>>
            SearchByNameAsync(
                string name)
        {
            var doctors =
                await _repo
                    .SearchByNameAsync(name);

            return doctors
                .Select(d => new DoctorDto
                {
                    DoctorId =
                        d.DoctorId,

                    FullName =
                        d.FullName,

                    Specialisation =
                        d.Specialisation
                            .ToString(),

                    ConsultationFee =
                        d.ConsultationFee,

                    IsActive =
                        d.IsActive,

                    DoctorPhoneNo =
                        d.DoctorPhoneNo,

                    DoctorEmail =
                        d.DoctorEmail
                })
                .ToList();
        }
    }
}