using HealthAppMVC.Models;
using HealthAppMVC.Repository.Interface;
using HealthAppMVC.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAppMVC.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(
            IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _patientRepository.GetAll();
        }

        public Patient GetPatientById(int id)
        {
            var patient =
                _patientRepository.GetById(id);

            if (patient == null)
            {
                throw new Exception(
                    $"Patient with Id {id} not found.");
            }

            return patient;
        }

        public void RegisterPatient(Patient patient)
        {
            if (_patientRepository.EmailExists(patient.Email))
            {
                throw new Exception(
                    "Email already exists.");
            }

            if (patient.DateOfBirth > DateTime.Today)
            {
                throw new Exception("Date of Birth cannot be a future date.");
            }

            patient.CreatedDate = DateTime.Now;

            _patientRepository.Add(patient);
        }

        public void UpdatePatient(Patient patient)
        {
            var existingPatient =
                _patientRepository.GetById(
                    patient.PatientId);

            if (existingPatient == null)
            {
                throw new Exception(
                    "Patient not found.");
            }

            if (patient.DateOfBirth > DateTime.Today)
            {
                throw new Exception("Date of Birth cannot be a future date.");
            }

            _patientRepository.Update(patient);
        }

      

        public int GetAppointmentCount(
            int patientId)
        {
            return _patientRepository
                .GetAppointmentCount(patientId);
        }

        public IEnumerable<Patient> SearchByName(string name)
        {
            return _patientRepository.SearchByName(name);
        }
    }
}