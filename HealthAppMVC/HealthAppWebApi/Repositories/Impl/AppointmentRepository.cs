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
    public class AppointmentRepository
     : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>>
     GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        public async Task<Appointment>
     GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(
                    a => a.AppointmentId == id);
        }

        public async Task AddAsync(
     Appointment appointment)
        {
            _context.Appointments.Add(
                appointment);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(
     Appointment appointment)
        {
            _context.Entry(appointment)
                .State =
                EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<bool>
     IsDoctorSlotBookedAsync(
         int doctorId,
         DateTime date,
         string slot)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    DbFunctions.TruncateTime(
                        a.ScheduledDate)
                    ==
                    DbFunctions.TruncateTime(date)
                    &&
                    a.TimeSlot == slot
                    &&
                    a.Status !=
                        AppointmentStatus.Cancelled);
        }

        public async Task<bool>
     HasPatientSlotConflictAsync(
         int patientId,
         DateTime date,
         string slot)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.PatientId == patientId
                    &&
                    DbFunctions.TruncateTime(
                        a.ScheduledDate)
                    ==
                    DbFunctions.TruncateTime(date)
                    &&
                    a.TimeSlot == slot
                    &&
                    a.Status !=
                        AppointmentStatus.Cancelled);
        }

        public async Task<bool>
    HasAppointmentWithDoctorOnSameDayAsync(
        int patientId,
        int doctorId,
        DateTime date)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.PatientId == patientId
                    &&
                    a.DoctorId == doctorId
                    &&
                    DbFunctions.TruncateTime(
                        a.ScheduledDate)
                    ==
                    DbFunctions.TruncateTime(
                        date)
                    &&
                    a.Status !=
                        AppointmentStatus.Cancelled);
        }

       
    public async Task<List<Appointment>>
    GetUpcomingConfirmedAppointmentsByDoctorAsync(
        int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.DoctorId == doctorId
                    &&
                    a.Status ==
                        AppointmentStatus.Confirmed
                    &&
                    a.ScheduledDate >=
                        DateTime.Today)
                .OrderBy(a => a.ScheduledDate)
                .ToListAsync();
        }

        
   public async Task<List<Appointment>>
    GetAppointmentsByPatientAsync(
        int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a =>
                    a.PatientId == patientId)
                .OrderByDescending(
                    a => a.ScheduledDate)
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