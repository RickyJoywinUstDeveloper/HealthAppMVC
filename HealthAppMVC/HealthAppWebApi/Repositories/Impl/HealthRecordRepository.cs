using HealthAppWebApi.App_Data;
using HealthAppWebApi.Models;
using HealthAppWebApi.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HealthAppWebApi.Repositories.Impl
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly AppDbContext _context;

        public HealthRecordRepository(AppDbContext context)
        {
            _context = context;
        }


        

public async Task<List<HealthRecord>>
            GetAllAsync()
        {
            return await _context.HealthRecords
                .Include(h => h.Appointment)
                .Include(h => h.Appointment.Patient)
                .Include(h => h.Appointment.Doctor)
                .OrderByDescending(
                    h => h.VisitDate)
                .ToListAsync();
        }



        public async Task<HealthRecord>
                    GetByIdAsync(int id)
        {
            return await _context.HealthRecords
                .Include(h => h.Appointment)
                .Include(h => h.Appointment.Patient)
                .Include(h => h.Appointment.Doctor)
                .FirstOrDefaultAsync(
                    h => h.HealthRecordId == id);
        }



        public async Task<HealthRecord>
                    GetByAppointmentIdAsync(
                        int appointmentId)
        {
            return await _context.HealthRecords
                .FirstOrDefaultAsync(
                    h => h.AppointmentId
                        == appointmentId);
        }



        public async Task AddAsync(
                   HealthRecord record)
        {
            _context.HealthRecords.Add(
                record);

            await _context.SaveChangesAsync();
        }




        public async Task<List<HealthRecord>>
                    GetByPatientIdAsync(
                        int patientId)
        {
            return await _context.HealthRecords
                .Include(h => h.Appointment)
                .Include(h => h.Appointment.Patient)
                .Include(h => h.Appointment.Doctor)
                .Where(h =>
                    h.Appointment.PatientId
                        == patientId)
                .OrderByDescending(
                    h => h.VisitDate)
                .ToListAsync();
        }


        public async Task<bool>
    HealthRecordExistsAsync(
        int appointmentId)
        {
            return await _context.HealthRecords
                .AnyAsync(h =>
                    h.AppointmentId ==
                    appointmentId);
        }

        public async Task<List<Appointment>>
    GetUpcomingAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.ScheduledDate >= DateTime.Today &&
                    (a.Status ==
                        AppointmentStatus.Pending ||
                     a.Status ==
                        AppointmentStatus.Confirmed))
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync();
        }

        public async Task<List<Appointment>>
    GetUpcomingAppointmentsByDoctorAsync(
        string doctorName)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.Doctor.FullName.Contains(
                        doctorName)
                    &&
                    a.ScheduledDate >=
                        DateTime.Today)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync();
        }


        public async Task<List<string>>
    GetAvailableSlotsAsync(
        int doctorId,
        DateTime scheduledDate)
        {
            List<string> bookedSlots =
                await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    DbFunctions.TruncateTime(
                        a.ScheduledDate)
                    ==
                    DbFunctions.TruncateTime(
                        scheduledDate)
                    &&
                    a.Status !=
                        AppointmentStatus.Cancelled)
                .Select(a => a.TimeSlot)
                .ToListAsync();

            return Constants.TimeSlots.Slots
                .Except(bookedSlots)
                .ToList();
        }

        public async Task<List<Appointment>>
    GetAppointmentsByPatientNameAsync(
        string patientName)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.Patient.FullName
                        .Contains(patientName))
                .OrderByDescending(
                    a => a.ScheduledDate)
                .ToListAsync();
        }

    }
}