using HealthAppMVC.Models;
using HealthAppMVC.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthAppMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(
            IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: Patient
        public ActionResult Index()
        {
            var patients =
                _patientService.GetAllPatients();

            return View(patients);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            var patient =
                _patientService.GetPatientById(id);

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
        public ActionResult Create(Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _patientService
                        .RegisterPatient(patient);

                    return RedirectToAction("Index");
                }

                return View(patient);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(patient);
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            var patient =
                _patientService.GetPatientById(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _patientService
                        .UpdatePatient(patient);

                    return RedirectToAction("Index");
                }

                return View(patient);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(patient);
            }
        }

        public ActionResult PatientServices()
        {
            return View();
        }

        public ActionResult SearchPatient()
        {
            var patients = _patientService.GetAllPatients();

            return View(patients);
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
                    value = p.FullName
                })
                .ToList();

            return Json(
                patients,
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPatientByName(
    int id)
        {
            return RedirectToAction(
                "Edit",
                new { id });
        }

        public ActionResult PatientSearch(
    string patientName)
        {
            var patients = _patientService.GetAllPatients();

            return View(patients);
        }


    }
}