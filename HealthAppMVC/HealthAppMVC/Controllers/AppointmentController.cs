
using HealthAppMVC.Services.Interface;
using SharedDto.AppointmentDtos;
using SharedDto.DoctorDtos;
using SharedDto.PatientDtos;
using SharedDto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class AppointmentController
        : Controller
    {
        private readonly
            IAppointmentApiService
            _appointmentService;

        private readonly
            IPatientApiService
            _patientService;

        private readonly
            IDoctorApiService
            _doctorService;

        public AppointmentController(
            IAppointmentApiService appointmentService,
            IPatientApiService patientService,
            IDoctorApiService doctorService)
        {
            _appointmentService =
                appointmentService;

            _patientService =
                patientService;

            _doctorService =
                doctorService;
        }

        // GET: Appointment
        public async Task<ActionResult>
            Index()
        {
            var appointments =
                await _appointmentService
                    .GetAllAppointmentsAsync();

            return View(appointments);
        }

        // GET: Appointment/Create
        [HttpGet]
        public async Task<ActionResult>
            Create()
        {
            await LoadDropdowns();

            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(CreateAppointmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadDropdowns();

                    return View(dto);
                }

                await _appointmentService
                    .BookAppointmentAsync(dto);

                TempData["Success"] =
                    "Appointment booked successfully.";

                return RedirectToAction(
                    "Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                await LoadDropdowns();

                return View(dto);
            }
        }

        // GET: Appointment/Confirm/5
        [HttpGet]
        public async Task<ActionResult>
            Confirm(int id)
        {
            await _appointmentService
                .ConfirmAppointmentAsync(id);

            TempData["Success"] =
                "Appointment confirmed.";

            return RedirectToAction(
                "UpcomingAppointments");
        }

        // GET: Appointment/Cancel/5
        [HttpGet]
        public async Task<ActionResult>
            Cancel(int id)
        {
            var appointment =
                await _appointmentService
                    .GetAppointmentByIdAsync(id);

            return View(appointment);
        }

        // POST: Appointment/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Cancel(
                int id,
                string cancellationReason)
        {
            try
            {
                CancelAppointmentDto dto =
                    new CancelAppointmentDto
                    {
                        CancellationReason =
                            cancellationReason
                    };

                await _appointmentService
                    .CancelAppointmentAsync(
                        id,
                        dto);

                TempData["Success"] =
                    "Appointment cancelled.";

                return RedirectToAction(
                    "UpcomingAppointments");
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    ex.Message;

                return RedirectToAction(
                    "Cancel",
                    new { id });
            }
        }

        private async Task LoadDropdowns()
        {
            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            var doctors =
                await _doctorService
                    .GetAllDoctorsAsync();

            ViewBag.Patients =
                new SelectList(
                    patients,
                    "PatientId",
                    "FullName");

            ViewBag.Doctors =
                new SelectList(
                    doctors,
                    "DoctorId",
                    "FullName");
        }

        public async Task<ActionResult>
            UpcomingAppointments(
                string doctorName)
        {
            var appointments =
                await _appointmentService
                    .GetUpcomingAppointmentsAsync();

            if (!string.IsNullOrWhiteSpace(
                doctorName))
            {
                appointments =
                    appointments
                    .Where(a =>
                        a.DoctorName
                        .ToLower()
                        .Contains(
                            doctorName
                            .ToLower()))
                    .ToList();
            }

            var appointmentsWithRecords =
    new List<int>();

            foreach (var appointment in appointments)
            {
                bool exists =
                    await _appointmentService
                        .HealthRecordExistsAsync(
                            appointment.AppointmentId);

                if (exists)
                {
                    appointmentsWithRecords
                        .Add(appointment.AppointmentId);
                }
            }

            ViewBag.AppointmentsWithRecords =
                appointmentsWithRecords;

            ViewBag.DoctorName =
                doctorName;

            return View(appointments);
        }

        public async Task<JsonResult>
            SearchDoctorNames(
                string term)
        {
            var doctors =
                await _doctorService
                    .SearchByNameAsync(term);

            var result =
                doctors
                    .Select(d => new
                    {
                        label = d.FullName,
                        value = d.FullName
                    })
                    .ToList();

            return Json(
                result,
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookAppointment()
        {
            ViewBag.Specialisations =
                Enum.GetNames(
                    typeof(SpecialisationType));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            BookAppointment(
                CreateAppointmentDto dto)
        {
            try
            {
                await _appointmentService
                    .BookAppointmentAsync(dto);

                TempData["Success"] =
                    "Appointment booked successfully.";

                return RedirectToAction(
                    "PatientServices",
                    "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                ViewBag.Specialisations =
                    Enum.GetNames(
                        typeof(SpecialisationType));

                return View(dto);
            }
        }

        public async Task<JsonResult>
            SearchPatientNames(
                string term)
        {
            var patients =
                await _patientService
                    .SearchByNameAsync(term);

            var result =
                patients
                    .Select(p => new
                    {
                        label = p.FullName,
                        value = p.PatientId
                    })
                    .ToList();

            return Json(
                result,
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult>
            GetDoctorsBySpecialisation(
                string specialisation)
        {
            var doctors =
                await _doctorService
                    .GetDoctorsBySpecialisationAsync(
                        specialisation);

            var result =
                doctors
                    .Where(d => d.IsActive)
                    .Select(d => new
                    {
                        DoctorId =
                            d.DoctorId,

                        FullName =
                            d.FullName
                    })
                    .ToList();

            return Json(
                result,
                JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult>
            GetAvailableSlots(
                int doctorId,
                DateTime scheduledDate)
        {
            var slots =
                await _appointmentService
                    .GetAvailableSlotsAsync(
                        doctorId,
                        scheduledDate);

            return Json(
                slots,
                JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult>
            ViewAppointments(
                string patientName)
        {
            var appointments =
                Enumerable.Empty
                    <AppointmentDto>();

            if (!string.IsNullOrWhiteSpace(
                patientName))
            {
                appointments =
                    await _appointmentService
                        .GetAppointmentsByPatientNameAsync(
                            patientName);
            }

            return View(appointments);
        }
    }
}