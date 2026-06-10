using HealthAppMVC.Services.Interface;
using SharedDto.HealthRecordDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class HealthRecordController
        : Controller
    {
        private readonly
            IHealthRecordApiService
            _healthRecordService;

        private readonly
            IPatientApiService
            _patientService;

        public HealthRecordController(
            IHealthRecordApiService healthRecordService,
            IPatientApiService patientService)
        {
            _healthRecordService =
                healthRecordService;

            _patientService =
                patientService;
        }

        // GET:
        // HealthRecord/Create?appointmentId=1
        [HttpGet]
        public ActionResult Create(
            int appointmentId)
        {
            CreateHealthRecordDto dto =
                new CreateHealthRecordDto
                {
                    AppointmentId =
                        appointmentId
                };

            return View(dto);
        }

        // POST:
        // HealthRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(
                CreateHealthRecordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                await _healthRecordService
                    .AddAsync(dto);

                TempData["Success"] =
                    "Health Record Added Successfully";

                return RedirectToAction(
                    "SearchPatientHistory");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(dto);
            }
        }

        // GET:
        // HealthRecord/Details/1
        public async Task<ActionResult>
            Details(int id)
        {
            var record =
                await _healthRecordService
                    .GetByIdAsync(id);

            if (record == null)
            {
                return HttpNotFound();
            }

            return View(record);
        }

        // GET:
        // HealthRecord/SearchPatientHistory
        public async Task<ActionResult>
            SearchPatientHistory(
                int? patientId)
        {
            IEnumerable<HealthRecordDto>
                records =
                    Enumerable.Empty
                        <HealthRecordDto>();

            if (patientId.HasValue)
            {
                records =
                    await _healthRecordService
                        .GetPatientHistoryAsync(
                            patientId.Value);
            }

            return View(records);
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
    }
}
