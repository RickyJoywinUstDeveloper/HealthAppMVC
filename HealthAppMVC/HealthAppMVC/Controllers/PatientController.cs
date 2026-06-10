using HealthAppMVC.Services.Interface;
using SharedDto.PatientDtos;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class PatientController
        : Controller
    {
        private readonly
            IPatientApiService
            _patientService;

        public PatientController(
            IPatientApiService patientService)
        {
            _patientService =
                patientService;
        }

        // GET: Patient
        public async Task<ActionResult>
            Index()
        {
            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            return View(patients);
        }

        // GET: Patient/Details/5
        public async Task<ActionResult>
            Details(int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Create(CreatePatientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                await _patientService
                    .CreatePatientAsync(dto);

                return RedirectToAction(
                    "Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(dto);
            }
        }

        // GET: Patient/Edit/5
        public async Task<ActionResult>
            Edit(int id)
        {
            var patient =
                await _patientService
                    .GetPatientByIdAsync(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            CreatePatientDto dto =
                new CreatePatientDto
                {
                    FullName =
                        patient.FullName,

                    DateOfBirth =
                        patient.DateOfBirth,

                    Gender =
                        patient.Gender,

                    Email =
                        patient.Email,

                    PhoneNumber =
                        patient.PhoneNumber,

                    InsuranceId =
                        patient.InsuranceId
                };

            return View(dto);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>
            Edit(
                int id,
                CreatePatientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                await _patientService
                    .UpdatePatientAsync(
                        id,
                        dto);

                return RedirectToAction(
                    "Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(dto);
            }
        }

        public ActionResult PatientServices()
        {
            return View();
        }

        public async Task<ActionResult>
            SearchPatient()
        {
            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            return View(patients);
        }

        public async Task<JsonResult>
            SearchPatientNames(
                string term)
        {
            var patients =
                await _patientService
                    .SearchByNameAsync(term);

            var result =
                patients.Select(p => new
                {
                    label = p.FullName,
                    value = p.FullName
                }).ToList();

            return Json(
                result,
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult
            EditPatientByName(int id)
        {
            return RedirectToAction(
                "Edit",
                new { id });
        }

        public async Task<ActionResult>
            PatientSearch(
                string patientName)
        {
            var patients =
                await _patientService
                    .GetAllPatientsAsync();

            if (!string.IsNullOrWhiteSpace(
                patientName))
            {
                patients =
                    patients.Where(p =>
                        p.FullName
                        .ToLower()
                        .Contains(
                            patientName
                            .ToLower()))
                    .ToList();
            }

            return View(patients);
        }
    }
}