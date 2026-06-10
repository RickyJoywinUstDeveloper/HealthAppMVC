using HealthAppWebApi.Services.Interface;
using SharedDto.PatientDtos;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthAppWebApi.Controllers
{
    [RoutePrefix("api/patients")]
    public class PatientsController : ApiController
    {
        private readonly IPatientService _service;

        public PatientsController(
            IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult>
            GetAll()
        {
            var patients =
                await _service
                    .GetAllPatientsAsync();

            return Ok(patients);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            Get(int id)
        {
            var patient =
                await _service
                    .GetPatientByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult>
            Create(CreatePatientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState);
                }

                await _service
                    .RegisterPatientAsync(dto);

                return Ok(
                    "Patient added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult>
            Update(
                int id,
                CreatePatientDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState);
                }

                await _service
                    .UpdatePatientAsync(
                        id,
                        dto);

                return Ok(
                    "Patient updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult>
            SearchByName(string name)
        {
            var patients =
                await _service
                    .SearchByNameAsync(name);

            return Ok(patients);
        }

        [HttpGet]
        [Route("{id:int}/appointmentcount")]
        public async Task<IHttpActionResult>
            GetAppointmentCount(
                int id)
        {
            int count =
                await _service
                    .GetAppointmentCountAsync(
                        id);

            return Ok(count);
        }
    }
}