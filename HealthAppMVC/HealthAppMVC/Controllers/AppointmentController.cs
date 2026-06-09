using HealthAppMVC.Models;
using HealthAppMVC.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService
            _appointmentService;

        private readonly IPatientService
            _patientService;

        private readonly IDoctorService
            _doctorService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public ActionResult Index()
        {
            var appointments =
                _appointmentService
                .GetAllAppointments();

            return View(appointments);
        }

        [HttpGet]
        public ActionResult Create()
        {
            LoadDropdowns();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            Appointment appointment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    LoadDropdowns();

                    return View(appointment);
                }

                _appointmentService
                    .BookAppointment(
                        appointment);

                TempData["Success"] =
                    "Appointment booked successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                LoadDropdowns();

                return View(appointment);
            }
        }

        [HttpGet]
        public ActionResult Confirm(int id)
        {
           
                _appointmentService
                    .ConfirmAppointment(id);

                TempData["Success"] =
                    "Appointment confirmed.";

                return RedirectToAction("UpcomingAppointments");
            
        }

        [HttpGet]
        public ActionResult Cancel(int id)
        {
            Appointment appointment =
                _appointmentService
                .GetAppointmentById(id);

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(
            int id,
            string cancellationReason)
        {
            try
            {
                _appointmentService
                    .CancelAppointment(
                        id,
                        cancellationReason);

                TempData["Success"] =
                    "Appointment cancelled.";

                return RedirectToAction("UpcomingAppointments");
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

        private void LoadDropdowns()
        {
            ViewBag.Patients =
                new SelectList(
                    _patientService
                        .GetAllPatients(),
                    "PatientId",
                    "FullName");

            ViewBag.Doctors =
                new SelectList(
                    _doctorService
                        .GetAllDoctors(),
                    "DoctorId",
                    "FullName");
        }

        public ActionResult UpcomingAppointments(
    string doctorName = "")
        {
            IEnumerable<Appointment> app;


            var appointments = _appointmentService.GetUpcomingAppointments();

            if (!string.IsNullOrWhiteSpace(doctorName))
            {
                appointments = appointments
                    .Where(a => a.DoctorName
                    .ToLower()
                    .Contains(doctorName.ToLower()));
            }

            ViewBag.DoctorName = doctorName; // 🔥 IMPORTANT

            return View(appointments);
        }

        public JsonResult SearchDoctorNames(
    string term)
        {
            var doctors =
                _doctorService
                .SearchByName(term)
                .Select(d => new
                {
                    label = d.FullName,
                    value = d.FullName
                })
                .ToList();

            return Json(
                doctors,
                JsonRequestBehavior.AllowGet);
        }


    }
}