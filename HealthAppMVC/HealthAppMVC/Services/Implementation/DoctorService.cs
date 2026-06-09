using HealthAppMVC.Models;
using HealthAppMVC.Repository.Interface;
using HealthAppMVC.Services.Interface;
using System;
using System.Collections.Generic;

namespace HealthAppMVC.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAll();
        }

        public Doctor GetDoctorById(int id)
        {
            Doctor doctor =
                _doctorRepository.GetById(id);

            if (doctor == null)
            {
                throw new Exception(
                    $"Doctor with Id {id} not found.");
            }

            return doctor;
        }

        public void AddDoctor(Doctor doctor)
        {
            if (doctor.ConsultationFee <= 0)
            {
                throw new Exception(
                    "Consultation fee must be greater than zero.");
            }

            if (doctor.YearsOfExperience < 0)
            {
                throw new Exception(
                    "Years of experience cannot be negative.");
            }

            doctor.IsActive = true;

            _doctorRepository.Add(doctor);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            Doctor existingDoctor =
                _doctorRepository.GetById(
                    doctor.DoctorId);

            if (existingDoctor == null)
            {
                throw new Exception(
                    "Doctor not found.");
            }

            if (doctor.ConsultationFee <= 0)
            {
                throw new Exception(
                    "Consultation fee must be greater than zero.");
            }

            if (doctor.YearsOfExperience < 0)
            {
                throw new Exception(
                    "Years of experience cannot be negative.");
            }

            _doctorRepository.Update(doctor);
        }

        public void ChangeDoctorStatus(
            int doctorId,
            bool isActive)
        {
            Doctor doctor =
                _doctorRepository.GetById(
                    doctorId);

            if (doctor == null)
            {
                throw new Exception(
                    "Doctor not found.");
            }

            _doctorRepository.ChangeStatus(
                doctorId,
                isActive);
        }

        public IEnumerable<Doctor> SearchBySpecialisation(
            SpecialisationType specialisation)
        {
            return _doctorRepository
                .SearchBySpecialisation(
                    specialisation);
        }

        public IEnumerable<Doctor> SearchByName(string name)
        {
            return _doctorRepository.SearchByName(name);
        }
    }
}