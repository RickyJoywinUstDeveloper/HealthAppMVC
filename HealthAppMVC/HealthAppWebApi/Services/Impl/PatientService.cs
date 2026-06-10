using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using HealthAppWebApi.Services.Interface;
using SharedDto.PatientDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAppWebApi.Services.Impl
{
    public class PatientService
        : IPatientService
    {
        private readonly
            IPatientRepository _repo;

        public PatientService(
            IPatientRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<PatientDto>>
            GetAllPatientsAsync()
        {
            var patients =
                await _repo.GetAllAsync();

            return patients.Select(p =>
                new PatientDto
                {
                    PatientId = p.PatientId,
                    FullName = p.FullName,
                    Gender = p.Gender.ToString(),
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    InsuranceId = p.InsuranceId
                }).ToList();
        }

        public async Task<PatientDto>
            GetPatientByIdAsync(int id)
        {
            var patient =
                await _repo.GetByIdAsync(id);

            if (patient == null)
                return null;

            return new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                Gender = patient.Gender.ToString(),
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                InsuranceId = patient.InsuranceId
            };
        }

        public async Task RegisterPatientAsync(
            CreatePatientDto dto)
        {
            if (await _repo
                .EmailExistsAsync(dto.Email))
            {
                throw new Exception(
                    "Email already exists.");
            }

            if (dto.DateOfBirth >
                DateTime.Today)
            {
                throw new Exception(
                    "Future date not allowed.");
            }

            Patient patient =
                new Patient
                {
                    FullName = dto.FullName,
                    DateOfBirth = dto.DateOfBirth,
                    Gender =
                        (GenderType)Enum.Parse(
                            typeof(GenderType),
                            dto.Gender,
                            true),

                    Email = dto.Email,

                    PhoneNumber =
                        dto.PhoneNumber,

                    InsuranceId =
                        dto.InsuranceId,

                    CreatedDate =
                        DateTime.UtcNow
                };

            await _repo.AddAsync(patient);
        }

        public async Task UpdatePatientAsync(
            int id,
            CreatePatientDto dto)
        {
            Patient patient =
                await _repo.GetByIdAsync(id);

            if (patient == null)
            {
                throw new Exception(
                    "Patient not found.");
            }

            patient.FullName =
                dto.FullName;

            patient.DateOfBirth =
                dto.DateOfBirth;

            patient.Email =
                dto.Email;

            patient.PhoneNumber =
                dto.PhoneNumber;

            patient.InsuranceId =
                dto.InsuranceId;

            patient.Gender =
                (GenderType)Enum.Parse(
                    typeof(GenderType),
                    dto.Gender,
                    true);

            await _repo.UpdateAsync(patient);
        }

        public async Task<List<PatientDto>>
            SearchByNameAsync(string name)
        {
            var patients =
                await _repo
                .SearchByNameAsync(name);

            return patients.Select(p =>
                new PatientDto
                {
                    PatientId = p.PatientId,
                    FullName = p.FullName,
                    Email = p.Email,
                    Gender = p.Gender.ToString()
                }).ToList();
        }

        public async Task<int>
            GetAppointmentCountAsync(
                int patientId)
        {
            return await _repo
                .GetAppointmentCountAsync(
                    patientId);
        }
    }
}