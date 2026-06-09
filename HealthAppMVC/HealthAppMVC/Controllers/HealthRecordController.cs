using HealthAppMVC.Models;
using HealthAppMVC.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class HealthRecordController
        : Controller
    {
        private readonly IHealthRecordService
            _healthRecordService;
        private readonly IPatientService _patientService;

        public HealthRecordController(
    IHealthRecordService healthRecordService,
    IPatientService patientService)
        {
            _healthRecordService =
                healthRecordService;

            _patientService =
                patientService;
        }

        // GET:
        // HealthRecord/Create?appointmentId=1
        public ActionResult Create(
            int appointmentId)
        {
            HealthRecord model =
                new HealthRecord
                {
                    AppointmentId =
                        appointmentId
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            HealthRecord record)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(record);
                }

                record = _healthRecordService
                     .AddHealthRecord(
                         record);

                TempData["Success"] =
                    "Health Record Added Successfully";

                return RedirectToAction(
                    "History",
                    new
                    {
                        patientId =
                        record.PatientId
                    });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(record);
            }
        }

        

        // HealthRecord/Details/1
        public ActionResult Details(
            int id)
        {
            var record =
                _healthRecordService
                .GetRecordById(id);

            if (record == null)
            {
                return HttpNotFound();
            }

            return View(record);
        }

        public ActionResult SearchPatientHistory(int? patientId)
        {
            IEnumerable<HealthRecord> records =
                Enumerable.Empty<HealthRecord>();

            if (patientId.HasValue)
            {
                records = _healthRecordService
                    .GetPatientHistory(patientId.Value);
            }

            return View(records);
        }

        public JsonResult SearchPatientNames(
    string term)
        {
            var patients =
                _patientService
                .SearchByName(term)
                .Select(p => new
                {
                    label = p.FullName,
                    value = p.PatientId
                })
                .ToList();

            return Json(
                patients,
                JsonRequestBehavior.AllowGet);
        }

        
    }
}